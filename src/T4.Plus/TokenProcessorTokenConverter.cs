using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.T4.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus
{
    public class TokenProcessorTokenConverter<TState> : ITokenProcessorTokenConverter<TState>
        where TState : class
    {
        public IEnumerable<ITemplateToken<TState>> Convert(IEnumerable<ITemplateToken<TState>> tokens)
        {
            var result = tokens
                .OrderBy(t => t is IOrderedItem oi ? oi.Order : 100)
                .ToArray()
                .AsEnumerable();

            foreach (var tokenConverterToken in result.OfType<ITokenConverterToken<TState>>().ToArray())
            {
                result = tokenConverterToken.Convert(result);
            }

            return result;
        }
    }
}
