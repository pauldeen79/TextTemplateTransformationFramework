using System.Collections.Generic;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Contracts
{
    public interface ITokenProcessorTokenConverter<TState>
        where TState : class
    {
        IEnumerable<ITemplateToken<TState>> Convert(IEnumerable<ITemplateToken<TState>> tokens);
    }
}
