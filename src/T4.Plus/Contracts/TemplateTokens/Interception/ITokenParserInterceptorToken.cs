using System.Collections.Generic;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.Interception
{
    public interface ITokenParserInterceptorToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        IEnumerable<string> Parse(SectionContext<TState> context, string argumentName);
    }
}
