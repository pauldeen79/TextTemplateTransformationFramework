namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    public interface IGeneratorToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string Name { get; }
        string Version { get; }
    }
}
