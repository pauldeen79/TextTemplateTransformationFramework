﻿namespace TemplateFramework.Core.CodeGeneration.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateFramework(this IServiceCollection services)
        => services
            .AddSingleton<ICodeGenerationAssembly, CodeGenerationAssembly>()
            .AddSingleton<ICodeGenerationEngine, CodeGenerationEngine>()
            ;
}
