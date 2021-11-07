using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.NamespaceFooterTokens
{
    /// <summary>
    /// Adds support for multiple file output using a template file manager.
    /// </summary>
    /// <seealso cref="ITemplateToken" />
    public interface ITemplateFileManagerToken<TState> : INamespaceFooterToken<TState>
        where TState : class
    {
    }
}
