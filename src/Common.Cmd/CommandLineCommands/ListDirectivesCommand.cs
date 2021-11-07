using McMaster.Extensions.CommandLineUtils;
using System;
using System.Diagnostics;
using System.Linq;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;

namespace TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands
{
    public class ListDirectivesCommand<TState> : ICommandLineCommand
        where TState : class
    {
        private readonly IScriptBuilder<TState> _scriptBuilder;

        public ListDirectivesCommand(IScriptBuilder<TState> scriptBuilder)
            => _scriptBuilder = scriptBuilder ?? throw new ArgumentNullException(nameof(scriptBuilder));

        public void Initialize(CommandLineApplication app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.Command("list-directives", command =>
            {
                command.Description = "Lists available directives";

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
                    foreach (var directive in _scriptBuilder.GetKnownDirectives().Select(p => p.GetDirectiveName()).OrderBy(s => s))
                    {
                        app.Out.WriteLine(directive);
                    }
                });
            });
        }
    }
}
