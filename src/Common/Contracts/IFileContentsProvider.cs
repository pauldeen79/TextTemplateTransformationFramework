namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface IFileContentsProvider
    {
        string GetFileContents(string fileName);
    }
}
