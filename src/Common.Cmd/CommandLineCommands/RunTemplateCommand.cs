﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Loader;
using McMaster.Extensions.CommandLineUtils;
using TextCopy;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using TextTemplateTransformationFramework.Common.Cmd.Extensions;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Runtime.CodeGeneration;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands
{
    public class RunTemplateCommand : ICommandLineCommand
    {
        private readonly ITextTemplateProcessor _processor;
        private readonly IFileContentsProvider _fileContentsProvider;
        private readonly IUserInput _userInput;
        private readonly IClipboard _clipboard;
        private readonly IAssemblyService _assemblyService;

        public RunTemplateCommand(ITextTemplateProcessor processor,
                                  IFileContentsProvider fileContentsProvider,
                                  IUserInput userInput,
                                  IClipboard clipboard,
                                  IAssemblyService assemblyService)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
            _fileContentsProvider = fileContentsProvider ?? throw new ArgumentNullException(nameof(fileContentsProvider));
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

                var filenameOption = command.Option<string>("-f|--filename <PATH>", "The template filename", CommandOptionType.SingleValue);
                var outputOption = command.Option<string>("-o|--output <PATH>", "The output filename", CommandOptionType.SingleValue);
                var diagnosticDumpOutputOption = command.Option<string>("-diag|--diagnosticoutput <PATH>", "The diagnostic output filename", CommandOptionType.SingleValue);
                var parametersArgument = command.Argument("Parameters", "Optional parameters to use (name:value)", true);
                var interactiveOption = command.Option<string>("-i|--interactive", "Fill parameters interactively", CommandOptionType.NoValue);
                var bareOption = command.Option<string>("-b|--bare", "Bare output (only template output)", CommandOptionType.NoValue);
                var clipboardOption = command.Option<string>("-c|--clipboard", "Copy output to clipboard", CommandOptionType.NoValue);
                var assemblyNameOption = command.Option<string>("-a|--assembly <ASSEMBLY>", "The template assembly", CommandOptionType.SingleValue);
                var classNameOption = command.Option<string>("-n|--classname <CLASS>", "The template class name", CommandOptionType.SingleValue);
#if !NETFRAMEWORK
                var currentDirectoryOption = command.Option<string>("-u|--use", "Use different current directory", CommandOptionType.SingleValue);
#endif

#if DEBUG
                var debuggerOption = command.Option<string>("-d|--launchdebugger", "Launches debugger", CommandOptionType.NoValue);
#endif

                command.HelpOption();
                command.OnExecute(() =>
                {
#if DEBUG
                    debuggerOption.LaunchDebuggerIfSet();
#endif
                    var filename = filenameOption.Value();
                    var assemblyName = assemblyNameOption.Value();
                    var className = classNameOption.Value();

                    if (string.IsNullOrEmpty(filename) && string.IsNullOrEmpty(assemblyName))
                    {
                        app.Error.WriteLine("Error: Either Filename or AssemblyName is required.");
                        return;
                    }

                    if (!string.IsNullOrEmpty(filename) && !string.IsNullOrEmpty(assemblyName))
                    {
                        app.Error.WriteLine("Error: You can either use Filename or AssemblyName, not both.");
                        return;
                    }

                    if (!string.IsNullOrEmpty(assemblyName) && string.IsNullOrEmpty(className))
                    {
                        app.Error.WriteLine("Error: When AssemblyName is filled, then ClassName is required.");
                        return;
                    }

                    if (!_fileContentsProvider.FileExists(filename))
                    {
                        app.Error.WriteLine($"Error: File [{filename}] does not exist.");
                        return;
                    }
                    ProcessResult result;
                    AssemblyLoadContext assemblyLoadContext = null;
                    if (!string.IsNullOrEmpty(filename))
                    {
                        var contents = _fileContentsProvider.GetFileContents(filename);
                        var template = new TextTemplate(contents, filename);
                        var parameters = GetParameters(template, app, interactiveOption, parametersArgument);
                        if (parameters == null)
                        {
                            return;
                        }
                        result = _processor.Process(template, parameters);

                        if (result.CompilerErrors.Any(e => !e.IsWarning))
                        {
                            app.Error.WriteLine("Compiler errors:");
                            result.CompilerErrors.Select(err => err.ToString()).ForEach(app.Error.WriteLine);
                            return;
                        }
                    }
                    else
                    {
#if NETFRAMEWORK
                    assemblyLoadContext = System.Runtime.Loader.AssemblyLoadContext.Default;
#else
                    assemblyLoadContext = new CustomAssemblyLoadContext("T4PlusCmd", true, () => currentDirectoryOption.HasValue()
                        ? new[] { currentDirectoryOption.Value() }
                        : _assemblyService.GetCustomPaths(assemblyName));
#endif
                        var template = new AssemblyTemplate(assemblyName, className, assemblyLoadContext);
                        var parameters = GetParameters(template, app, interactiveOption, parametersArgument);
                        if (parameters == null)
                        {
                            return;
                        }
                        result = _processor.Process(template, parameters);
                    }

                    if (!string.IsNullOrEmpty(result.Exception))
                    {
                        app.Error.WriteLine("Exception occured while processing the template:");
                        app.Error.WriteLine(result.Exception);
                        return;
                    }

                    var templateOutput = result.Output;
                    var output = outputOption.Value();
                    var diagnosticDumpOutput = diagnosticDumpOutputOption.Value();

                    WriteOutput(app, result, templateOutput, output, diagnosticDumpOutput, bareOption, clipboardOption);
#if !NETFRAMEWORK
                    assemblyLoadContext?.Unload();
#endif
                });
            });
        }

        private TemplateParameter[] GetParameters(TextTemplate textTemplate,
                                                  CommandLineApplication app,
                                                  CommandOption<string> interactiveOption,
                                                  CommandArgument parametersArgument)
        {
            if (interactiveOption.HasValue())
            {
                var parameters = new List<TemplateParameter>();
                var parametersResult = _processor.ExtractParameters(textTemplate);
                return GetParameters(app, parameters, parametersResult);
            }

            return parametersArgument.Values.Where(p => p.Contains(':')).Select(p => new TemplateParameter { Name = p.Split(':')[0], Value = string.Join(":", p.Split(':').Skip(1)) }).ToArray();
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
                                                  CommandOption<string> interactiveOption,
                                                  CommandArgument parametersArgument)
        {
            if (interactiveOption.HasValue())
            {
                var parameters = new List<TemplateParameter>();
                var parametersResult = _processor.ExtractParameters(assemblyTemplate);
                return GetParameters(app, parameters, parametersResult);
            }

            return parametersArgument.Values.Where(p => p.Contains(':')).Select(p => new TemplateParameter { Name = p.Split(':')[0], Value = string.Join(":", p.Split(':').Skip(1)) }).ToArray();
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
