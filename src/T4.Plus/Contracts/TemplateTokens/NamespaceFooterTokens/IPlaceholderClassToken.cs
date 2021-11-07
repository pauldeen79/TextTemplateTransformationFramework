using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.NamespaceFooterTokens
{
    public interface IPlaceholderClassToken<TState> : INamespaceFooterToken<TState>
        where TState : class
    {
        string ClassName { get; }
        string BaseClass { get; }
        string RootClassName { get; }
        string ModelType { get; }
        bool UseModelForRoutingOnly { get; }
    }
}
