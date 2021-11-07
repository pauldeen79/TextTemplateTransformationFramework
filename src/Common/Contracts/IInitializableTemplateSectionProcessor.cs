namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface IInitializableTemplateSectionProcessor<TState> : ITemplateSectionProcessor<TState>
        where TState : class
    {
        /// <summary>
        /// Initializes this isntance from the current section.
        /// </summary>
        /// <param name="context">The section (context) where this template section processor was registered.</param>
        /// <returns>Section process result.</returns>
        SectionProcessResult<TState> Initialize(SectionContext<TState> context);
    }
}
