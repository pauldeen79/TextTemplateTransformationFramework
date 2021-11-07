using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.NamespaceFooterTokens
{
    public interface IChildViewModelNamespaceFooterClassToken<TState> : IChildViewModelClassToken<TState>, INamespaceFooterToken<TState>
        where TState : class
    {
    }
}
