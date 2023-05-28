using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using TextCopy;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Runtime;
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

#if NETFRAMEWORK
                    var assembly = _assemblyService.LoadAssembly(assemblyName, System.Runtime.Loader.AssemblyLoadContext.Default);
#else
                    var context = new CustomAssemblyLoadContext("T4PlusCmd", true, () => currentDirectoryOption.HasValue()
                        ? new[] { currentDirectoryOption.Value() }
                        : _assemblyService.GetCustomPaths(assemblyName));
                    var assembly = _assemblyService.LoadAssembly(assemblyName, context);
#endif
                    var settings = CreateCodeGenerationSettings(generateMultipleFilesOption, dryRunOption, basePathOption);
                    var templateOutput = GetOutputFromAssembly(assembly, settings);

                    WriteOutput(app, templateOutput, bareOption, clipboardOption, settings.BasePath, settings.DryRun);
#if !NETFRAMEWORK
                    context.Unload();
#endif
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
            => assembly.GetExportedTypes().Where(t => !t.IsAbstract && !t.IsInterface && t.GetInterfaces().Any(i => i.FullName == "TextTemplateTransformationFramework.Runtime.CodeGeneration.ICodeGenerationProvider"))
                .Select(t => new CodeGenerationProviderWrapper(Activator.CreateInstance(t)));

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

        [ExcludeFromCodeCoverage]
        private sealed class CodeGenerationProviderWrapper : ICodeGenerationProvider
        {
            private readonly object _instance;

            public CodeGenerationProviderWrapper(object instance)
            {
                _instance = instance;
            }

            public bool GenerateMultipleFiles => (bool)_instance.GetType().GetProperty(nameof(GenerateMultipleFiles)).GetValue(_instance);

            public bool SkipWhenFileExists => (bool)_instance.GetType().GetProperty(nameof(SkipWhenFileExists)).GetValue(_instance);

            public string BasePath => (string)_instance.GetType().GetProperty(nameof(BasePath)).GetValue(_instance);

            public string Path => (string)_instance.GetType().GetProperty(nameof(Path)).GetValue(_instance);

            public string DefaultFileName => (string)_instance.GetType().GetProperty(nameof(DefaultFileName)).GetValue(_instance);

            public bool RecurseOnDeleteGeneratedFiles => (bool)_instance.GetType().GetProperty(nameof(RecurseOnDeleteGeneratedFiles)).GetValue(_instance);

            public string LastGeneratedFilesFileName => (string)_instance.GetType().GetProperty(nameof(LastGeneratedFilesFileName)).GetValue(_instance);

            public Action AdditionalActionDelegate => (Action)_instance.GetType().GetProperty(nameof(AdditionalActionDelegate)).GetValue(_instance);

            public object CreateAdditionalParameters() => _instance.GetType().GetMethod(nameof(CreateAdditionalParameters)).Invoke(_instance, Array.Empty<object>());

            public object CreateGenerator() => _instance.GetType().GetMethod(nameof(CreateGenerator)).Invoke(_instance, Array.Empty<object>());

            public object CreateModel() => _instance.GetType().GetMethod(nameof(CreateModel)).Invoke(_instance, Array.Empty<object>());

            public void Initialize(bool generateMultipleFiles, bool skipWhenFileExists, string basePath) => _instance.GetType().GetMethod(nameof(Initialize)).Invoke(_instance, new object[] { generateMultipleFiles, skipWhenFileExists, basePath });
        }
    }
}
