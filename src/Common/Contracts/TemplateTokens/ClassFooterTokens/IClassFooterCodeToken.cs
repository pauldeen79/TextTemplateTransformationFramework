namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.ClassFooterTokens
{
    /// <summary>
    /// Contract for rendering code in the class footer.
    /// </summary>
    /// <seealso cref="IClassFooterToken" />
    /// <seealso cref="ICodeToken" />
    public interface IClassFooterCodeToken<TState> : IClassFooterToken<TState>, ICodeToken<TState>
        where TState : class
    {
    }
}
