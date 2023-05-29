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
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            app.Command("add-template", command =>
            {
                command.Description = "Adds template to the templates list";
                var debuggerOption = CommandBase.GetDebuggerOption(command);
                var shortNameOption = command.Option<string>("-s|--shortname <NAME>", "The unique name of the template", CommandOptionType.SingleValue);
                var fileNameOption = command.Option<string>("-f|--filename <PATH>", "The template filename", CommandOptionType.SingleValue);
                var assemblyNameOption = command.Option<string>("-a|--assembly <ASSEMBLY>", "The template assembly", CommandOptionType.SingleValue);
                var classNameOption = command.Option<string>("-n|--classname <CLASS>", "The template class name", CommandOptionType.SingleValue);
                command.HelpOption();
                command.OnExecute(() =>
                {
                    CommandBase.LaunchDebuggerIfSet(debuggerOption);
                    var shortName = shortNameOption.Value();
                    var fileName = fileNameOption.Value();
                    var assemblyName = assemblyNameOption.Value();
                    var className = classNameOption.Value();

                    var validationResult = CommandBase.GetValidationResult(_fileContentsProvider, fileName, assemblyName, className);
                    if (!string.IsNullOrEmpty(validationResult))
                    {
                        app.Out.WriteLine($"Error: {validationResult}");
                        return;
                    }

                    if (string.IsNullOrEmpty(shortName))
                    {
                        app.Out.WriteLine("Error: Shortname is required.");
                        return;
                    }

                    var type = string.IsNullOrEmpty(assemblyName)
                        ? TemplateType.TextTemplate
                        : TemplateType.AssemblyTemplate;

                    _templateInfoRepository.Add(new TemplateInfo(shortName, fileName ?? string.Empty, assemblyName ?? string.Empty, className ?? string.Empty, type));
                    app.Out.WriteLine("Template has been added successfully.");
                });
            });
        }
    }
}
