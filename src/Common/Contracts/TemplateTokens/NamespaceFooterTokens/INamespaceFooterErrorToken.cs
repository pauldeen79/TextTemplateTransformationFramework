using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.MessageTokens;

namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.NamespaceFooterTokens
{
    public interface INamespaceFooterErrorToken<TState> : INamespaceFooterToken<TState>, IErrorToken<TState>
        where TState : class
    {
    }
}
