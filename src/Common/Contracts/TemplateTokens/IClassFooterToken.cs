namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    /// <summary>
    /// Contract for a token that needs to be placed in the class footer.
    /// </summary>
    /// <seealso cref="ITemplateToken" />
    public interface IClassFooterToken<TState> : ITemplateToken<TState>
        where TState : class
    {
    }
}
