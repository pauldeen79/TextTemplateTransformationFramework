using Microsoft.Extensions.DependencyInjection;
using TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using TextTemplateTransformationFramework.Common.Cmd.Default;

namespace TextTemplateTransformationFramework.Common.Cmd.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTextTemplateTransformationCommands<TState>(this IServiceCollection instance)
            where TState : class
            => instance
                .AddSingleton<ICommandLineProcessor, CommandLineProcessor>()
                .AddSingleton<ICommandLineCommand, VersionCommand>()
                .AddSingleton<ICommandLineCommand, GenerateDirectiveCommand<TState>>()
                .AddSingleton<ICommandLineCommand, ListDirectivesCommand<TState>>()
                .AddSingleton<ICommandLineCommand, ListDirectiveCommand<TState>>()
                .AddSingleton<ICommandLineCommand, ListParametersCommand>()
                .AddSingleton<ICommandLineCommand, RunTemplateCommand>()
                .AddSingleton<ICommandLineCommand, SourceCodeCommand>()
                .AddSingleton<IUserInput, UserInput>()
                .AddSingleton<IClipboardService, ClipboardService>();
    }
}
