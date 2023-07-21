﻿namespace TemplateFramework.Core.Tests.CodeGeneration;

public partial class CodeGenerationEngineTests
{
    public class Generate : CodeGenerationEngineTests
    {
        [Fact]
        public void Generates_Multiple_Files_When_Provider_Wants_This()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.DefaultFilename).Returns(string.Empty);
            CodeGenerationProviderMock.SetupGet(x => x.GenerateMultipleFiles).Returns(true);
            CodeGenerationSettingsMock.SetupGet(x => x.BasePath).Returns(string.Empty);
            CodeGenerationProviderMock.Setup(x => x.CreateGenerator()).Returns(this);
            TemplateFileManagerMock.SetupGet(x => x.MultipleContentBuilder).Returns(new Mock<IMultipleContentBuilder>().Object);

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, TemplateFileManagerMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.IsAny<IRenderTemplateRequest<object?>>()), Times.Once);
        }

        [Fact]
        public void Generates_Single_File_When_Provider_Wants_This()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.DefaultFilename).Returns("MyFile.txt");
            CodeGenerationProviderMock.SetupGet(x => x.GenerateMultipleFiles).Returns(false);
            CodeGenerationProviderMock.Setup(x => x.CreateGenerator()).Returns(this);
            CodeGenerationSettingsMock.SetupGet(x => x.BasePath).Returns(string.Empty);
            TemplateFileManagerMock.Setup(x => x.StartNewFile(It.IsAny<string>(), It.IsAny<bool>())).Returns(new StringBuilder());

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, TemplateFileManagerMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            TemplateEngineMock.Verify(x => x.Render(It.IsAny<IRenderTemplateRequest<object?>>()), Times.Once);
        }

        [Fact]
        public void Processes_Result_On_TemplateFileManager()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.DefaultFilename).Returns("MyFile.txt");
            CodeGenerationProviderMock.Setup(x => x.CreateGenerator()).Returns(this);
            CodeGenerationSettingsMock.SetupGet(x => x.BasePath).Returns(string.Empty);
            TemplateFileManagerMock.Setup(x => x.StartNewFile(It.IsAny<string>(), It.IsAny<bool>())).Returns(new StringBuilder());

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, TemplateFileManagerMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            TemplateFileManagerMock.Verify(x => x.Process(true, false), Times.Once);
        }
    }
}
