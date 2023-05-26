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
                var currentDirectoryOption = GetCurrentDirectoryOption(command);
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

                    var validationResult = Validate(filename, assemblyName, className);
                    if (!string.IsNullOrEmpty(validationResult))
                    {
                        app.Error.WriteLine($"Error: {validationResult}");
                        return;
                    }

                    var assemblyLoadContext = CreateAssemblyContext(currentDirectoryOption, assemblyName);
                    var result = !string.IsNullOrEmpty(filename)
                        ? _processor.ExtractParameters(new TextTemplate(_fileContentsProvider.GetFileContents(filename), filename))
                        : _processor.ExtractParameters(new AssemblyTemplate(assemblyName, className, assemblyLoadContext));

                    if (result.CompilerErrors.Any(e => !e.IsWarning))
                    {
                        app.Error.WriteLine("Compiler errors:");
                        result.CompilerErrors.Select(err => err.ToString()).ToList().ForEach(app.Error.WriteLine);
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

        private static CommandOption<string> GetCurrentDirectoryOption(CommandLineApplication command)
        {
            CommandOption<string> currentDirectoryOption;
#if !NETFRAMEWORK
            currentDirectoryOption = command.Option<string>("-u|--use", "Use different current directory", CommandOptionType.SingleValue);
#else
            currentDirectoryOption = null;
#endif
            return currentDirectoryOption;
        }

        private AssemblyLoadContext CreateAssemblyContext(CommandOption<string> currentDirectoryOption, string assemblyName)
        {
            AssemblyLoadContext assemblyLoadContext;
#if NETFRAMEWORK
            assemblyLoadContext = AssemblyLoadContext.Default;
#else
            assemblyLoadContext = new CustomAssemblyLoadContext("T4PlusCmd", true, () => currentDirectoryOption.HasValue()
                ? new[] { currentDirectoryOption.Value() }
                : _assemblyService.GetCustomPaths(assemblyName));
#endif
            return assemblyLoadContext;
        }

        private string Validate(string filename, string assemblyName, string className)
        {
            if (string.IsNullOrEmpty(filename) && string.IsNullOrEmpty(assemblyName))
            {
                return "Either Filename or AssemblyName is required.";
            }

            if (!string.IsNullOrEmpty(filename) && !string.IsNullOrEmpty(assemblyName))
            {
                return "You can either use Filename or AssemblyName, not both.";
            }

            if (!string.IsNullOrEmpty(assemblyName) && string.IsNullOrEmpty(className))
            {
                return "When AssemblyName is filled, then ClassName is required.";
            }

            if (!string.IsNullOrEmpty(filename) && !_fileContentsProvider.FileExists(filename))
            {
                return $"File [{filename}] does not exist.";
            }

            return null;
        }
    }
}
