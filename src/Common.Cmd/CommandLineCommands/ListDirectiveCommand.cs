using System;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands
{
    public class ListDirectiveCommand<TState> : ICommandLineCommand
        where TState : class
    {
        private readonly IScriptBuilder<TState> _scriptBuilder;

        public ListDirectiveCommand(IScriptBuilder<TState> scriptBuilder)
            => _scriptBuilder = scriptBuilder ?? throw new ArgumentNullException(nameof(scriptBuilder));

        public void Initialize(CommandLineApplication app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            app.Command("list-directive-arguments", command =>
            {
                command.Description = "Lists available directive arguments";

                var directiveNameOption = command.Option<string>("-n|--name <NAME>", "The directive name to list arguments for", CommandOptionType.SingleValue);
                var debuggerOption = CommandBase.GetDebuggerOption(command);

                command.HelpOption();
                command.OnExecute(() =>
                {
                    CommandBase.LaunchDebuggerIfSet(debuggerOption);
                    var directiveName = directiveNameOption.Value();
                    if (string.IsNullOrEmpty(directiveName))
                    {
                        app.Error.WriteLine("Error: Directive name is required.");
                        return;
                    }

                    var directive = _scriptBuilder.GetKnownDirectives().FirstOrDefault(d => d.GetDirectiveName() == directiveName);
                    if (directive == null)
                    {
                        app.Error.WriteLine($"Error: Could not find directive with name [{directiveName}]");
                        return;
                    }

                    var directiveModel = directive.GetModel();
                    var results = directiveModel
                        .GetType()
                        .GetProperties()
                        .Select(p => new { p.Name, p.PropertyType, DefaultValue = p.GetValue(directiveModel) })
                        .Select(p => $"{p.Name} ({p.PropertyType.FullName}) {p.DefaultValue.ToStringWithNullCheck()}");

                    app.Out.WriteLine("Arguments:");
                    results.ForEach(app.Out.WriteLine);
                });
            });
        }
    }
}
