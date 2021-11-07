namespace TextTemplateTransformationFramework.Common.Contracts
{
    /// <summary>
    /// Processes a template section.
    /// </summary>
    public interface ITemplateSectionProcessor<TState>
        where TState : class
    {
        /// <summary>
        /// Processes the specified section.
        /// </summary>
        /// <param name="context">The section (context) to process.</param>
        /// <returns>Section process result.</returns>
        SectionProcessResult<TState> Process(SectionContext<TState> context);
    }
}
