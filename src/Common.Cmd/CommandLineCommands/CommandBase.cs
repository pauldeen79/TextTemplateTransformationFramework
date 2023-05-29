using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using System.Threading;
using McMaster.Extensions.CommandLineUtils;
using TextTemplateTransformationFramework.Common.Cmd.Extensions;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.Runtime.CodeGeneration;

namespace TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands
{
    internal static class CommandBase
    {
        internal static CommandOption<string> GetCurrentDirectoryOption(CommandLineApplication app)
        {
#if !NETFRAMEWORK
            return app.Option<string>("-u|--use", "Use different current directory", CommandOptionType.SingleValue);
#else
            return null;
#endif
        }

        [ExcludeFromCodeCoverage]
        internal static CommandOption<string> GetDebuggerOption(CommandLineApplication app)
        {
#if DEBUG
            return app.Option<string>("-d|--launchdebugger", "Launches debugger", CommandOptionType.NoValue);
#else
            return null;
#endif
        }

        internal static AssemblyLoadContext CreateAssemblyLoadContext(IAssemblyService assemblyService, string assemblyName, bool currentDirectoryIsFilled, string currentDirectory)
        {
            AssemblyLoadContext assemblyLoadContext;
#if NETFRAMEWORK
            assemblyLoadContext = AssemblyLoadContext.Default;
#else
            assemblyLoadContext = new CustomAssemblyLoadContext("T4PlusCmd", true, () => currentDirectoryIsFilled
                ? new[] { currentDirectory }
                : assemblyService.GetCustomPaths(assemblyName));
#endif
            return assemblyLoadContext;
        }

        internal static string GetValidationResult(IFileContentsProvider fileContentsProvider, string fileName, string assemblyName, string className)
        {
            if (string.IsNullOrEmpty(fileName) && string.IsNullOrEmpty(assemblyName))
            {
                return "Either Filename or AssemblyName is required.";
            }

            if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(assemblyName))
            {
                return "You can either use Filename or AssemblyName, not both.";
            }

            if (!string.IsNullOrEmpty(assemblyName) && string.IsNullOrEmpty(className))
            {
                return "When AssemblyName is filled, then ClassName is required.";
            }

            if (!string.IsNullOrEmpty(fileName) && !fileContentsProvider.FileExists(fileName))
            {
                return $"File [{fileName}] does not exist.";
            }

            return null;
        }

        [ExcludeFromCodeCoverage]
        internal static void LaunchDebuggerIfSet(CommandOption<string> debuggerOption)
        {
#if DEBUG
            debuggerOption.LaunchDebuggerIfSet();
#else
            // This method is left empty intentionally.
            // When not built for Debug build configuration, debuggerOption is null and there is no way we can launch the debugger.
#endif
        }

        internal static (bool IsSuccessful, string ErrorMessage, ITemplateSectionProcessor<TState> Directive) GetDirectiveAndModel<TState>(
            CommandOption<string> directiveNameOption,
            IScriptBuilder<TState> scriptBuilder) where TState : class
        {
            var directiveName = directiveNameOption.Value();
            if (string.IsNullOrEmpty(directiveName))
            {
                return (false, "Directive name is required.", null);
            }

            var directive = scriptBuilder.GetKnownDirectives().FirstOrDefault(d => d.GetDirectiveName() == directiveName);
            if (directive == null)
            {
                return (false, $"Could not find directive with name [{directiveName}]", null);
            }

            return (true, null, directive);
        }

        [ExcludeFromCodeCoverage]
        internal static void Watch(CommandLineApplication app, CommandOption<string> watchOption, string fileName, Action action)
        {
            action();

            if (!watchOption.HasValue())
            {
                return;
            }

            if (!File.Exists(fileName))
            {
                app.Out.WriteLine($"Error: Could not find file [{fileName}]. Could not watch file for changes.");
                return;
            }

            app.Out.WriteLine($"Watching file [{fileName}] for changes...");
            var previousLastWriteTime = new FileInfo(fileName).LastWriteTime;
            while (true)
            {
                if (!File.Exists(fileName))
                {
                    app.Out.WriteLine($"Error: Could not find file [{fileName}]. Could not watch file for changes.");
                    return;
                }
                var currentLastWriteTime = new FileInfo(fileName).LastWriteTime;
                if (currentLastWriteTime != previousLastWriteTime)
                {
                    previousLastWriteTime = currentLastWriteTime;
                    action();
                }
                Thread.Sleep(1000);
            }
        }
    }
}
