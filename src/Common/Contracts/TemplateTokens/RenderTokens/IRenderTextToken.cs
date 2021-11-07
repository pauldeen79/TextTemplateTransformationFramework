namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.RenderTokens
{
    public interface IRenderTextToken<TState> : IRenderToken<TState>, ITextToken<TState>
        where TState : class
    {
    }
}
