using System.IO;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.FileContentsProviders
{
    public sealed class FileSystemFileContentsProvider : IFileContentsProvider
    {
        public string GetFileContents(string fileName) =>
            File.ReadAllText(fileName);
    }
}
