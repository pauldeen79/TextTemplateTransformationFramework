namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    public interface IOverrideTemplateToken<TState> : ITemplateToken<TState>
        where TState : class
    {
    }
}
