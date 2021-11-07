using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateSectionProcessors;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.Common.FileContentsProviders;
using TextTemplateTransformationFramework.Common.FileNameProviders;
using TextTemplateTransformationFramework.Common.SectionProcessors;
using TextTemplateTransformationFramework.Common.SectionProcessors.Sections;
using TextTemplateTransformationFramework.T4.Contracts;
using TextTemplateTransformationFramework.T4.LanguageServices;

namespace TextTemplateTransformationFramework.T4.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTextTemplateTransformationT4(this IServiceCollection instance, params Type[] templateSectionProcessorTypesToSkip)
            => instance
                .AddTextTemplateTransformation<TokenParserState>()
                .AddSingleton<IArgumentParser, ArgumentParser>()
                .AddSingleton<ICodeSectionProcessor<TokenParserState>, CodeSection<TokenParserState>>()
                .AddSingleton<IScriptBuilder<TokenParserState>>
                (
                    provider => new ScriptBuilder
                    (
                        provider.GetRequiredService<ICompositeTemplateSectionProcessor<TokenParserState>>(),
                        provider.GetRequiredService<IFileNameProvider>(),
                        provider.GetRequiredService<IFileContentsProvider>(),
                        provider.GetRequiredService<ITemplateCodeCompiler<TokenParserState>>()
                    )
                )
                .AddSingleton<ITextSectionProcessor<TokenParserState>, TextSection<TokenParserState>>()
                .AddSingleton<IFileContentsProvider, FileSystemFileContentsProvider>()
                .AddSingleton<IFileNameProvider, FileSystemFileNameProvider>()
                .AddTemplateSectionProcessors<TokenParserState>(typeof(ITemplateSectionProcessor<TokenParserState>).Assembly, templateSectionProcessorTypesToSkip)
                .AddSingleton<ICompositeTemplateSectionProcessor<TokenParserState>>
                (
                    provider => new CompositeTemplateSectionProcessor<TokenParserState>
                    (
                        provider.GetServices<ITemplateSectionProcessor<TokenParserState>>()
                                .Concat
                                (
                                    provider.GetService<IGroupedTokenMapperTypeProvider>()
                                            .GetGroups()
                                            .GroupBy(g => g.Key)
                                            .Select
                                            (
                                                g => new GroupedTokenMapperAdapter<TokenParserState>
                                                (
                                                    g.SelectMany(x => x),
                                                    provider.GetRequiredService<IFileNameProvider>(),
                                                    provider.GetRequiredService<IFileContentsProvider>(),
                                                    provider.GetRequiredService<ITemplateCodeCompiler<TokenParserState>>()
                                                )
                                            )
                                            .Cast<ITemplateSectionProcessor<TokenParserState>>()
                                )
                                .Concat
                                (
                                    provider.GetService<ITokenMapperTypeProvider>()
                                            .GetTypes()
                                            .Select
                                            (
                                                t => new TokenMapperAdapter<TokenParserState>
                                                    (
                                                        t,
                                                        provider.GetRequiredService<IFileNameProvider>(),
                                                        provider.GetRequiredService<IFileContentsProvider>(),
                                                        provider.GetRequiredService<ITemplateCodeCompiler<TokenParserState>>()
                                                    )
                                            )
                                            .Cast<ITemplateSectionProcessor<TokenParserState>>()
                                )
                    )
                )
                .AddSingleton<ITextTemplateTokenParser<TokenParserState>, TokenParser>()
                .AddSingleton<ITokenProcessor<TokenParserState>, TokenProcessor<TokenParserState>>()
                .AddSingleton<ITokenArgumentParser<TokenParserState>, TokenArgumentParser<TokenParserState>>()
                .AddSingleton<ITokenPlaceholderProcessor<TokenParserState>, TokenPlaceholderProcessor<TokenParserState>>()
                .AddSingleton<ITokenSectionProcessor<TokenParserState>, TokenSectionProcessor>()
                .AddSingleton<ITokenParserTokenStateProcessor<TokenParserState>, TokenStateProcessor>()
                .AddSingleton<ITokenParserTokenModifier, TokenModifier>()
                .AddSingleton<ITokenProcessorCodeDomLanguageConverter, TokenProcessorCodeDomLanguageConverter>()
                .AddSingleton<ITokenProcessorTokenConverter<TokenParserState>, TokenProcessorTokenConverter<TokenParserState>>()
                .AddSingleton<ITokenProcessorCodeGenerator<TokenParserState>, TokenProcessorCodeGenerator<TokenParserState>>()
                .AddSingleton<ITokenProcessorReferenceAssemblyNamesProvider<TokenParserState>, TokenProcessorReferenceAssemblyNamesProvider<TokenParserState>>()
                .AddSingleton<ITokenProcessorPackageReferenceNamesProvider<TokenParserState>, TokenProcessorPackageReferenceNamesProvider<TokenParserState>>()
                .AddSingleton<ITokenProcessorDefaultNamespaceImportTokenProvider<TokenParserState>, TokenProcessorDefaultNamespaceImportTokenProvider<TokenParserState>>();
    }
}
