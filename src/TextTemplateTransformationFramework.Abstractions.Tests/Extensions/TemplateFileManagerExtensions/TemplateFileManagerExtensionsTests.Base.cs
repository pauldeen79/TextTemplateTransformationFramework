namespace TextTemplateTransformationFramework.Abstractions.Tests.Extensions
{
    public partial class TemplateFileManagerExtensionsTests
    {
        protected Mock<ITemplateFileManager> CreateSut() => new();
        protected const string FileName = "FileName.txt";
        protected const string LastGeneratedFilesPath = "LastGeneratedFiles.txt";
        protected const bool Split = true;
    }
}
