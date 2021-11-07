namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface IFileContentsProviderContainer
    {
        IFileContentsProvider FileContentsProvider { get; set; }
    }
}
