namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.NamespaceFooterTokens
{
    public interface INamespaceFooterTextToken<TState> : INamespaceFooterToken<TState>, ITextToken<TState>
        where TState : class
    {
    }
}
