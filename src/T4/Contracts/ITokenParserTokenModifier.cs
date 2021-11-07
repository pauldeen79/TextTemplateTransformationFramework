using System.Collections.Generic;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Contracts
{
    public interface ITokenParserTokenModifier
    {
        IEnumerable<ITemplateToken<TokenParserState>> Modify(IEnumerable<ITemplateToken<TokenParserState>> tokens);
    }
}
