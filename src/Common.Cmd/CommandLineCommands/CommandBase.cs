﻿using System;
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
                ? [currentDirectory]
                : assemblyService.GetCustomPaths(assemblyName));
#endif
            return assemblyLoadContext;
        }

        internal static string GetValidationResultWithRequiredShortName(IFileContentsProvider fileContentsProvider, string fileName, string assemblyName, string className, string shortName)
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

            if (string.IsNullOrEmpty(shortName))
            {
                return "Shortname is required.";
            }

            return string.Empty;
        }

        internal static string GetValidationResult(IFileContentsProvider fileContentsProvider, string fileName, string assemblyName, string className, string shortName)
        {
            int typeIndicatorCounter = 0;
            if (!string.IsNullOrEmpty(fileName)) typeIndicatorCounter++;
            if (!string.IsNullOrEmpty(assemblyName)) typeIndicatorCounter++;
            if (!string.IsNullOrEmpty(shortName)) typeIndicatorCounter++;

            if (typeIndicatorCounter == 0)
            {
                return "Either Filename, AssemblyName or ShortName is required.";
            }

            if (typeIndicatorCounter > 1)
            {
                return "You can either use Filename, AssemblyName or ShortName, not a combination of these.";
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

        internal static TemplateType GetTemplateType(string assemblyName)
            => string.IsNullOrEmpty(assemblyName)
                ? TemplateType.TextTemplate
                : TemplateType.AssemblyTemplate;

        [ExcludeFromCodeCoverage]
        internal static void LaunchDebuggerIfSet(CommandOption<string> debuggerOption)
        {
#if DEBUG
            debuggerOption.LaunchDebuggerIfSet();
#else
            // This method is left empty intentionally.
            // When not built for Debug build configuration, debuggerOption == null and there is no way we can launch the debugger.
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

        internal static void ModifyGlobalTemplate(
            CommandLineApplication app,
            IFileContentsProvider fileContentsProvider,
            Action<TemplateInfo> modifyAction,
            string commandName,
            string commandDescription,
            string resultText,
            bool allowParameters)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            app.Command(commandName, command =>
            {
                command.Description = commandDescription;
                var debuggerOption = GetDebuggerOption(command);
                var shortNameOption = command.Option<string>("-s|--shortname <NAME>", "The unique name of the template", CommandOptionType.SingleValue);
                var fileNameOption = command.Option<string>("-f|--filename <PATH>", "The template filename", CommandOptionType.SingleValue);
                var assemblyNameOption = command.Option<string>("-a|--assembly <ASSEMBLY>", "The template assembly", CommandOptionType.SingleValue);
                var classNameOption = command.Option<string>("-n|--classname <CLASS>", "The template class name", CommandOptionType.SingleValue);
                var parametersArgument = allowParameters
                    ? command.Argument("Parameters", "Optional parameters to use (name:value)", true)
                    : null;
                command.HelpOption();
                command.OnExecute(() =>
                {
                    LaunchDebuggerIfSet(debuggerOption);
                    var shortName = shortNameOption.Value();
                    var fileName = fileNameOption.Value();
                    var assemblyName = assemblyNameOption.Value();
                    var className = classNameOption.Value();

                    var validationResult = GetValidationResultWithRequiredShortName(fileContentsProvider, fileName, assemblyName, className, shortName);
                    if (!string.IsNullOrEmpty(validationResult))
                    {
                        app.Out.WriteLine($"Error: {validationResult}");
                        return;
                    }

                    var type = GetTemplateType(assemblyName);
                    var parameters = allowParameters
                        ? parametersArgument.Values.Where(p => p.Contains(':')).Select(p => new TemplateParameter { Name = p.Split(':')[0], Value = string.Join(":", p.Split(':').Skip(1)) }).ToArray()
                        : Array.Empty<TemplateParameter>();
                    
                    modifyAction(new TemplateInfo(shortName, fileName ?? string.Empty, assemblyName ?? string.Empty, className ?? string.Empty, type, parameters));
                    
                    app.Out.WriteLine(resultText);
                });
            });
        }
    }
}
