namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.ClassFooterTokens
{
    /// <summary>
    /// Contract for defining an expression to render in the class footer.
    /// </summary>
    /// <seealso cref="IClassFooterToken" />
    /// <seealso cref="IExpressionToken" />
    public interface IClassFooterExpressionToken<TState> : IClassFooterToken<TState>, IExpressionToken<TState>
        where TState : class
    {
    }
}
