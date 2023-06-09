using System;
using McMaster.Extensions.CommandLineUtils;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using TextTemplateTransformationFramework.Common.Contracts;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands
{
    public class ListTemplatesCommand : ICommandLineCommand
    {
        private readonly ITemplateInfoRepository _templateInfoRepository;

        public ListTemplatesCommand(ITemplateInfoRepository templateInfoRepository)
        {
            _templateInfoRepository = templateInfoRepository ?? throw new ArgumentNullException(nameof(templateInfoRepository));
        }

        public void Initialize(CommandLineApplication app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            app.Command("list-templates", command =>
            {
                command.Description = "Lists templates";
                var debuggerOption = CommandBase.GetDebuggerOption(command);
                command.HelpOption();
                command.OnExecute(() =>
                {
                    CommandBase.LaunchDebuggerIfSet(debuggerOption);
                    _templateInfoRepository.GetTemplates().ForEach(app.Out.WriteLine);
                });
            });
        }
    }
}
