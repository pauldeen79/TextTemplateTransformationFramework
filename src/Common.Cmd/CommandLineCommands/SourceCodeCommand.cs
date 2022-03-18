using System;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using TextTemplateTransformationFramework.Common.Cmd.Extensions;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands
{
    public class SourceCodeCommand : ICommandLineCommand
    {
        private readonly ITextTemplateProcessor _processor;
        private readonly IFileContentsProvider _fileContentsProvider;

        public SourceCodeCommand(ITextTemplateProcessor processor, IFileContentsProvider fileContentsProvider)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
            _fileContentsProvider = fileContentsProvider ?? throw new ArgumentNullException(nameof(processor));
        }

        public void Initialize(CommandLineApplication app)
        {
            app.Command("source", command =>
            {
                command.Description = "Generates the template, and shows the template source code";

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

                    var result = _processor.PreProcess(new TextTemplate(_fileContentsProvider.GetFileContents(filename), filename), parameters);

                    if (!string.IsNullOrEmpty(result.Exception))
                    {
                        app.Error.WriteLine("Exception occured:");
                        app.Error.WriteLine(result.Exception);
                        return;
                    }

                    var sourceCode = result.SourceCode;
                    var output = outputOption.Value();
                    var diagnosticDumpOutput = diagnosticDumpOutputOption.Value();

                    WriteOutput(app, result, sourceCode, output, diagnosticDumpOutput);
                });
            });
        }

        private void WriteOutput(CommandLineApplication app, ProcessResult result, string sourceCode, string output, string diagnosticDumpOutput)
        {
            if (!string.IsNullOrEmpty(diagnosticDumpOutput))
            {
                _fileContentsProvider.WriteFileContents(diagnosticDumpOutput, result.DiagnosticDump);
                app.Out.WriteLine($"Written diagnostic dump to file: {diagnosticDumpOutput}");
            }

            if (!string.IsNullOrEmpty(output))
            {
                _fileContentsProvider.WriteFileContents(output, sourceCode);
                app.Out.WriteLine($"Written source code output to file: {output}");
            }
            else
            {
                app.Out.WriteLine("Source code output:");
                app.Out.WriteLine(sourceCode);
            }
        }
    }
}
