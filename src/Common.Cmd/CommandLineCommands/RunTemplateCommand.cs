using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Loader;
using McMaster.Extensions.CommandLineUtils;
using TextCopy;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using TextTemplateTransformationFramework.Common.Contracts;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands
{
    public class RunTemplateCommand : ICommandLineCommand
    {
        private readonly ITextTemplateProcessor _processor;
        private readonly IFileContentsProvider _fileContentsProvider;
        private readonly ITemplateInfoRepository _templateInfoRepository;
        private readonly IUserInput _userInput;
        private readonly IClipboard _clipboard;
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable S4487 // Unread "private" fields should be removed
#pragma warning disable IDE0052 // Remove unread private members
        private readonly IAssemblyService _assemblyService;
#pragma warning restore IDE0052 // Remove unread private members
#pragma warning restore S4487 // Unread "private" fields should be removed
#pragma warning restore IDE0079 // Remove unnecessary suppression

        public RunTemplateCommand(ITextTemplateProcessor processor,
                                  IFileContentsProvider fileContentsProvider,
                                  ITemplateInfoRepository templateInfoRepository,
                                  IUserInput userInput,
                                  IClipboard clipboard,
                                  IAssemblyService assemblyService)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
            _fileContentsProvider = fileContentsProvider ?? throw new ArgumentNullException(nameof(fileContentsProvider));
            _templateInfoRepository = templateInfoRepository ?? throw new ArgumentNullException(nameof(templateInfoRepository));
            _userInput = userInput ?? throw new ArgumentNullException(nameof(userInput));
            _clipboard = clipboard ?? throw new ArgumentNullException(nameof(clipboard));
            _assemblyService = assemblyService ?? throw new ArgumentNullException(nameof(assemblyService));
        }

        public void Initialize(CommandLineApplication app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            app!.Command("run", command =>
            {
                command.Description = "Runs the template, and shows the template output";

                var shortNameOption = command.Option<string>("-s|--shortname <NAME>", "The unique name of the template", CommandOptionType.SingleValue);
                var fileNameOption = command.Option<string>("-f|--filename <PATH>", "The template filename", CommandOptionType.SingleValue);
                var outputOption = command.Option<string>("-o|--output <PATH>", "The output filename", CommandOptionType.SingleValue);
                var diagnosticDumpOutputOption = command.Option<string>("-diag|--diagnosticoutput <PATH>", "The diagnostic output filename", CommandOptionType.SingleValue);
                var parametersArgument = command.Argument("Parameters", "Optional parameters to use (name:value)", true);
                var interactiveOption = command.Option<string>("-i|--interactive", "Fill parameters interactively", CommandOptionType.NoValue);
                var bareOption = command.Option<string>("-b|--bare", "Bare output (only template output)", CommandOptionType.NoValue);
                var clipboardOption = command.Option<string>("-c|--clipboard", "Copy output to clipboard", CommandOptionType.NoValue);
                var assemblyNameOption = command.Option<string>("-a|--assembly <ASSEMBLY>", "The template assembly", CommandOptionType.SingleValue);
                var classNameOption = command.Option<string>("-n|--classname <CLASS>", "The template class name", CommandOptionType.SingleValue);
                var watchOption = command.Option<string>("-w|--watch", "Watches for file changes", CommandOptionType.NoValue);
                var currentDirectoryOption = CommandBase.GetCurrentDirectoryOption(command);
                var debuggerOption = CommandBase.GetDebuggerOption(command);
                command.HelpOption();
                command.OnExecute(() =>
                {
                    CommandBase.LaunchDebuggerIfSet(debuggerOption);
                    var fileName = fileNameOption.Value();
                    var assemblyName = assemblyNameOption.Value();
                    var className = classNameOption.Value();
                    var shortName = shortNameOption.Value();

                    var validationResult = CommandBase.GetValidationResult(_fileContentsProvider, fileName, assemblyName, className, shortName);
                    if (!string.IsNullOrEmpty(validationResult))
                    {
                        app.Out.WriteLine($"Error: {validationResult}");
                        return;
                    }

                    CommandBase.Watch(app, watchOption, !string.IsNullOrEmpty(fileName) ? fileName : assemblyName, () =>
                    {
                        var result = ProcessTemplate(app, parametersArgument, interactiveOption, currentDirectoryOption, (fileName, assemblyName, className, shortName));
                        if (!result.Success)
                        {
                            return;
                        }

                        if (!string.IsNullOrEmpty(result.ProcessResult.Exception))
                        {
                            app.Error.WriteLine("Exception occured while processing the template:");
                            app.Error.WriteLine(result.ProcessResult.Exception);
                            return;
                        }

                        var templateOutput = result.ProcessResult.Output;
                        var output = outputOption.Value();
                        var diagnosticDumpOutput = diagnosticDumpOutputOption.Value();

                        WriteOutput(app, result.ProcessResult, templateOutput, output, diagnosticDumpOutput, bareOption, clipboardOption);
#if !NETFRAMEWORK
                        result.AssemblyLoadContext?.Unload();
#endif
                    });
                });
            });
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable S1172 // Unused method parameters should be removed
        private (bool Success, ProcessResult ProcessResult, AssemblyLoadContext AssemblyLoadContext) ProcessTemplate(CommandLineApplication app,
                                                                                                                     CommandArgument parametersArgument,
                                                                                                                     CommandOption<string> interactiveOption,
                                                                                                                     CommandOption<string> currentDirectoryOption,
                                                                                                                     (string filename,
                                                                                                                     string assemblyName,
                                                                                                                     string className,
                                                                                                                     string shortName) names)
#pragma warning restore S1172 // Unused method parameters should be removed
#pragma warning restore IDE0079 // Remove unnecessary suppression
        {
            var parameters = parametersArgument.Values;
            if (!string.IsNullOrEmpty(names.shortName))
            {
                var info = _templateInfoRepository.FindByShortName(names.shortName);
                if (info == null)
                {
                    return (true, ProcessResult.Create(Array.Empty<CompilerError>(), string.Empty, string.Empty, string.Empty, string.Empty, new InvalidOperationException($"Could not find template with short name {names.shortName}")), null);
                }

                if (info.Type == TemplateType.TextTemplate)
                {
                    names.filename = info.FileName;
                }
                else
                {
                    names.assemblyName = info.AssemblyName;
                    names.className = info.ClassName;
                }
                parameters = MergeParameters(parameters, info.Parameters);
            }

            if (!string.IsNullOrEmpty(names.filename))
            {
                var x = ProcessTextTemplate(names.filename, app, interactiveOption.HasValue(), parameters);
                return (x.Success, x.Result, null);
            }
            else
            {
#if !NETFRAMEWORK
                var x = ProcessAssemblyTemplate(names.assemblyName, names.className, app, interactiveOption.HasValue(), parametersArgument.Values, currentDirectoryOption?.HasValue() == true, currentDirectoryOption?.HasValue() == true ? currentDirectoryOption!.Value() : null);
#else
                var x = ProcessAssemblyTemplate(names.assemblyName, names.className, app, interactiveOption.HasValue(), parametersArgument.Values, false, null);
#endif
#if !NETFRAMEWORK
                return (x.Success, x.Result, x.AssemblyLoadContext);
#else
                return (x.Success, x.Result, null);
#endif
            }
        }

        private ReadOnlyCollection<string> MergeParameters(IReadOnlyList<string> commandLineArgumentParameters, TemplateParameter[] globalTemplateParameters)
            => commandLineArgumentParameters
                .Where(p => p.Contains(':')).Select(p => new TemplateParameter { Name = p.Split(':')[0], Value = string.Join(":", p.Split(':').Skip(1)) })
                .Concat(globalTemplateParameters)
                .GroupBy(t => t.Name)
                .Select(x => $"{x.Key}:{x.First().Value}")
                .ToList()
                .AsReadOnly();

        private (bool Success, ProcessResult Result) ProcessTextTemplate(string filename, CommandLineApplication app, bool interactive, IEnumerable<string> parameterArguments)
        {
            var contents = _fileContentsProvider.GetFileContents(filename);
            var template = new TextTemplate(contents, filename);
            var parameters = GetParameters(template, app, interactive, parameterArguments);
            if (parameters == null)
            {
                return (false, null);
            }
            var result = _processor.Process(template, parameters);

            if (Array.Exists(result.CompilerErrors, e => !e.IsWarning))
            {
                app.Error.WriteLine("Compiler errors:");
                result.CompilerErrors.Select(err => err.ToString()).ForEach(app.Error.WriteLine);
                return (false, result);
            }

            return (true, result);
        }

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable S1172 // Unused method parameters should be removed
        private (bool Success, ProcessResult Result, AssemblyLoadContext AssemblyLoadContext) ProcessAssemblyTemplate(string assemblyName, string className, CommandLineApplication app, bool interactive, IEnumerable<string> parameterArguments, bool currentDirectoryIsFilled, string currentDirectory)
#pragma warning restore S1172 // Unused method parameters should be removed
#pragma warning restore IDE0079 // Remove unnecessary suppression
        {
            var assemblyLoadContext = CommandBase.CreateAssemblyLoadContext(_assemblyService, assemblyName, currentDirectoryIsFilled, currentDirectory);
            var template = new AssemblyTemplate(assemblyName, className, assemblyLoadContext);
            var parameters = GetParameters(template, app, interactive, parameterArguments);
            if (parameters == null)
            {
                return (false, null, assemblyLoadContext);
            }

            return (true, _processor.Process(template, parameters), assemblyLoadContext);
        }

        private TemplateParameter[] GetParameters(TextTemplate textTemplate,
                                                  CommandLineApplication app,
                                                  bool interactive,
                                                  IEnumerable<string> parameterArguments)
        {
            if (interactive)
            {
                var parameters = new List<TemplateParameter>();
                var parametersResult = _processor.ExtractParameters(textTemplate);
                return GetParameters(app, parameters, parametersResult);
            }

            return parameterArguments.Where(p => p.Contains(':')).Select(p => new TemplateParameter { Name = p.Split(':')[0], Value = string.Join(":", p.Split(':').Skip(1)) }).ToArray();
        }

        private TemplateParameter[] GetParameters(CommandLineApplication app, List<TemplateParameter> parameters, ExtractParametersResult parametersResult)
        {
            if (parametersResult.Exception != null)
            {
                app.Error.WriteLine("Exception occured while extracting parameters from the template:");
                app.Error.WriteLine(parametersResult.Exception);
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
                return null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null
            }
            foreach (var parameter in parametersResult.Parameters)
            {
                parameter.Value = _userInput.GetValue(parameter);
                parameters.Add(parameter);
            }

            return parameters.ToArray();
        }

        private TemplateParameter[] GetParameters(AssemblyTemplate assemblyTemplate,
                                                  CommandLineApplication app,
                                                  bool interactive,
                                                  IEnumerable<string> parameterArguments)
        {
            if (interactive)
            {
                var parameters = new List<TemplateParameter>();
                var parametersResult = _processor.ExtractParameters(assemblyTemplate);
                return GetParameters(app, parameters, parametersResult);
            }

            return parameterArguments.Where(p => p.Contains(':')).Select(p => new TemplateParameter { Name = p.Split(':')[0], Value = string.Join(":", p.Split(':').Skip(1)) }).ToArray();
        }

        private void WriteOutput(CommandLineApplication app, ProcessResult result, string templateOutput, string output, string diagnosticDumpOutput, CommandOption<string> bareOption, CommandOption<string> clipboardOption)
        {
            if (!string.IsNullOrEmpty(diagnosticDumpOutput))
            {
                WriteDiagnosticsDumpOutput(app, result, diagnosticDumpOutput, bareOption);
            }

            if (!string.IsNullOrEmpty(output))
            {
                WriteOutputToFile(app, templateOutput, output, bareOption);
            }
            else if (clipboardOption.HasValue())
            {
                WriteOutputToClipboard(app, templateOutput, bareOption);
            }
            else
            {
                WriteOutputToHost(app, templateOutput, bareOption);
            }
        }

        private void WriteDiagnosticsDumpOutput(CommandLineApplication app, ProcessResult result, string diagnosticDumpOutput, CommandOption<string> bareOption)
        {
            _fileContentsProvider.WriteFileContents(diagnosticDumpOutput, result.DiagnosticDump);
            if (!bareOption.HasValue())
            {
                app.Out.WriteLine($"Written diagnostic dump to file: {diagnosticDumpOutput}");
            }
        }

        private void WriteOutputToClipboard(CommandLineApplication app, string templateOutput, CommandOption<string> bareOption)
        {
            _clipboard.SetText(templateOutput);
            if (!bareOption.HasValue())
            {
                app.Out.WriteLine("Copied template output to clipboard");
            }
        }

        private static void WriteOutputToHost(CommandLineApplication app, string templateOutput, CommandOption<string> bareOption)
        {
            if (!bareOption.HasValue())
            {
                app.Out.WriteLine("Template output:");
            }
            app.Out.WriteLine(templateOutput);
        }

        private void WriteOutputToFile(CommandLineApplication app, string templateOutput, string output, CommandOption<string> bareOption)
        {
            _fileContentsProvider.WriteFileContents(output, templateOutput);
            if (!bareOption.HasValue())
            {
                app.Out.WriteLine($"Written template output to file: {output}");
            }
        }
    }
}
