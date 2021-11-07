namespace TextTemplateTransformationFramework.Common.Contracts.TemplateSectionProcessors
{
    public interface ICodeSectionProcessor<TState> : INonDiscoverableTemplateSectionProcessor<TState>
        where TState : class
    {
    }
}
