﻿using TemplateFramework.Core.CodeGeneration;

namespace TemplateFramework.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTemplateFramework(this IServiceCollection services)
        => services
            .AddSingleton<ITemplateFactory, TemplateFactory>()
            .AddSingleton<ICodeGenerationAssembly, CodeGenerationAssembly>()
            .AddSingleton<ICodeGenerationEngine, CodeGenerationEngine>()
            .AddSingleton<IFileSystem, FileSystem>()
            .AddSingleton<ITemplateEngine, TemplateEngine>()
            .AddSingleton<ITemplateFileManagerFactory, TemplateFileManagerFactory>()
            .AddSingleton<ITemplateInitializer, DefaultTemplateInitializer>()
            .AddSingleton<ITemplateRenderer, SingleContentTemplateRenderer>()
            .AddSingleton<ITemplateRenderer, MultipleContentTemplateRenderer>()
            ;
}
