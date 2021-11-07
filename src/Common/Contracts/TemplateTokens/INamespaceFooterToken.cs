namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    /// <summary>
    /// Contract for a token that needs to be placed in the namespace footer.
    /// </summary>
    /// <seealso cref="ITemplateToken" />
    public interface INamespaceFooterToken<TState> : ITemplateToken<TState>
        where TState : class
    {
    }
}
