namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    public interface ITemplateGenerationEnvironmentAccessorToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        string GenerationEnvironmentAccessor { get; }
    }
}
