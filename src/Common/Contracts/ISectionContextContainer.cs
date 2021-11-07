namespace TextTemplateTransformationFramework.Common.Contracts
{
    /// <summary>
    /// Allows a model mapper to access the current section context.
    /// </summary>
    public interface ISectionContextContainer<TState>
        where TState : class
    {
        /// <summary>
        /// Gets or sets the section context.
        /// </summary>
        SectionContext<TState> SectionContext { get; set; }
    }
}
