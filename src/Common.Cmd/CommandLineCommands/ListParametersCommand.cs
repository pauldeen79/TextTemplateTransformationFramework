using System;
using System.Linq;
using System.Runtime.Loader;
using McMaster.Extensions.CommandLineUtils;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using TextTemplateTransformationFramework.Common.Contracts;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands
{
    public class ListParametersCommand : ICommandLineCommand
    {
        private readonly ITextTemplateProcessor _processor;
        private readonly IFileContentsProvider _fileContentsProvider;
        private readonly ITemplateInfoRepository _templateInfoRepository;
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable S4487 // Unread "private" fields should be removed
#pragma warning disable IDE0052 // Remove unread private members
        private readonly IAssemblyService _assemblyService;
#pragma warning restore IDE0052 // Remove unread private members
#pragma warning restore S4487 // Unread "private" fields should be removed
#pragma warning restore IDE0079 // Remove unnecessary suppression

        public ListParametersCommand(ITextTemplateProcessor processor,
                                     IFileContentsProvider fileContentsProvider,
                                     ITemplateInfoRepository templateInfoRepository,
                                     IAssemblyService assemblyService)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
            _fileContentsProvider = fileContentsProvider ?? throw new ArgumentNullException(nameof(fileContentsProvider));
            _templateInfoRepository = templateInfoRepository ?? throw new ArgumentNullException(nameof(templateInfoRepository));
            _assemblyService = assemblyService ?? throw new ArgumentNullException(nameof(assemblyService));
        }

        public void Initialize(CommandLineApplication app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            app.Command("list-parameters", command =>
            {
                command.Description = "Lists template parameters";

                var shortNameOption = command.Option<string>("-s|--shortname <NAME>", "The unique name of the template", CommandOptionType.SingleValue);
                var fileNameOption = command.Option<string>("-f|--filename <PATH>", "The template filename", CommandOptionType.SingleValue);
                var assemblyNameOption = command.Option<string>("-a|--assembly <ASSEMBLY>", "The template assembly", CommandOptionType.SingleValue);
                var classNameOption = command.Option<string>("-n|--classname <CLASS>", "The template class name", CommandOptionType.SingleValue);
                var currentDirectoryOption = CommandBase.GetCurrentDirectoryOption(command);
                var debuggerOption = CommandBase.GetDebuggerOption(command);
                command.HelpOption();
                command.OnExecute(() =>
                {
                    CommandBase.LaunchDebuggerIfSet(debuggerOption);
                    var validationResult = CommandBase.GetValidationResult(_fileContentsProvider, fileNameOption.Value(), assemblyNameOption.Value(), classNameOption.Value(), shortNameOption.Value());
                    if (!string.IsNullOrEmpty(validationResult))
                    {
                        app.Error.WriteLine($"Error: {validationResult}");
                        return;
                    }

                    var assemblyLoadContext = CommandBase.CreateAssemblyLoadContext
                    (
                        _assemblyService,
                        assemblyNameOption.Value(),
                        currentDirectoryOption?.HasValue() == true,
                        currentDirectoryOption?.HasValue() == true
                            ? currentDirectoryOption!.Value()
                            : null
                    );
                    var result = ExtractParameters(fileNameOption.Value(), assemblyNameOption.Value(), classNameOption.Value(), shortNameOption.Value(), assemblyLoadContext);

                    if (Array.Exists(result.CompilerErrors, e => !e.IsWarning))
                    {
                        app.Error.WriteLine("Compiler errors:");
                        result.CompilerErrors.Select(err => err.ToString()).ForEach(app.Error.WriteLine);
                        return;
                    }

                    if (!string.IsNullOrEmpty(result.Exception))
                    {
                        app.Error.WriteLine("Exception occured:");
                        app.Error.WriteLine(result.Exception);
                        return;
                    }
#if !NETFRAMEWORK
                    assemblyLoadContext?.Unload();
#endif
                    result.Parameters.Select(p => $"{p.Name} ({p.Type.FullName})").ForEach(app.Out.WriteLine);
                });
            });
        }

        private ExtractParametersResult ExtractParameters(string fileName,
                                                          string assemblyName,
                                                          string className,
                                                          string shortName,
                                                          AssemblyLoadContext assemblyLoadContext)
        {
            if (!string.IsNullOrEmpty(shortName))
            {
                var info = _templateInfoRepository.FindByShortName(shortName);
                if (info == null)
                {
                    return ExtractParametersResult.Create(Enumerable.Empty<TemplateParameter>(), Array.Empty<CompilerError>(), string.Empty, string.Empty, new InvalidOperationException($"Could not find template with short name {shortName}"));
                }

                if (info.Type == TemplateType.TextTemplate)
                {
                    fileName = info.FileName;
                }
                else
                {
                    assemblyName = info.AssemblyName;
                    className = info.ClassName;
                }
            }

            return !string.IsNullOrEmpty(fileName)
                ? _processor.ExtractParameters(new TextTemplate(_fileContentsProvider.GetFileContents(fileName), fileName))
                : _processor.ExtractParameters(new AssemblyTemplate(assemblyName, className, assemblyLoadContext));
        }
    }
}
