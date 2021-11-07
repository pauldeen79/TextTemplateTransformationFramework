namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface INonDiscoverableTemplateSectionProcessor<TState> : ITemplateSectionProcessor<TState>
        where TState : class
    {
    }
}
