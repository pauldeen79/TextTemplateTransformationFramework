using System;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;
using Utilities.Extensions;

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
            if (app == null) throw new ArgumentNullException(nameof(app));
            app.Command("list-directives", command =>
            {
                command.Description = "Lists available directives";
                var debuggerOption = CommandBase.GetDebuggerOption(command);
                command.HelpOption();
                command.OnExecute(() =>
                {
                    CommandBase.LaunchDebuggerIfSet(debuggerOption);
                    _scriptBuilder.GetKnownDirectives().Select(p => p.GetDirectiveName())
                                                       .OrderBy(s => s)
                                                       .ForEach(app.Out.WriteLine);
                });
            });
        }
    }
}
