namespace TextTemplateTransformationFramework.Common.Contracts.TemplateSectionProcessors
{
    public interface ITextSectionProcessor<TState> : INonDiscoverableTemplateSectionProcessor<TState>
        where TState : class
    {
    }
}
