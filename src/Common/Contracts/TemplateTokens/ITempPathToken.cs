namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    public interface ITempPathToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string Value { get; }
    }
}
