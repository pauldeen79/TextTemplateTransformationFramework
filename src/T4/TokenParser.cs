using System;
using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.T4.Contracts;
using TextTemplateTransformationFramework.T4.Extensions;
using Utilities;
using Utilities.Extensions;
using StatePattern = Utilities.Pattern<TextTemplateTransformationFramework.T4.TokenParserState, TextTemplateTransformationFramework.T4.TokenParserState>;

namespace TextTemplateTransformationFramework.T4
{
    /// <summary>
    /// Implementation of ITokenParser that uses T4 template syntax.
    /// </summary>
    /// <seealso cref="ITextTemplateTokenParser" />
    public class TokenParser : ITextTemplateTokenParser<TokenParserState>
    {
        public const string SectionPrefix = "@ ";
        public const string SectionSuffix = " ";

        public TokenParser(IArgumentParser argumentParser,
                           ITokenArgumentParser<TokenParserState> tokenArgumentParser,
                           ITokenParserTokenStateProcessor<TokenParserState> tokenStateProcessor,
                           ITokenPlaceholderProcessor<TokenParserState> tokenPlaceholderProcessor,
                           ITokenParserTokenModifier tokenModifier)
        {
            _argumentParser = argumentParser ?? throw new ArgumentNullException(nameof(argumentParser));
            _tokenArgumentParser = tokenArgumentParser ?? throw new ArgumentNullException(nameof(argumentParser));
            _tokenStateProcessor = tokenStateProcessor ?? throw new ArgumentNullException(nameof(tokenStateProcessor));
            _tokenPlaceholderProcessor = tokenPlaceholderProcessor ?? throw new ArgumentNullException(nameof(tokenPlaceholderProcessor));
            _tokenModifier = tokenModifier ?? throw new ArgumentNullException(nameof(tokenModifier));
        }

        private readonly IArgumentParser _argumentParser;
        private readonly ITokenArgumentParser<TokenParserState> _tokenArgumentParser;
        private readonly ITokenParserTokenStateProcessor<TokenParserState> _tokenStateProcessor;
        private readonly ITokenPlaceholderProcessor<TokenParserState> _tokenPlaceholderProcessor;
        private readonly ITokenParserTokenModifier _tokenModifier;
        private readonly ProcessScope _processScope = new ProcessScope();

        public IEnumerable<ITemplateToken<TokenParserState>> Parse(ITextTemplateProcessorContext<TokenParserState> context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.AssemblyTemplate != null)
            {
                // Short-hand: Skip parsing because there is no text template
                return Enumerable.Empty<ITemplateToken<TokenParserState>>();
            }

            return Enumerable
                .Range(0, context.TextTemplate.Template?.Length ?? 0)
                .Aggregate
                (
                    TokenParserState.Initial(context), (s, position) =>
                        MatchStatePattern.Evaluate
                        (
                            s.SetPosition
                            (
                                context.TextTemplate.Template[position],
                                context.TextTemplate.Template.GetCharacterAt(position + 1)
                            )
                        )
                )
                .Apply(state => state.ProcessLastSection(ProcessState)) //process the last section
                .Tokens
                .AsEnumerable()
                .Apply(Modify);
        }

        private StatePattern MatchStatePattern
            => Pattern.Match
            (
                Clause.When<TokenParserState, TokenParserState>(state => state.ShouldSkip())
                      .Then(state => state.ProcessSkip()),
                Clause.When<TokenParserState, TokenParserState>(state => state.ShouldSwitchToNewLine())
                      .Then(state => state.NewLine()),
                Clause.When<TokenParserState, TokenParserState>(state => state.ShouldSwitchToStartSection())
                      .Then(state => state.StartSection(ProcessState)),
                Clause.When<TokenParserState, TokenParserState>(state => state.ShouldSwitchToEndSection())
                      .Then(state => state.EndSection(ProcessState)),
                Clause.When<TokenParserState, TokenParserState>(state => state.ShouldSwitchToExpressionSection())
                      .Then(state => state.ExpressionSection()),
                Clause.When<TokenParserState, TokenParserState>(state => state.ShouldSwitchToDirective())
                      .Then(state => state.Directive()),
                Clause.When<TokenParserState, TokenParserState>(state => state.ShouldSwitchToStartCodeBlock())
                      .Then(state => state.StartCodeBlock())
            )
            .Default
            (
                state => state.AddToCurrentSection(ProcessState)
            );

        private ProcessSectionResult<TokenParserState> ProcessState(TokenParserState state)
            => _tokenStateProcessor.Process(state, GetTokenParserCallback(), state.Context.Logger, state.Context.Parameters);

        private ITokenParserCallback<TokenParserState> GetTokenParserCallback()
            => new TokenParserCallback
            (
                _tokenPlaceholderProcessor,
                context => _processScope.Busy
                    ? Parse(context)
                    : _processScope.Process(() => Parse(context)),
                () => _processScope.Busy,
                _tokenArgumentParser,
                _argumentParser
            );

        /// <summary>
        /// Modifies the tokens.
        /// </summary>
        /// <param name="tokens">Source tokens.</param>
        /// <returns>
        /// Modified tokens.
        /// </returns>
        /// <remarks>
        /// By default, the source tokens are returned unmodified. You can override this method to modify them.
        /// </remarks>
        private IEnumerable<ITemplateToken<TokenParserState>> Modify(IEnumerable<ITemplateToken<TokenParserState>> tokens)
            => _tokenModifier.Modify(tokens);
    }
}
