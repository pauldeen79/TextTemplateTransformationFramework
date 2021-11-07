using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.MessageTokens;

namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.NamespaceFooterTokens
{
    public interface INamespaceFooterWarningToken<TState> : INamespaceFooterToken<TState>, IWarningToken<TState>
        where TState : class
    {
    }
}
