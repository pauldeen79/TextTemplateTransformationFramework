using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Decorators;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.ProcessFinalizers;
using TextTemplateTransformationFramework.Common.ProcessInitializers;
using TextTemplateTransformationFramework.Common.RequestProcessors;
using TextTemplateTransformationFramework.Common.Requests;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTextTemplateTransformation<TState>(this IServiceCollection instance) where TState : class
            => instance
                .AddSingleton<ITemplateCompilerOutputValidator<TState>, TemplateCompilerOutputValidator<TState>>()
                .AddSingleton<ITemplateInitializer<TState>, TemplateInitializer<TState>>()
                .AddSingleton<IAssemblyService, AssemblyService>()
                .AddSingleton<ILoggerFactory, LoggerFactory>()
                .AddSingleton<ICodeCompiler<TState>, CodeCompiler<TState>>()
                .AddSingleton<ITextTemplateProcessor, TextTemplateProcessor<TState>>()
                .AddSingleton<ITemplateCodeCompiler<TState>, TemplateCodeCompiler<TState>>()
                .AddSingleton<ITemplateFactory<TState>, TemplateFactory<TState>>()
                .AddSingleton<ITemplateInitializeMethodParameterProvider, TemplateInitializeMethodParameterProvider>()
                .AddSingleton<ITemplateInitializeParameterSetter<TState>, TemplateInitializeParameterSetter<TState>>()
                .AddSingleton<ITemplateProcessor<TState>, TemplateProcessor<TState>>()
                .AddSingleton<ProcessTemplateRequestProcessor<TState>>()
                .AddSingleton<IRequestProcessor<ProcessTemplateRequest<TState>, ProcessResult>>
                (
                    provider => new ProcessTemplateRequestProcessorDecorator<TState>
                    (
                        provider.GetRequiredService<ProcessTemplateRequestProcessor<TState>>(),
                        provider.GetRequiredService<IProcessInitializer<ITemplateProcessorContext<TState>>>(),
                        provider.GetRequiredService<IProcessFinalizer<ITemplateProcessorContext<TState>>>()
                    )
                )
                .AddSingleton<IProcessInitializer<ITemplateProcessorContext<TState>>, EmptyProcessInitializer<ITemplateProcessorContext<TState>>>()
                .AddSingleton<IProcessFinalizer<ITemplateProcessorContext<TState>>, EmptyProcessFinalizer<ITemplateProcessorContext<TState>>>()
                .AddSingleton<ITemplateRenderer<TState>, TemplateRenderer<TState>>()
                .AddSingleton<ITemplateRenderParameterSetter<TState>, TemplateRenderParameterSetter<TState>>()
                .AddSingleton<ITemplateValidator, TemplateValidator>()
                .AddSingleton<ITextTemplateParameterExtractor<TState>, TextTemplateParameterExtractor<TState>>()
                .AddSingleton<ITextTemplateProcessorPropertyOwnerProvider<TState>, TextTemplateProcessorPropertyOwnerProvider<TState>>()
                .AddSingleton<ITextTemplateProcessorPropertyProvider<TState>, TextTemplateProcessorPropertyProvider<TState>>()
                .AddSingleton<ITemplateOutputCreator<TState>, TemplateOutputCreator<TState>>()
                .AddSingleton<IRequestProcessor<ExtractParametersFromTextTemplateRequest<TState>, ExtractParametersResult>, ExtractParametersFromTextTemplateRequestProcessor<TState>>()
                .AddSingleton<IRequestProcessor<ExtractParametersFromAssemblyTemplateRequest<TState>, ExtractParametersResult>, ExtractParametersFromAssemblyTemplateRequestProcessor<TState>>()
                .AddSingleton<IRequestProcessor<PreProcessTextTemplateRequest<TState>, ProcessResult>, PreProcessTextTemplateRequestProcessor<TState>>()
                .AddSingleton<IRequestProcessor<ProcessTextTemplateRequest<TState>, ProcessResult>, ProcessTextTemplateRequestProcessor<TState>>()
                .AddSingleton<IRequestProcessor<ProcessAssemblyTemplateRequest<TState>, ProcessResult>, ProcessAssemblyTemplateRequestProcessor<TState>>()
                .AddSingleton<ExtractParametersFromTextTemplateRequestProcessor<TState>>()
                .AddSingleton<IRequestProcessor<ExtractParametersFromTextTemplateRequest<TState>, ExtractParametersResult>>
                (
                    provider => new ExtractParametersFromTextTemplateRequestProcessorDecorator<TState>
                    (
                        provider.GetRequiredService<ExtractParametersFromTextTemplateRequestProcessor<TState>>(),
                        provider.GetRequiredService<IProcessInitializer<ITextTemplateProcessorContext<TState>>>(),
                        provider.GetRequiredService<IProcessFinalizer<ITextTemplateProcessorContext<TState>>>()
                    )
                )
                .AddSingleton<PreProcessTextTemplateRequestProcessor<TState>>()
                .AddSingleton<IRequestProcessor<PreProcessTextTemplateRequest<TState>, ProcessResult>>
                (
                    provider => new PreProcessTextTemplateRequestProcessorDecorator<TState>
                    (
                        provider.GetRequiredService<PreProcessTextTemplateRequestProcessor<TState>>(),
                        provider.GetRequiredService<IProcessInitializer<ITextTemplateProcessorContext<TState>>>(),
                        provider.GetRequiredService<IProcessFinalizer<ITextTemplateProcessorContext<TState>>>()
                    )
                )
                .AddSingleton<ProcessTextTemplateRequestProcessor<TState>>()
                .AddSingleton<IRequestProcessor<ProcessTextTemplateRequest<TState>, ProcessResult>>
                (
                    provider => new ProcessTextTemplateRequestProcessorDecorator<TState>
                    (
                        provider.GetRequiredService<ProcessTextTemplateRequestProcessor<TState>>(),
                        provider.GetRequiredService<IProcessInitializer<ITextTemplateProcessorContext<TState>>>(),
                        provider.GetRequiredService<IProcessFinalizer<ITextTemplateProcessorContext<TState>>>()
                    )
                )
                .AddSingleton<IProcessInitializer<ITextTemplateProcessorContext<TState>>, EmptyProcessInitializer<ITextTemplateProcessorContext<TState>>>()
                .AddSingleton<IProcessFinalizer<ITextTemplateProcessorContext<TState>>, EmptyProcessFinalizer<ITextTemplateProcessorContext<TState>>>()
                .AddSingleton<ITemplateProxy, TemplateProxy>()
                .AddSingleton<ITokenMapperTypeProvider, TokenMapperTypeProvider>()
                .AddSingleton<IGroupedTokenMapperTypeProvider, GroupedTokenMapperTypeProvider>();

        public static IServiceCollection AddTemplateSectionProcessors<TState>(this IServiceCollection instance, Assembly assembly, params Type[] templateSectionProcessorTypesToSkip)
            where TState : class
        {
            foreach (var type in assembly.GetTemplateSectionProcessorTypes<TState>().Where(t => !templateSectionProcessorTypesToSkip.Any(x => x.FullName.WithoutGenerics() == t.FullName.WithoutGenerics())))
            {
                instance.AddSingleton(typeof(ITemplateSectionProcessor<TState>), type);
            }

            foreach (var type in assembly.GetTokenMapperTypes().Where(t => !templateSectionProcessorTypesToSkip.Any(x => x.FullName.WithoutGenerics() == t.FullName.WithoutGenerics())))
            {
                instance.AddSingleton(typeof(ITokenMapperTypeContainer), new TokenMapperTypeContainer(type));
            }

            foreach (var g in assembly.GetGroupedTokenMapperTypes())
            {
                instance.AddSingleton(typeof(IGroupedTokenMapperTypeContainer), new GroupedTokenMapperTypeContainer(g));
            }

            return instance;
        }
    }
}
