using System.Collections.Generic;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Models
{
    public class ChildTemplateTokenInfo<TState>
        where TState : class
    {
        public ChildTemplateTokenInfo(IEnumerable<ITemplateToken<TState>> childTokens, IEnumerable<ITemplateToken<TState>> rootTokens)
        {
            ChildTokens = childTokens;
            RootTokens = rootTokens;
        }

        public IEnumerable<ITemplateToken<TState>> ChildTokens { get; }
        public IEnumerable<ITemplateToken<TState>> RootTokens { get; }
    }
}
