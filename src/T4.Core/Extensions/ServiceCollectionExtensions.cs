using Microsoft.Extensions.DependencyInjection;
using ScriptCompiler.Extensions;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.T4.Core.ProcessFinalizers;
using TextTemplateTransformationFramework.T4.Core.ProcessInitializers;
using TextTemplateTransformationFramework.T4.Extensions;

namespace TextTemplateTransformationFramework.T4.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTextTemplateTransformationT4NetCore(this IServiceCollection instance)
            => instance
                .AddScriptCompiler()
                .AddTextTemplateTransformationT4()
                .AddSingleton<ICodeCompiler<TokenParserState>, CodeCompiler<TokenParserState>>()
                .AddSingleton<IProcessInitializer<ITextTemplateProcessorContext<TokenParserState>>, AssemblyLoadContextProcessInitializer<TokenParserState>>()
                .AddSingleton<IProcessFinalizer<ITextTemplateProcessorContext<TokenParserState>>, AssemblyLoadContextProcessFinalizer<TokenParserState>>();
    }
}
