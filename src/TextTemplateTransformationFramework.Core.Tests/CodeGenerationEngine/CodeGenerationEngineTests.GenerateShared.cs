namespace TextTemplateTransformationFramework.Core.Tests;

public partial class CodeGenerationEngineTests
{
    public class Generate_Shared : CodeGenerationEngineTests
    {
        [Fact]
        public void Throws_On_Null_CodeGenerationProvider()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Invoking(x => x.Generate(provider: null!, CodeGenerationSettingsMock.Object))
               .Should().Throw<ArgumentNullException>().WithParameterName("provider");
        }

        [Fact]
        public void Throws_On_Null_CodeGenerationSettings()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Invoking(x => x.Generate(CodeGenerationProviderMock.Object, settings: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("settings");
        }

        [Fact]
        public void Saves_Result_When_BasePath_Is_Filled_And_DryRun_Is_False()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.BasePath).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.DefaultFilename).Returns("MyFile.txt");
            CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(false);
            CodeGenerationSettingsMock.SetupGet(x => x.BasePath).Returns(string.Empty);

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            TemplateFileManagerMock.Verify(x => x.SaveAll(), Times.Once);
        }

        [Fact]
        public void Does_Not_Save_Result_When_BasePath_Is_Filled_But_DryRun_Is_True()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.BasePath).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.DefaultFilename).Returns("MyFile.txt");
            CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(true);
            CodeGenerationSettingsMock.SetupGet(x => x.BasePath).Returns(string.Empty);

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            TemplateFileManagerMock.Verify(x => x.SaveAll(), Times.Never);
        }

        [Fact]
        public void Does_Not_Save_Result_When_BasePath_Is_Empty()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.BasePath).Returns(string.Empty);
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.DefaultFilename).Returns("MyFile.txt");
            CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(false);
            CodeGenerationSettingsMock.SetupGet(x => x.BasePath).Returns(string.Empty);

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            TemplateFileManagerMock.Verify(x => x.SaveAll(), Times.Never);
        }

        [Fact]
        public void Deletes_Previous_LastGeneratedFiles_File_When_Provider_Has_NonEmpty_LastGeneratedFilesFilename()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.BasePath).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.GenerateMultipleFiles).Returns(true);
            CodeGenerationProviderMock.SetupGet(x => x.LastGeneratedFilesFilename).Returns("GeneratedFiles.txt");
            CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(false);
            CodeGenerationSettingsMock.SetupGet(x => x.BasePath).Returns(string.Empty);

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            TemplateFileManagerMock.Verify(x => x.DeleteLastGeneratedFiles(Path.Combine(TestData.BasePath, "GeneratedFiles.txt"), false), Times.Once);
        }

        [Fact]
        public void Does_Not_Delete_Previous_LastGeneratedFiles_File_When_Provider_Has_Empty_LastGeneratedFilesFilename()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.BasePath).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.GenerateMultipleFiles).Returns(true);
            CodeGenerationProviderMock.SetupGet(x => x.LastGeneratedFilesFilename).Returns(string.Empty);
            CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(false);
            CodeGenerationSettingsMock.SetupGet(x => x.BasePath).Returns(string.Empty);

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            TemplateFileManagerMock.Verify(x => x.DeleteLastGeneratedFiles(Path.Combine(TestData.BasePath, "GeneratedFiles.txt"), false), Times.Never);
        }

        [Fact]
        public void Writes_Next_LastGeneratedFiles_File_When_Provider_Has_NonEmpty_LastGeneratedFilesFilename()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.BasePath).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.GenerateMultipleFiles).Returns(true);
            CodeGenerationProviderMock.SetupGet(x => x.LastGeneratedFilesFilename).Returns("GeneratedFiles.txt");
            CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(false);
            CodeGenerationSettingsMock.SetupGet(x => x.BasePath).Returns(string.Empty);

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            TemplateFileManagerMock.Verify(x => x.SaveLastGeneratedFiles(Path.Combine(TestData.BasePath, "GeneratedFiles.txt")), Times.Once);
        }

        [Fact]
        public void Does_Not_Write_Next_LastGeneratedFiles_File_When_Provider_Has_Empty_LastGeneratedFilesFilename()
        {
            // Arrange
            var sut = CreateSut();
            CodeGenerationProviderMock.SetupGet(x => x.BasePath).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(TestData.BasePath);
            CodeGenerationProviderMock.SetupGet(x => x.GenerateMultipleFiles).Returns(true);
            CodeGenerationProviderMock.SetupGet(x => x.LastGeneratedFilesFilename).Returns(string.Empty);
            CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(false);
            CodeGenerationSettingsMock.SetupGet(x => x.BasePath).Returns(string.Empty);

            // Act
            sut.Generate(CodeGenerationProviderMock.Object, CodeGenerationSettingsMock.Object);

            // Assert
            TemplateFileManagerMock.Verify(x => x.SaveLastGeneratedFiles(Path.Combine(TestData.BasePath, "GeneratedFiles.txt")), Times.Never);
        }
    }
}
