using System.IO;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.FileContentsProviders
{
    public sealed class FileSystemFileContentsProvider : IFileContentsProvider
    {
        public bool FileExists(string fileName)
         => File.Exists(fileName);

        public string GetFileContents(string fileName)
            => File.ReadAllText(fileName);

        public void WriteFileContents(string path, string contents)
            => File.WriteAllText(path, contents);
    }
}
