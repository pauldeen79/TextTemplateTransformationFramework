using System;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using TextTemplateTransformationFramework.Common.Cmd.Extensions;
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
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.Command("generate-directive", command =>
            {
                command.Description = "Generates a directive with specified arguments";

                var directiveNameOption = command.Option<string>("-n|--name <NAME>", "The directive name to generate", CommandOptionType.SingleValue);
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
                    var directiveModelType = directiveModel.GetType();
                    var parameters = parametersArgument.Values.Where(p => p.Contains(":")).Select(p => new TemplateParameter { Name = p.Split(':')[0], Value = string.Join(":", p.Split(':').Skip(1)) }).ToArray();
                    foreach (var parameter in parameters)
                    {
                        var property = directiveModelType.GetProperty(parameter.Name);
                        property?.SetValue(directiveModel, parameter.ConvertType(directiveModelType));
                    }

                    app.Out.WriteLine(_scriptBuilder.Build(directive, directiveModel));
                });
            });
        }
    }
}
