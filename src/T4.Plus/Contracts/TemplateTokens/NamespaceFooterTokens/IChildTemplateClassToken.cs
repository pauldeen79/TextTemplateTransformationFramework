using System.Collections.Generic;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.NamespaceFooterTokens
{
    public interface IChildTemplateClassToken<TState> : INamespaceFooterToken<TState>
        where TState : class
    {
        string ClassName { get; }
        string BaseClass { get; }
        string RootClassName { get; }
        string ModelType { get; }
        IEnumerable<ITemplateToken<TState>> ChildTemplateTokens { get; }
        bool UseModelForRoutingOnly { get; }
    }
}
