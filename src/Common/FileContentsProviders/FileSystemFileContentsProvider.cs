using System.Diagnostics.CodeAnalysis;
using System.IO;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.FileContentsProviders
{
    [ExcludeFromCodeCoverage]
    public sealed class FileSystemFileContentsProvider : IFileContentsProvider
    {
        public void CreateDirectory(string directory)
            => Directory.CreateDirectory(directory);

        public bool DirectoryExists(string directory)
            => Directory.Exists(directory);

        public bool FileExists(string fileName)
            => File.Exists(fileName);

        public string GetFileContents(string fileName)
            => File.ReadAllText(fileName);

        public void WriteFileContents(string path, string contents)
            => File.WriteAllText(path, contents);
    }
}
