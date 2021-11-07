using System.Collections.Generic;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.Interception
{
    public interface ITemplateParserInterceptorToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        IEnumerable<ITemplateToken<TState>> Process(TextTemplate textTemplate, IEnumerable<TemplateParameter> parameters, ICallback<TState, ITextTemplateTokenParser<TState>> callback);
    }
}
