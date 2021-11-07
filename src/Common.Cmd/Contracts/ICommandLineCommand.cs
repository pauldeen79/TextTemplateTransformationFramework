using McMaster.Extensions.CommandLineUtils;

namespace TextTemplateTransformationFramework.Common.Cmd.Contracts
{
    public interface ICommandLineCommand
    {
        void Initialize(CommandLineApplication app);
    }
}
