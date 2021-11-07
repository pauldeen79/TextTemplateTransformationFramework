namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.RenderTokens
{
    /// <summary>
    /// Contract for defining an expression to render.
    /// </summary>
    /// <seealso cref="IRenderToken" />
    /// <seealso cref="IExpressionToken" />
    public interface IRenderExpressionToken<TState> : IRenderToken<TState>, IExpressionToken<TState>
        where TState : class
    {
    }
}
