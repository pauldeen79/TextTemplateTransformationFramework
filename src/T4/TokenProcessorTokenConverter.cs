using System.Collections.Generic;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.T4.Contracts;

namespace TextTemplateTransformationFramework.T4
{
    public class TokenProcessorTokenConverter<TState> : ITokenProcessorTokenConverter<TState>
        where TState : class
    {
        public IEnumerable<ITemplateToken<TState>> Convert(IEnumerable<ITemplateToken<TState>> tokens)
            => tokens;
    }
}
