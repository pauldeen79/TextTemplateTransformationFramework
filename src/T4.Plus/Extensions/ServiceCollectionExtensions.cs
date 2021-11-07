using Microsoft.Extensions.DependencyInjection;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.Common.Mappers;
using TextTemplateTransformationFramework.T4.Contracts;
using TextTemplateTransformationFramework.T4.Extensions;
using TextTemplateTransformationFramework.T4.Plus.ProcessFinalizers;
using TextTemplateTransformationFramework.T4.Plus.ProcessInitializers;

namespace TextTemplateTransformationFramework.T4.Plus.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTextTemplateTransformationT4Plus(this IServiceCollection instance)
            => instance
                .AddTextTemplateTransformationT4(typeof(AssemblyTokenMapper<TokenParserState>))
                .AddSingleton<TextTemplateTransformationFramework.Common.Default.TemplateCodeCompiler<TokenParserState>>()
                .AddSingleton<ITemplateCodeCompiler<TokenParserState>>(provider => new TemplateCodeCompiler<TokenParserState>(provider.GetRequiredService<TextTemplateTransformationFramework.Common.Default.TemplateCodeCompiler<TokenParserState>>()))
                .AddSingleton<TextTemplateTransformationFramework.Common.Default.TemplateFactory<TokenParserState>>()
                .AddSingleton<ITemplateFactory<TokenParserState>>(provider => new TemplateFactory<TokenParserState>(provider.GetRequiredService<TextTemplateTransformationFramework.Common.Default.TemplateFactory<TokenParserState>>()))
                .AddSingleton<ITemplateInitializeParameterSetter<TokenParserState>, TemplateInitializeParameterSetter<TokenParserState>>()
                .AddSingleton<IProcessInitializer<ITemplateProcessorContext<TokenParserState>>, TemplateProcessorInitializer<TokenParserState>>()
                .AddSingleton<IProcessFinalizer<ITemplateProcessorContext<TokenParserState>>, TemplateProcessorFinalizer<TokenParserState>>()
                .AddTemplateSectionProcessors<TokenParserState>(typeof(TemplateFactory<TokenParserState>).Assembly)
                .AddSingleton<ITemplateRenderParameterSetter<TokenParserState>, TemplateRenderParameterSetter<TokenParserState>>()
                .AddSingleton<ITemplateValidator, TemplateValidator>()
                .AddSingleton<ITextTemplateProcessorPropertyOwnerProvider<TokenParserState>, TextTemplateProcessorPropertyOwnerProvider<TokenParserState>>()
                .AddSingleton<ITextTemplateProcessorPropertyProvider<TokenParserState>, TextTemplateProcessorPropertyProvider<TokenParserState>>()
                .AddSingleton<IProcessInitializer<ITextTemplateProcessorContext<TokenParserState>>, TraceTextTemplateProcessorInitializer<TokenParserState>>()
                .AddSingleton<T4.TokenParser>()
                .AddSingleton<ITextTemplateTokenParser<TokenParserState>>
                (
                    provider => new TokenParser(provider.GetRequiredService<T4.TokenParser>())
                )
                .AddSingleton<ITokenArgumentParser<TokenParserState>, TokenArgumentParser<TokenParserState>>()
                .AddSingleton<ITokenParserTokenModifier, TokenModifier>()
                .AddSingleton<T4.TokenStateProcessor>()
                .AddSingleton<ITokenParserTokenStateProcessor<TokenParserState>>
                (
                    provider => new TokenStateProcessor(provider.GetRequiredService<T4.TokenStateProcessor>())
                )
                .AddSingleton<ITokenPlaceholderProcessor<TokenParserState>, TokenPlaceholderProcessor<TokenParserState>>()
                .AddSingleton<T4.TokenProcessor<TokenParserState>>()
                .AddSingleton<ITokenProcessor<TokenParserState>>
                (
                    provider => new TokenProcessor<TokenParserState>(provider.GetRequiredService<T4.TokenProcessor<TokenParserState>>())
                )
                .AddSingleton<T4.TokenSectionProcessor>()
                .AddSingleton<ITokenSectionProcessor<TokenParserState>>
                (
                    provider => new TokenSectionProcessor
                    (
                        provider.GetRequiredService<T4.TokenSectionProcessor>(),
                        provider.GetRequiredService<ITokenPlaceholderProcessor<TokenParserState>>()
                    )
                )
                .AddSingleton<ITokenProcessorTokenConverter<TokenParserState>, TokenProcessorTokenConverter<TokenParserState>>()
                .AddSingleton<ITokenProcessorCodeGenerator<TokenParserState>, TokenProcessorCodeGenerator<TokenParserState>>();
    }
}
