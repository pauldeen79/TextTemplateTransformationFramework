namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    public interface IExpressionToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string Expression { get; }
    }
}
