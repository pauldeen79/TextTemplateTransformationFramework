namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface ICompositeTemplateSectionProcessor<TState> : INonDiscoverableTemplateSectionProcessor<TState>
        where TState : class
    {
    }
}
