using McMaster.Extensions.CommandLineUtils;

namespace TextTemplateTransformationFramework.Common.Cmd.Contracts
{
    public interface ICommandLineProcessor
    {
        void Initialize(CommandLineApplication app);
    }
}
