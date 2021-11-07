namespace TextTemplateTransformationFramework.Common.Contracts.TemplateTokens
{
    public interface ITemplateSectionProcessorTemplateToken<TState> : ITemplateToken<TState>
        where TState : class
    {
        ITemplateSectionProcessor<TState> TemplateSectionProcessor { get; }
    }
}
