using System.Collections.Generic;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.T4.Contracts;

namespace TextTemplateTransformationFramework.T4
{
    public class TokenModifier : ITokenParserTokenModifier
    {
        public IEnumerable<ITemplateToken<TokenParserState>> Modify(IEnumerable<ITemplateToken<TokenParserState>> tokens)
            => tokens;
    }
}
