using System;
using System.Diagnostics.CodeAnalysis;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Tests.TestFixtures
{
    [ExcludeFromCodeCoverage]
    internal sealed class FileContentsProviderMock : IFileContentsProvider
    {
        public Func<string, string> GetFileContentsDelegate { get; set; }

        public void CreateDirectory(string directory)
        {
            throw new NotImplementedException();
        }

        public bool DirectoryExists(string directory)
        {
            throw new NotImplementedException();
        }

        public bool FileExists(string fileName)
        {
            throw new NotImplementedException();
        }

        public string GetFileContents(string fileName)
            => GetFileContentsDelegate is null
                ? default
                : GetFileContentsDelegate(fileName);

        public void WriteFileContents(string path, string contents)
        {
            throw new NotImplementedException();
        }
    }
}
