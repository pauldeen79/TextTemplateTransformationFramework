namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface IFileContentsProvider
    {
        bool DirectoryExists(string directory);
        bool FileExists(string fileName);
        string GetFileContents(string fileName);
        void WriteFileContents(string path, string contents);
        void CreateDirectory(string directory);
    }
}
