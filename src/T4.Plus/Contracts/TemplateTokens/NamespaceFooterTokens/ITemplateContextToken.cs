using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.NamespaceFooterTokens
{
    public interface ITemplateContextToken<TState> : INamespaceFooterToken<TState>
        where TState : class
    {
    }
}
