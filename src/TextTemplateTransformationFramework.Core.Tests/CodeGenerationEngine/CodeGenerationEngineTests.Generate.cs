namespace TextTemplateTransformationFramework.Core.Tests;

public partial class CodeGenerationEngineTests
{
    public class Generate : CodeGenerationEngineTests
    {
        private Mock<ICodeGenerationProvider> CodeGenerationProviderMock { get; } = new();
        private Mock<ICodeGenerationSettings> CodeGenerationSettingsMock { get; } = new();

        [Fact]
        public void Generates_Multiple_Files_When_Provider_Wants_This()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.GenerateMultipleFiles).Returns(true);

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.IsAny<object>(), It.IsAny<IMultipleContentBuilder>(), It.IsAny<string>(), It.IsAny<object?>()), Times.Once);
        }

        [Fact]
        public void Generates_Single_File_When_Provider_Wants_This()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.DefaultFilename).Returns("MyFile.txt");
            CodeGenerationProviderMock.SetupGet(x => x.GenerateMultipleFiles).Returns(false);

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.IsAny<object>(), It.IsAny<StringBuilder>(), It.IsAny<string>(), It.IsAny<object?>()), Times.Once);
        }

        [Fact]
        public void Processes_Result_On_TemplateFileManager()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.DefaultFilename).Returns("MyFile.txt");

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            TemplateFileManagerMock.Verify(x => x.Process(true, false), Times.Once);
        }
    }
}
