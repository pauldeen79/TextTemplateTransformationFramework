using System.Collections.Generic;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Contracts
{
    public interface ITokenPlaceholderProcessor<TState>
        where TState : class
    {
        string Process(string value, IEnumerable<ITemplateToken<TState>> existingTokens, TokenParserState state);
    }
}
