namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    public interface ICodeToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string Code { get; }
    }
}
