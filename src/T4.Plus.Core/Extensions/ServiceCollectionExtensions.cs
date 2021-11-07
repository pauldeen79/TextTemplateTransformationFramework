using Microsoft.Extensions.DependencyInjection;
using ScriptCompiler.Extensions;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.T4.Core;
using TextTemplateTransformationFramework.T4.Core.ProcessFinalizers;
using TextTemplateTransformationFramework.T4.Plus.Core.ProcessInitializers;
using TextTemplateTransformationFramework.T4.Plus.Extensions;

namespace TextTemplateTransformationFramework.T4.Plus.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTextTemplateTransformationT4PlusNetCore(this IServiceCollection instance)
            => instance
                .AddScriptCompiler()
                .AddTextTemplateTransformationT4Plus()
                .AddSingleton<ICodeCompiler<TokenParserState>, CodeCompiler<TokenParserState>>()
                .AddSingleton<IProcessInitializer<ITextTemplateProcessorContext<TokenParserState>>, PlusCoreProcessInitializer<TokenParserState>>()
                .AddSingleton<IProcessFinalizer<ITextTemplateProcessorContext<TokenParserState>>, AssemblyLoadContextProcessFinalizer<TokenParserState>>();
    }
}
