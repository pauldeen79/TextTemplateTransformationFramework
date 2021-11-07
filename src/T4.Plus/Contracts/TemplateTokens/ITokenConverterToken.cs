using System.Collections.Generic;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens
{
    public interface ITokenConverterToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        IEnumerable<ITemplateToken<TState>> Convert(IEnumerable<ITemplateToken<TState>> result);
    }
}
