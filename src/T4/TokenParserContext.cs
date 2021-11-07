using System;
using TextTemplateTransformationFramework.Common;

namespace TextTemplateTransformationFramework.T4
{
    public sealed class TokenParserContext
    {
        public TokenParserState State { get; }
        public SectionContext<TokenParserState> SectionContext { get; }

        public TokenParserContext(TokenParserState state, SectionContext<TokenParserState> sectionContext)
        {
            State = state ?? throw new ArgumentNullException(nameof(state));
            SectionContext = sectionContext ?? throw new ArgumentNullException(nameof(sectionContext));
        }
    }
}
