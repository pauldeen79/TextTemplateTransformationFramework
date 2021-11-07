namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.BaseClassFooterTokens
{
    /// <summary>
    /// Contract for rendering code in the class footer.
    /// </summary>
    /// <seealso cref="IBaseClassFooterToken" />
    /// <seealso cref="ICodeToken" />
    public interface IBaseClassFooterCodeToken<TState> : IBaseClassFooterToken<TState>, ICodeToken<TState>
        where TState : class
    {
    }
}
