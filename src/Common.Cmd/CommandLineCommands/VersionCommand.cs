using System;
using McMaster.Extensions.CommandLineUtils;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;

namespace TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands
{
    public class VersionCommand : ICommandLineCommand
    {
        public void Initialize(CommandLineApplication app)
        {
            app.VersionOptionFromAssemblyAttributes(GetType().Assembly);
        }
    }
}
