namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    /// <summary>
    /// Contract for a namespace import token.
    /// </summary>
    /// <seealso cref="ITemplateToken" />
    public interface INamespaceImportToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string Namespace { get; }
    }
}
