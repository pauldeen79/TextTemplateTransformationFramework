namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.BaseClassFooterTokens
{
    /// <summary>
    /// Contract for defining an expression to render in the base class footer.
    /// </summary>
    /// <seealso cref="IClassFooterToken" />
    /// <seealso cref="IExpressionToken" />
    public interface IBaseClassFooterExpressionToken<TState> : IBaseClassFooterToken<TState>, IExpressionToken<TState>
        where TState : class
    {
    }
}
