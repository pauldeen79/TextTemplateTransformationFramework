using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using TextCopy;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using TextTemplateTransformationFramework.Common.Cmd.Extensions;
using TextTemplateTransformationFramework.Runtime;
using TextTemplateTransformationFramework.Runtime.CodeGeneration;

namespace TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands
{
    public class RunCodeGenerationProviderAssemblyCommand : ICommandLineCommand
    {
        private readonly IClipboard _clipboard;

        public RunCodeGenerationProviderAssemblyCommand(IClipboard clipboard)
        {
            _clipboard = clipboard ?? throw new ArgumentNullException(nameof(clipboard));
        }

        public void Initialize(CommandLineApplication app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            app.Command("assembly", command =>
            {
                command.Description = "Runs all code generation providers from the specified assembly";

                var assemblyOption = command.Option<string>("-a|--assembly <PATH>", "The assembly name", CommandOptionType.SingleValue);
                var generateMultipleFilesOption = command.Option<bool>("-m|--multiple", "Indicator whether multiple files should be generated", CommandOptionType.SingleValue);
                var dryRunOption = command.Option<bool>("-r|--dryrun", "Indicator whether a dry run should be performed", CommandOptionType.SingleValue);
                var basePathOption = command.Option<string>("-p|--path", "Base path for code generation", CommandOptionType.SingleValue);
                var bareOption = command.Option<string>("-b|--bare", "Bare output (only template output)", CommandOptionType.NoValue);
                var clipboardOption = command.Option<string>("-c|--clipboard", "Copy output to clipboard", CommandOptionType.NoValue);

#if DEBUG
                var debuggerOption = command.Option<string>("-d|--launchdebugger", "Launches debugger", CommandOptionType.NoValue);
#endif

                command.HelpOption();
                command.OnExecute(() =>
                {
#if DEBUG
                    debuggerOption.LaunchDebuggerIfSet();
#endif
                    var assemblyName = assemblyOption.Value();
                    if (string.IsNullOrEmpty(assemblyName))
                    {
                        app.Error.WriteLine("Error: Assembly name is required.");
                        return;
                    }

                    var assembly = Assembly.Load(assemblyName);
                    var settings = CreateCodeGenerationSettings(generateMultipleFilesOption, dryRunOption, basePathOption);
                    var templateOutput = GetOutputFromAssembly(assembly, settings);

                    WriteOutput(app, templateOutput, bareOption, clipboardOption, settings.BasePath, settings.DryRun);
                });
            });
        }

        private static CodeGenerationSettings CreateCodeGenerationSettings(CommandOption<bool> generateMultipleFilesOption, CommandOption<bool> dryRunOption, CommandOption<string> basePathOption)
        {
            var generateMultipleFiles = generateMultipleFilesOption.HasValue() && generateMultipleFilesOption.ParsedValue;
            var dryRun = dryRunOption.HasValue() && dryRunOption.ParsedValue;
            var basePath = basePathOption.HasValue()
                ? basePathOption.Value()
                : null;

            return new CodeGenerationSettings(basePath, generateMultipleFiles, dryRun);
        }

        private static string GetOutputFromAssembly(Assembly assembly, CodeGenerationSettings settings)
        {
            var multipleContentBuilder = new MultipleContentBuilder { BasePath = settings.BasePath };
            foreach (var codeGenerationProvider in GetCodeGeneratorProviders(assembly))
            {
                GenerateCode.For(settings, multipleContentBuilder, codeGenerationProvider);
            }

            return multipleContentBuilder.ToString();
        }

        private static IEnumerable<ICodeGenerationProvider> GetCodeGeneratorProviders(Assembly assembly)
            => assembly.GetExportedTypes()
                .Where(t => !t.IsAbstract
                    && !t.IsInterface
                    && typeof(ICodeGenerationProvider).IsAssignableFrom(t)
                    && t.GetConstructors().Any(c => c.GetParameters().Length == 0)
                )
                .Select(t => (ICodeGenerationProvider)Activator.CreateInstance(t));

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
