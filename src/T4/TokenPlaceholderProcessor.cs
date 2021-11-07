using System.Collections.Generic;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.T4.Contracts;

namespace TextTemplateTransformationFramework.T4
{
    public class TokenPlaceholderProcessor<TState> : ITokenPlaceholderProcessor<TState>
        where TState : class
    {
        public string Process(string value, IEnumerable<ITemplateToken<TState>> existingTokens, TokenParserState state)
            => value;
    }
}
