namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.BaseClassFooterTokens
{
    public interface IBaseClassFooterTextToken<TState> : IBaseClassFooterToken<TState>, ITextToken<TState>
        where TState : class
    {
    }
}
