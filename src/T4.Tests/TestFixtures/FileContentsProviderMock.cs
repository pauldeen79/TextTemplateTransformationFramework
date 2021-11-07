using System;
using System.Diagnostics.CodeAnalysis;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Tests.TestFixtures
{
    [ExcludeFromCodeCoverage]
    internal class FileContentsProviderMock : IFileContentsProvider
    {
        public Func<string, string> GetFileContentsDelegate { get; set; }

        public string GetFileContents(string fileName)
            => GetFileContentsDelegate == null
                ? default
                : GetFileContentsDelegate(fileName);
    }
}
