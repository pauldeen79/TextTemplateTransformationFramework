using System;
using McMaster.Extensions.CommandLineUtils;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands
{
    public class AddTemplateCommand : ICommandLineCommand
    {
        private readonly IFileContentsProvider _fileContentsProvider;
        private readonly ITemplateInfoRepository _templateInfoRepository;

        public AddTemplateCommand(IFileContentsProvider fileContentsProvider, ITemplateInfoRepository templateInfoRepository)
        {
            _fileContentsProvider = fileContentsProvider ?? throw new ArgumentNullException(nameof(fileContentsProvider));
            _templateInfoRepository = templateInfoRepository ?? throw new ArgumentNullException(nameof(templateInfoRepository));
        }

        public void Initialize(CommandLineApplication app)
            => CommandBase.ModifyGlobalTemplate
            (
                app,
                _fileContentsProvider,
                _templateInfoRepository.Add,
                "add-template",
                "Adds the specified template to the templates list",
                "Template has been added successfully.",
                true
            );
    }
}
