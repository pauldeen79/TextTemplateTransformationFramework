using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus
{
    public class TokenSectionProcessor : ITokenSectionProcessor<TokenParserState>
    {
        private readonly ITokenSectionProcessor<TokenParserState> _baseProcessor;
        private readonly ITokenPlaceholderProcessor<TokenParserState> _tokenPlaceholderProcessor;

        public TokenSectionProcessor(ITokenSectionProcessor<TokenParserState> baseProcessor,
                                     ITokenPlaceholderProcessor<TokenParserState> tokenPlaceholderProcessor)
        {
            _baseProcessor = baseProcessor ?? throw new ArgumentNullException(nameof(baseProcessor));
            _tokenPlaceholderProcessor = tokenPlaceholderProcessor ?? throw new ArgumentNullException(nameof(tokenPlaceholderProcessor));
        }

        public ProcessSectionResult<TokenParserState> Process(SectionContext<TokenParserState> context,
                                                              Type sectionProcessorType,
                                                              SectionProcessResult<TokenParserState> sectionProcessResult)
        {
            if (sectionProcessResult == null)
            {
                throw new ArgumentNullException(nameof(sectionProcessResult));
            }

            var additionalTokens = new List<ITemplateToken<TokenParserState>>();
            if (sectionProcessResult.Understood)
            {
                //load 'pre-load' assembly references, so they can be used in the parsing process.
                additionalTokens.AddRange
                (
                    sectionProcessResult
                        .Tokens
                        .OfType<IPreLoadToken<TokenParserState>>()
                        .Select
                        (
                            preloadToken => new
                            {
                                PreloadToken = preloadToken,
                                AssemblyName = _tokenPlaceholderProcessor.Process(preloadToken.Name, context.ExistingTokens, context.State)
                            }
                        )
                        .Where(a => !string.IsNullOrEmpty(a.AssemblyName))
#pragma warning disable S3885 // "Assembly.Load" should be used
                        .Select(a => new { a.PreloadToken, a.AssemblyName, Assembly = Assembly.LoadFrom(a.AssemblyName.GetAssemblyName()) })
#pragma warning restore S3885 // "Assembly.Load" should be used
                        .Where(a => a.Assembly != null)
                        .Select(a => new AssemblyToken<TokenParserState>(context, a.PreloadToken.Name, a.Assembly))
                );
            }

            var baseResult = _baseProcessor.Process
            (
                context,
                sectionProcessorType,
                sectionProcessResult
            );

            return additionalTokens.Count > 0
                ? baseResult
                    .With
                    (
                        new[]
                        {
                            new SourceSectionToken<TokenParserState>
                            (
                                context,
                                sectionProcessorType,
                                sectionProcessResult.TokensAreForRootTemplateSection,
                                additionalTokens
                            )
                        }
                    )
                : baseResult;
        }
    }
}
