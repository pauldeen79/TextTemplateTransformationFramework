using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.NamespaceFooterTokens
{
    public interface IChildTemplateClassBaseToken<TState> : INamespaceFooterToken<TState>
        where TState : class
    {
        string ClassName { get; }
    }
}
