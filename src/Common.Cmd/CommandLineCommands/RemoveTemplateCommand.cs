using System;
using McMaster.Extensions.CommandLineUtils;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands
{
    public class RemoveTemplateCommand : ICommandLineCommand
    {
        private readonly IFileContentsProvider _fileContentsProvider;
        private readonly ITemplateInfoRepository _templateInfoRepository;

        public RemoveTemplateCommand(IFileContentsProvider fileContentsProvider, ITemplateInfoRepository templateInfoRepository)
        {
            _fileContentsProvider = fileContentsProvider ?? throw new ArgumentNullException(nameof(fileContentsProvider));
            _templateInfoRepository = templateInfoRepository ?? throw new ArgumentNullException(nameof(templateInfoRepository));
        }

        public void Initialize(CommandLineApplication app)
            => CommandBase.ModifyGlobalTemplate
            (
                app,
                _fileContentsProvider,
                _templateInfoRepository.Remove,
                "remove-template",
                "Removes the specified template from the templates list",
                "Template has been removed successfully.",
                false
            );
    }
}
