using System;
using System.Diagnostics;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using TextTemplateTransformationFramework.Common.Cmd.Extensions;
using TextTemplateTransformationFramework.Common.Contracts;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands
{
    public class RunTemplateCommand : ICommandLineCommand
    {
        private readonly ITextTemplateProcessor _processor;
        private readonly IFileContentsProvider _fileContentsProvider;

        public RunTemplateCommand(ITextTemplateProcessor processor, IFileContentsProvider fileContentsProvider)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
            _fileContentsProvider = fileContentsProvider ?? throw new ArgumentNullException(nameof(processor));
        }

        public void Initialize(CommandLineApplication app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.Command("run", command =>
            {
                command.Description = "Runs the template, and shows the template output";

                var filenameOption = command.Option<string>("-f|--filename <PATH>", "The template filename", CommandOptionType.SingleValue);
                var outputOption = command.Option<string>("-o|--output <PATH>", "The output filename", CommandOptionType.SingleValue);
                var diagnosticDumpOutputOption = command.Option<string>("-diag|--diagnosticoutput <PATH>", "The diagnostic output filename", CommandOptionType.SingleValue);
                var parametersArgument = command.Argument("Parameters", "Optional parameters to use (name:value)", true);
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
                    var parameters = parametersArgument.Values.Where(p => p.Contains(":")).Select(p => new TemplateParameter { Name = p.Split(':')[0], Value = string.Join(":", p.Split(':').Skip(1)) }).ToArray();

                    if (string.IsNullOrEmpty(filename))
                    {
                        app.Error.WriteLine("Error: Filename is required.");
                        return;
                    }

                    if (!_fileContentsProvider.FileExists(filename))
                    {
                        app.Error.WriteLine($"Error: File [{filename}] does not exist.");
                        return;
                    }

                    var result = _processor.Process(new TextTemplate(_fileContentsProvider.GetFileContents(filename), filename), parameters);

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

                    var templateOutput = result.Output;
                    var output = outputOption.Value();
                    var diagnosticDumpOutput = diagnosticDumpOutputOption.Value();

                    WriteOutput(app, result, templateOutput, output, diagnosticDumpOutput);
                });
            });
        }

        private void WriteOutput(CommandLineApplication app, ProcessResult result, string templateOutput, string output, string diagnosticDumpOutput)
        {
            if (!string.IsNullOrEmpty(diagnosticDumpOutput))
            {
                _fileContentsProvider.WriteFileContents(diagnosticDumpOutput, result.DiagnosticDump);
                app.Out.WriteLine($"Written diagnostic dump to file: {diagnosticDumpOutput}");
            }

            if (!string.IsNullOrEmpty(output))
            {
                _fileContentsProvider.WriteFileContents(output, templateOutput);
                app.Out.WriteLine($"Written template output to file: {output}");
            }
            else
            {
                app.Out.WriteLine("Template output:");
                app.Out.WriteLine(templateOutput);
            }
        }
    }
}
