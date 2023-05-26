using System;
using System.Linq;
using System.Runtime.Loader;
using McMaster.Extensions.CommandLineUtils;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using TextTemplateTransformationFramework.Common.Cmd.Extensions;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Runtime.CodeGeneration;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands
{
    public class ListParametersCommand : ICommandLineCommand
    {
        private readonly ITextTemplateProcessor _processor;
        private readonly IFileContentsProvider _fileContentsProvider;
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable S4487 // Unread "private" fields should be removed
#pragma warning disable IDE0052 // Remove unread private members
        private readonly IAssemblyService _assemblyService;
#pragma warning restore IDE0052 // Remove unread private members
#pragma warning restore S4487 // Unread "private" fields should be removed
#pragma warning restore IDE0079 // Remove unnecessary suppression

        public ListParametersCommand(ITextTemplateProcessor processor,
                                     IFileContentsProvider fileContentsProvider,
                                     IAssemblyService assemblyService)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
            _fileContentsProvider = fileContentsProvider ?? throw new ArgumentNullException(nameof(fileContentsProvider));
            _assemblyService = assemblyService ?? throw new ArgumentNullException(nameof(assemblyService));
        }

        public void Initialize(CommandLineApplication app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            app.Command("list-parameters", command =>
            {
                command.Description = "Lists template parameters";

                var filenameOption = command.Option<string>("-f|--filename <PATH>", "The template filename", CommandOptionType.SingleValue);
                var assemblyNameOption = command.Option<string>("-a|--assembly <ASSEMBLY>", "The template assembly", CommandOptionType.SingleValue);
                var classNameOption = command.Option<string>("-n|--classname <CLASS>", "The template class name", CommandOptionType.SingleValue);
                var currentDirectoryOption = CommandBase.GetCurrentDirectoryOption(command);
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

                    var validationResult = CommandBase.GetValidationResult(_fileContentsProvider, filename, assemblyName, className);
                    if (!string.IsNullOrEmpty(validationResult))
                    {
                        app.Error.WriteLine($"Error: {validationResult}");
                        return;
                    }

                    var assemblyLoadContext = CommandBase.CreateAssemblyLoadContext(_assemblyService, assemblyName, currentDirectoryOption?.HasValue() == true, currentDirectoryOption?.HasValue() == true ? currentDirectoryOption!.Value() : null);
                    var result = ExtractParameters(filename, assemblyName, className, assemblyLoadContext);

                    if (result.CompilerErrors.Any(e => !e.IsWarning))
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

        private ExtractParametersResult ExtractParameters(string filename,
                                                          string assemblyName,
                                                          string className,
                                                          AssemblyLoadContext assemblyLoadContext)
            => !string.IsNullOrEmpty(filename)
                ? _processor.ExtractParameters(new TextTemplate(_fileContentsProvider.GetFileContents(filename), filename))
                : _processor.ExtractParameters(new AssemblyTemplate(assemblyName, className, assemblyLoadContext));
    }
}
