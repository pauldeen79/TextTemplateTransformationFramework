using McMaster.Extensions.CommandLineUtils;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands
{
    public class SourceCodeCommand : ICommandLineCommand
    {
        private readonly ITextTemplateProcessor _processor;

        public SourceCodeCommand(ITextTemplateProcessor processor)
            => _processor = processor ?? throw new ArgumentNullException(nameof(processor));

        public void Initialize(CommandLineApplication app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

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
                    if (debuggerOption.HasValue())
                    {
                        Debugger.Launch();
                    }
#endif
                    var filename = filenameOption.Value();
                    var parameters = parametersArgument.Values.Where(p => p.Contains(":")).Select(p => new TemplateParameter { Name = p.Split(':')[0], Value = string.Join(":", p.Split(':').Skip(1)) }).ToArray();

                    if (string.IsNullOrEmpty(filename))
                    {
                        app.Error.WriteLine("Error: Filename is required.");
                        return;
                    }

                    if (!File.Exists(filename))
                    {
                        app.Error.WriteLine($"Error: File [{filename}] does not exist.");
                        return;
                    }

                    var result = _processor.PreProcess(new TextTemplate(File.ReadAllText(filename), filename), parameters);

                    if (!string.IsNullOrEmpty(result.Exception))
                    {
                        app.Error.WriteLine("Exception occured:");
                        app.Error.WriteLine(result.Exception);
                        return;
                    }

                    var sourceCode = result.SourceCode;
                    var output = outputOption.Value();
                    var diagnosticDumpOutput = diagnosticDumpOutputOption.Value();

                    if (!string.IsNullOrEmpty(diagnosticDumpOutput))
                    {
                        File.WriteAllText(diagnosticDumpOutput, result.DiagnosticDump);
                        app.Out.WriteLine($"Written diagnostic dump to file: {diagnosticDumpOutput}");
                    }

                    if (!string.IsNullOrEmpty(output))
                    {
                        File.WriteAllText(output, sourceCode);
                        app.Out.WriteLine($"Written source code output to file: {output}");
                    }
                    else
                    {
                        app.Out.WriteLine("Source code output:");
                        app.Out.WriteLine(sourceCode);
                    }
                });
            });
        }
    }
}
