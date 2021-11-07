using System.Collections.Generic;
using System.IO;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.FileNameProviders
{
    public sealed class FileSystemFileNameProvider : IFileNameProvider
    {
        public IEnumerable<string> GetFiles(string path, string searchPattern, bool recurse) =>
            string.IsNullOrEmpty(searchPattern)
                ? Directory.GetFiles(path)
                : Directory.GetFiles(path, searchPattern, GetSearchOption(recurse));

        private static SearchOption GetSearchOption(bool recurse)
            => recurse
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;
    }
}
