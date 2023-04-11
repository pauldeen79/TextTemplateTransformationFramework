using System;
using System.Diagnostics.CodeAnalysis;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.TestFixtures
{
    [ExcludeFromCodeCoverage]
    internal sealed class FileContentsProviderMock : IFileContentsProvider
    {
        public Func<string, string> GetFileContentsDelegate { get; set; }

        public bool FileExists(string fileName)
        {
            throw new NotImplementedException();
        }

        public string GetFileContents(string fileName)
            => GetFileContentsDelegate == null
                ? default
                : GetFileContentsDelegate(fileName);

        public void WriteFileContents(string path, string contents)
        {
            throw new NotImplementedException();
        }
    }
}
