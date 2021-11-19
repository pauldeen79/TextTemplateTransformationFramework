using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using McMaster.Extensions.CommandLineUtils;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;

namespace TextTemplateTransformationFramework.Common.Cmd.Tests.TestFixtures
{
    [ExcludeFromCodeCoverage]
    internal static class CommandLineCommandHelper
    {
        internal static string ExecuteCommand<T>(Func<T> sutCreateDelegate, params string[] arguments)
            where T : ICommandLineCommand
        {
            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream);
            var app = new CommandLineApplication();
            app.Out = writer;
            app.Error = writer;
            var sut = sutCreateDelegate.Invoke();
            sut.Initialize(app);

            // Act
            app.Commands.First().Execute(arguments);

            writer.Flush();
            return Encoding.UTF8.GetString(stream.ToArray());
        }
    }
}
