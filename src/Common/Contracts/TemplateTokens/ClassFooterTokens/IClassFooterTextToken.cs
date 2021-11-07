namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.ClassFooterTokens
{
    public interface IClassFooterTextToken<TState> : IClassFooterToken<TState>, ITextToken<TState>
        where TState : class
    {
    }
}
