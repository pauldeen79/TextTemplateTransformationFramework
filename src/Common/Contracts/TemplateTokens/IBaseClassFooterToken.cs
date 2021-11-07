namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    public interface IBaseClassFooterToken<TState> : ITemplateToken<TState>
        where TState : class
    {
    }
}
