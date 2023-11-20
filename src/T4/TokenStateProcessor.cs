using System;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateSectionProcessors;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.Contracts;
using TextTemplateTransformationFramework.T4.Extensions;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.T4
{
    public class TokenStateProcessor: ITokenParserTokenStateProcessor<TokenParserState>
    {
        private readonly ICompositeTemplateSectionProcessor<TokenParserState> _sectionProcessor;
        private readonly ICodeSectionProcessor<TokenParserState> _codeSectionProcessor;
        private readonly ITextSectionProcessor<TokenParserState> _textSectionProcessor;
        private readonly ITokenSectionProcessor<TokenParserState> _tokenSectionProcessor;

        public TokenStateProcessor(ICompositeTemplateSectionProcessor<TokenParserState> sectionProcessor,
                                   ICodeSectionProcessor<TokenParserState> codeSectionProcessor,
                                   ITextSectionProcessor<TokenParserState> textSectionProcessor,
                                   ITokenSectionProcessor<TokenParserState> tokenSectionProcessor)
        {
            _sectionProcessor = sectionProcessor ?? throw new ArgumentNullException(nameof(sectionProcessor));
            _codeSectionProcessor = codeSectionProcessor ?? throw new ArgumentNullException(nameof(codeSectionProcessor));
            _textSectionProcessor = textSectionProcessor ?? throw new ArgumentNullException(nameof(textSectionProcessor));
            _tokenSectionProcessor = tokenSectionProcessor ?? throw new ArgumentNullException(nameof(tokenSectionProcessor));
        }

        public ProcessSectionResult<TokenParserState> Process(TokenParserState state, ITokenParserCallback<TokenParserState> tokenParserCallback, ILogger logger, TemplateParameter[] parameters)
        {
            if (state is null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            var context = new TokenParserContext(state, CreateSectionContext(state, tokenParserCallback, logger, parameters));
            return context.SectionContext switch
            {
                var sectionContext when sectionContext.IsText() => ProcessStateUsingProcessor(sectionContext, _textSectionProcessor),
                var sectionContext when sectionContext.IsValidForProcessors() => ProcessStateUsingProcessors(sectionContext, context.State),
                _ => ProcessSectionResult<TokenParserState>.Empty
            };
        }

        private SectionContext<TokenParserState> CreateSectionContext(TokenParserState state, ITokenParserCallback<TokenParserState> tokenParserCallback, ILogger logger, TemplateParameter[] parameters)
            => state.ToSectionContext
            (
                state.InitialFileName ?? state.FileName,
                tokenParserCallback,
                logger,
                parameters
            );

        private ProcessSectionResult<TokenParserState> ProcessStateUsingProcessor(SectionContext<TokenParserState> sectionContext, ITemplateSectionProcessor<TokenParserState> processor)
            => _tokenSectionProcessor.Process(sectionContext, processor.GetType(), processor.Process(sectionContext));

        private ProcessSectionResult<TokenParserState> ProcessStateUsingProcessors(SectionContext<TokenParserState> sectionContext, TokenParserState state)
            => new[] { _sectionProcessor }
                .Concat(state.CustomSectionProcessors)
                .Concat(state.Context.GetCustomSectionProcessors())
                .Concat(sectionContext.TokenParserCallback.GetCustomSectionProcessors())
                .Aggregate
                (
                    DirectiveProcessResult<TokenParserState>.Empty,
                    (directiveProcessResult, processor) =>
                        processor.Either
                        (
                            _ => directiveProcessResult.ShouldProcess(),
                            p => p.Process(sectionContext).Either
                            (
                                sectionProcessResult => sectionProcessResult.Understood,
                                sectionProcessResult => directiveProcessResult
                                    .With(_tokenSectionProcessor.Process(sectionContext, processor.GetType(), sectionProcessResult)),
                            _ => directiveProcessResult
                            ),
                            _ => directiveProcessResult
                        )
                )
                .Either
                (
                    directiveProcessResult => directiveProcessResult.Understood,
                    directiveProcessResult => directiveProcessResult.ProcessSectionResult,
                    directiveProcessResult => directiveProcessResult
                        .Either
                        (
#if NETFRAMEWORK
                            _ => sectionContext.Section.StartsWith("@"), _ => ProcessSectionResult.Unrecognized(sectionContext),
#else
                            _ => sectionContext.Section.StartsWith('@'), _ => ProcessSectionResult.Unrecognized(sectionContext),
#endif
                            _ => ProcessStateUsingProcessor(sectionContext, _codeSectionProcessor)
                        )
                );
    }
}
