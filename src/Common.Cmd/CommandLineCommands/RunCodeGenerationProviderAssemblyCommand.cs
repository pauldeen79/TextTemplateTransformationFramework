using System;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using TextCopy;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Runtime.CodeGeneration;

namespace TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands
{
    public class RunCodeGenerationProviderAssemblyCommand : ICommandLineCommand
    {
        private readonly IClipboard _clipboard;
        private readonly IAssemblyService _assemblyService;

        public RunCodeGenerationProviderAssemblyCommand(IClipboard clipboard, IAssemblyService assemblyService)
        {
            _clipboard = clipboard ?? throw new ArgumentNullException(nameof(clipboard));
            _assemblyService = assemblyService ?? throw new ArgumentNullException(nameof(assemblyService));
        }

        public void Initialize(CommandLineApplication app)
        {
            if (app is null) throw new ArgumentNullException(nameof(app));
            app.Command("assembly", command =>
            {
                command.Description = "Runs all code generation providers from the specified assembly";

                var assemblyOption = command.Option<string>("-a|--assembly <PATH>", "The assembly name", CommandOptionType.SingleValue);
                var watchOption = command.Option<string>("-w|--watch", "Watches for file changes", CommandOptionType.NoValue);
                var generateMultipleFilesOption = command.Option<bool>("-m|--multiple", "Indicator whether multiple files should be generated", CommandOptionType.SingleValue);
                var dryRunOption = command.Option<bool>("-r|--dryrun", "Indicator whether a dry run should be performed", CommandOptionType.SingleValue);
                var basePathOption = command.Option<string>("-p|--path", "Base path for code generation", CommandOptionType.SingleValue);
                var bareOption = command.Option<string>("-b|--bare", "Bare output (only template output)", CommandOptionType.NoValue);
                var clipboardOption = command.Option<string>("-c|--clipboard", "Copy output to clipboard", CommandOptionType.NoValue);
                var filterClassNameOption = command.Option<string>("-f|--filter <CLASSNAME>", "Filter code generation provider by class name", CommandOptionType.MultipleValue);
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable S1481 // Unused local variables should be removed
                var currentDirectoryOption = CommandBase.GetCurrentDirectoryOption(command);
#pragma warning restore S1481 // Unused local variables should be removed
#pragma warning restore IDE0079 // Remove unnecessary suppression
                var debuggerOption = CommandBase.GetDebuggerOption(command);
                command.HelpOption();
                command.OnExecute(() =>
                {
                    CommandBase.LaunchDebuggerIfSet(debuggerOption);
                    var assemblyName = assemblyOption.Value();
                    if (string.IsNullOrEmpty(assemblyName))
                    {
                        app.Error.WriteLine("Error: Assembly name is required.");
                        return;
                    }

                    var basePath = basePathOption.HasValue()
                        ? basePathOption.Value()
                        : null;
                    var dryRun = dryRunOption.HasValue() && dryRunOption.ParsedValue;
                    CommandBase.Watch(app, watchOption, assemblyName, () =>
                    {
                        using var codeGenerationAssembly = new CodeGenerationAssembly
                        (
                            assemblyName,
                            basePath,
                            generateMultipleFilesOption.HasValue() && generateMultipleFilesOption.ParsedValue,
                            dryRun,
                            currentDirectoryOption.HasValue()
                                ? currentDirectoryOption.Value()
                                : _assemblyService.GetCustomPaths(assemblyName).FirstOrDefault(),
                            filterClassNameOption.Values
                        );
                        var templateOutput = codeGenerationAssembly.Generate();
                        WriteOutput(app, templateOutput, bareOption, clipboardOption, basePath, dryRun);
                    });
                });
            });
        }

        private void WriteOutput(CommandLineApplication app, string templateOutput, CommandOption<string> bareOption, CommandOption<string> clipboardOption, string basePath, bool dryRun)
        {
            if (!string.IsNullOrEmpty(basePath) && !dryRun)
            {
                if (!bareOption.HasValue())
                {
                    app.Out.WriteLine($"Written code generation output to path: {basePath}");
                }
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

        private void WriteOutputToClipboard(CommandLineApplication app, string templateOutput, CommandOption<string> bareOption)
        {
            _clipboard.SetText(templateOutput);
            if (!bareOption.HasValue())
            {
                app.Out.WriteLine("Copied code generation output to clipboard");
            }
        }

        private static void WriteOutputToHost(CommandLineApplication app, string templateOutput, CommandOption<string> bareOption)
        {
            if (!bareOption.HasValue())
            {
                app.Out.WriteLine("Code generation output:");
            }
            app.Out.WriteLine(templateOutput);
        }
    }
}
