namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.NamespaceFooterTokens
{
    /// <summary>
    /// Contract for rendering code in the namespace footer.
    /// </summary>
    /// <seealso cref="INamespaceFooterToken" />
    /// <seealso cref="ICodeToken" />
    public interface INamespaceFooterCodeToken<TState> : INamespaceFooterToken<TState>, ICodeToken<TState>
        where TState : class
    {
    }
}
