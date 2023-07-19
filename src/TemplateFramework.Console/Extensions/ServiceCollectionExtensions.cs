﻿namespace TemplateFramework.Console.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateCommands(this IServiceCollection instance)
        => instance
            .AddSingleton<IUserInput, UserInput>()
            .AddSingleton<ICommandLineProcessor, CommandLineProcessor>()
            .AddSingleton<ICommandLineCommand, VersionCommand>();
}
