using Microsoft.Extensions.DependencyInjection;
using ScriptCompiler.NetFramework.Extensions;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.T4.Extensions;

namespace TextTemplateTransformationFramework.T4.NetFramework.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTextTemplateTransformationT4NetFramework(this IServiceCollection instance)
            => instance
                .AddScriptCompiler()
                .AddTextTemplateTransformationT4()
                .AddSingleton<ITemplateCodeCompiler<TokenParserState>, TemplateCodeCompiler<TokenParserState>>();
    }
}
