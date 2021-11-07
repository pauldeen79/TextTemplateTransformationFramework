using McMaster.Extensions.CommandLineUtils;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands
{
    public class ListParametersCommand : ICommandLineCommand
    {
        private readonly ITextTemplateProcessor _processor;

        public ListParametersCommand(ITextTemplateProcessor processor)
            => _processor = processor ?? throw new ArgumentNullException(nameof(processor));

        public void Initialize(CommandLineApplication app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.Command("list-parameters", command =>
            {
                command.Description = "Lists template parameters";

                var filenameOption = command.Option<string>("-f|--filename <PATH>", "The template filename", CommandOptionType.SingleValue);

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

                    var result = _processor.ExtractParameters(new TextTemplate(File.ReadAllText(filename), filename));

                    if (result.CompilerErrors.Any(e => !e.IsWarning))
                    {
                        app.Error.WriteLine("Compiler errors:");
                        foreach (var error in result.CompilerErrors.Select(err => err.ToString()))
                        {
                            app.Error.WriteLine(error);
                        }
                        return;
                    }

                    if (!string.IsNullOrEmpty(result.Exception))
                    {
                        app.Error.WriteLine("Exception occured:");
                        app.Error.WriteLine(result.Exception);
                        return;
                    }

                    foreach (var parameter in result.Parameters.Select(p => $"{p.Name} ({p.Type.FullName})"))
                    {
                        app.Out.WriteLine(parameter);
                    }
                });
            });
        }
    }
}
