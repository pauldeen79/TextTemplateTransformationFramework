﻿using System;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateSectionProcessors;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.Contracts;
using TextTemplateTransformationFramework.T4.Extensions;
using Utilities;
using Utilities.Extensions;
using ContextPattern = Utilities.Pattern<TextTemplateTransformationFramework.T4.TokenParserContext, TextTemplateTransformationFramework.T4.ProcessSectionResult<TextTemplateTransformationFramework.T4.TokenParserState>>;

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

        public ProcessSectionResult<TokenParserState> Process(TokenParserState state, ITokenParserCallback<TokenParserState> tokenParserCallback, ILogger logger)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            return MatchContextPattern.Evaluate(new TokenParserContext(state, CreateSectionContext(state, tokenParserCallback, logger)));
        }

        private ContextPattern MatchContextPattern
            => Pattern.Match
            (
                Clause.When<TokenParserContext, ProcessSectionResult<TokenParserState>>(context => context.SectionContext.IsText())
                      .Then(context => ProcessStateUsingProcessor(context.SectionContext, _textSectionProcessor)),
                Clause.When<TokenParserContext, ProcessSectionResult<TokenParserState>>(context => context.SectionContext.IsValidForProcessors())
                      .Then(context => ProcessStateUsingProcessors(context.SectionContext, context.State))
            )
            .Default
            (
                _ => ProcessSectionResult<TokenParserState>.Empty
            );

        private SectionContext<TokenParserState> CreateSectionContext(TokenParserState state, ITokenParserCallback<TokenParserState> tokenParserCallback, ILogger logger)
            => state.ToSectionContext
            (
                state.InitialFileName ?? state.FileName,
                tokenParserCallback,
                logger
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
                            _ => sectionContext.Section.StartsWith("@"), _ => ProcessSectionResult.Unrecognized(sectionContext),
                            _ => ProcessStateUsingProcessor(sectionContext, _codeSectionProcessor)
                        )
                );
    }
}
