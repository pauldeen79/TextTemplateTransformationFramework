namespace TextTemplateTransformationFramework.T4.Common.Contracts
{
    /// <summary>
    /// Allows for a section processor to specify a custom directive name. (as dynamic alternative for the static DirectivePrefixAttribute)
    /// </summary>
    public interface ITemplateCustomDirectiveName
    {
        /// <summary>
        /// Gets the custom directive name.
        /// </summary>
        string TemplateCustomDirectiveName { get; }
    }
}
