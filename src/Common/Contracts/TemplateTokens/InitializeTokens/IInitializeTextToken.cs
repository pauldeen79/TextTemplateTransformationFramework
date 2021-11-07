namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.InitializeTokens
{
    public interface IInitializeTextToken<TState> : IInitializeToken<TState>, ITextToken<TState>
        where TState : class
    {
    }
}
