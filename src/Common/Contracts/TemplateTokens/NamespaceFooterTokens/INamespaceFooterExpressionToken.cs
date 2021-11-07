namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.NamespaceFooterTokens
{
    /// <summary>
    /// Contract for defining an expression to render in the namespace footer.
    /// </summary>
    /// <seealso cref="INamespaceFooterToken" />
    /// <seealso cref="IExpressionToken" />
    public interface INamespaceFooterExpressionToken<TState> : INamespaceFooterToken<TState>, IExpressionToken<TState>
        where TState : class
    {
    }
}
