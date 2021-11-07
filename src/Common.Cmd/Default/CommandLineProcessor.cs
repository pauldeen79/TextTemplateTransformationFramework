using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common.Cmd.Default
{
    public class CommandLineProcessor : ICommandLineProcessor
    {
        private readonly IEnumerable<ICommandLineCommand> _commands;

        public CommandLineProcessor(IEnumerable<ICommandLineCommand> commands)
        {
            _commands = commands ?? throw new ArgumentNullException(nameof(commands));
        }

        public void Initialize(CommandLineApplication app)
        {
            _commands.ForEach(command => command.Initialize(app));
        }
    }
}
