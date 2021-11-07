using System.Collections.Generic;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.Interception
{
    public interface ITokenProcessorInterceptorToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        TemplateCodeOutput<TState> Process(IEnumerable<ITemplateToken<TState>> tokens, ICallback<TState, ITokenProcessor<TState>> callback);
    }
}
