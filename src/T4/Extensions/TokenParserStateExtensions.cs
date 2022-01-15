using System;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;

namespace TextTemplateTransformationFramework.T4.Extensions
{
    public static class TokenParserStateExtensions
    {
        public static int PreviousOrCurrentMode(this TokenParserState state)
            => state.CurrentMode == Mode.Directive
                && !state.PreviousMode.IsTextRange()
                && state.PreviousMode != Mode.Directive
                    ? state.PreviousMode
                    : state.CurrentMode;

        public static string GetSection(this TokenParserState state)
            => state.CurrentSectionBuilder.ToString();

        public static bool ShouldSkip(this TokenParserState state)
            => state.Skip > 0;

        public static bool ShouldSwitchToNewLine(this TokenParserState state)
            => (state.CurrentPosition == '\r' && state.NextPosition == '\n') || (Environment.NewLine.Length == 1 && state.CurrentPosition == Environment.NewLine[0]);

        public static bool ShouldSwitchToStartSection(this TokenParserState state)
            => state.CurrentPosition == '<' && state.PreviousPosition != '\\' && state.NextPosition == '#';

        public static bool ShouldSwitchToEndSection(this TokenParserState state)
            => state.CurrentPosition == '#' && state.PreviousPosition != '\\' && state.NextPosition == '>';

        public static bool ShouldSwitchToExpressionSection(this TokenParserState state)
            => state.CurrentPosition == '=' && state.CurrentMode == Mode.StartBlock;

        public static bool ShouldSwitchToDirective(this TokenParserState state)
            => state.CurrentPosition == '@' && state.CurrentMode == Mode.StartBlock;

        public static bool ShouldSwitchToStartCodeBlock(this TokenParserState state)
            => state.CurrentMode == Mode.StartBlock;

        public static SectionContext<TokenParserState> ToSectionContext
        (
            this TokenParserState state,
            string fileName,
            ITokenParserCallback<TokenParserState> tokenParserCallback,
            ILogger logger,
            TemplateParameter[] parameters
        ) => SectionContext.FromSection
        (
            new Section(fileName, state.PreviousLine ?? state.LineCounter, state.GetSection()),
            state.PreviousOrCurrentMode(),
            state.ExistingTokens.Concat(state.Tokens),
            tokenParserCallback,
            state,
            logger,
            parameters
        );
    }
}
