namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface IFileContentsProvider
    {
        bool FileExists(string fileName);
        string GetFileContents(string fileName);
        void WriteFileContents(string path, string contents);
    }
}
