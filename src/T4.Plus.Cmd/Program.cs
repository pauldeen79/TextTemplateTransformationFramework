using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using TextTemplateTransformationFramework.Common.Cmd.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Core.Extensions;

namespace TextTemplateTransformationFramework.T4.Plus.Cmd
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            var app = new CommandLineApplication
            {
                Name = "t3f",
                Description = "TextTemplateTransformationFramework",
                UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.CollectAndContinue
            };
            app.HelpOption();

            var serviceCollection = new ServiceCollection()
                .AddTextTemplateTransformationT4PlusNetCore()
                .AddTextTemplateTransformationCommands<TokenParserState>();
            using var provider = serviceCollection.BuildServiceProvider();
            var processor = provider.GetRequiredService<ICommandLineProcessor>();
            processor.Initialize(app);
            return app.Execute(args);
        }
    }
}
