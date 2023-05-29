using Microsoft.Extensions.DependencyInjection;
using TextCopy;
using TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using TextTemplateTransformationFramework.Common.Cmd.Default;

namespace TextTemplateTransformationFramework.Common.Cmd.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTextTemplateTransformationCommands<TState>(this IServiceCollection instance)
            where TState : class
        {
            instance.InjectClipboard();
            return instance
                .AddSingleton<ICommandLineProcessor, CommandLineProcessor>()
                .AddSingleton<ICommandLineCommand, VersionCommand>()
                .AddSingleton<ICommandLineCommand, GenerateDirectiveCommand<TState>>()
                .AddSingleton<ICommandLineCommand, ListDirectivesCommand<TState>>()
                .AddSingleton<ICommandLineCommand, ListDirectiveCommand<TState>>()
                .AddSingleton<ICommandLineCommand, ListParametersCommand>()
                .AddSingleton<ICommandLineCommand, RunTemplateCommand>()
                .AddSingleton<ICommandLineCommand, SourceCodeCommand>()
                .AddSingleton<ICommandLineCommand, RunCodeGenerationProviderAssemblyCommand>()
                .AddSingleton<ICommandLineCommand, ListTemplatesCommand>()
                .AddSingleton<ICommandLineCommand, AddTemplateCommand>()
                .AddSingleton<ICommandLineCommand, RemoveTemplateCommand>()
                .AddSingleton<IUserInput, UserInput>();
        }
    }
}
