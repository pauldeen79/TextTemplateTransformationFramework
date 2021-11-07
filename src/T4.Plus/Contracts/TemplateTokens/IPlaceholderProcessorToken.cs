using System.Collections.Generic;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens
{
    public interface IPlaceholderProcessorToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string Process(string value, IEnumerable<ITemplateToken<TState>> existingTokens);
    }
}
