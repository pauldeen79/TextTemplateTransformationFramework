namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    public interface IEnvironmentVersionToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string Value { get; }
    }
}
