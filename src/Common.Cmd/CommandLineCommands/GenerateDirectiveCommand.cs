using System;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;

namespace TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands
{
    public class GenerateDirectiveCommand<TState> : ICommandLineCommand
        where TState : class
    {
        private readonly IScriptBuilder<TState> _scriptBuilder;

        public GenerateDirectiveCommand(IScriptBuilder<TState> scriptBuilder)
            => _scriptBuilder = scriptBuilder ?? throw new ArgumentNullException(nameof(scriptBuilder));

        public void Initialize(CommandLineApplication app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            app.Command("generate-directive", command =>
            {
                command.Description = "Generates a directive with specified arguments";

                var directiveNameOption = command.Option<string>("-n|--name <NAME>", "The directive name to generate", CommandOptionType.SingleValue);
                var parametersArgument = command.Argument("Parameters", "Optional parameters to use (name:value)", true);
                var debuggerOption = CommandBase.GetDebuggerOption(command);
                command.HelpOption();
                command.OnExecute(() =>
                {
                    CommandBase.LaunchDebuggerIfSet(debuggerOption);
                    var result = CommandBase.GetDirectiveAndModel(directiveNameOption, _scriptBuilder);
                    if (!result.IsSuccessful)
                    {
                        app.Error.WriteLine($"Error: {result.ErrorMessage}");
                        return;
                    }

                    var directiveModel = result.Directive.GetModel();
                    var directiveModelType = directiveModel.GetType();
                    var parameters = parametersArgument.Values.Where(p => p.Contains(':')).Select(p => new TemplateParameter { Name = p.Split(':')[0], Value = string.Join(":", p.Split(':').Skip(1)) }).ToArray();
                    foreach (var parameter in parameters)
                    {
                        var property = directiveModelType.GetProperty(parameter.Name);
                        property?.SetValue(directiveModel, parameter.ConvertType(directiveModelType));
                    }

                    app.Out.WriteLine(_scriptBuilder.Build(result.Directive, directiveModel));
                });
            });
        }
    }
}
