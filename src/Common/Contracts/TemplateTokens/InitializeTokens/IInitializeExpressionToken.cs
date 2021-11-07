namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.InitializeTokens
{
    /// <summary>
    /// Contract for defining an expression to render in the class footer.
    /// </summary>
    /// <seealso cref="IInitializeToken" />
    /// <seealso cref="IExpressionToken" />
    public interface IInitializeExpressionToken<TState> : IInitializeToken<TState>, IExpressionToken<TState>
        where TState : class
    {
    }
}
