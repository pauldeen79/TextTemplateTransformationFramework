namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.RenderTokens
{
    /// <summary>
    /// Contract for rendering code in the Render method.
    /// </summary>
    /// <seealso cref="IRenderToken" />
    /// <seealso cref="ICodeToken" />
    public interface IRenderCodeToken<TState> : IRenderToken<TState>, ICodeToken<TState>
        where TState : class
    {
    }
}
