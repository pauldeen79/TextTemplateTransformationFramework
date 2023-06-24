namespace TextTemplateTransformationFramework.Core.Tests
{
    public partial class CodeGenerationEngineTests
    {
        public class Generate : CodeGenerationEngineTests
        {
            private Mock<ICodeGenerationProvider> CodeGenerationProviderMock { get; } = new();
            private Mock<ICodeGenerationSettings> CodeGenerationSettingsMock { get; } = new();

            [Fact]
            public void Throws_On_Null_CodeGenerationProvider()
            {
                // Arrange
                var sut = CreateSut();

                // Act
                sut.Invoking(x => x.Generate(null!, CodeGenerationSettingsMock.Object))
                   .Should().Throw<ArgumentNullException>().WithParameterName("provider");
            }

            [Fact]
            public void Throws_On_Null_CodeGenerationSettings()
            {
                // Arrange
                var sut = CreateSut();

                // Act
                sut.Invoking(x => x.Generate(CodeGenerationProviderMock.Object, null!))
                   .Should().Throw<ArgumentNullException>().WithParameterName("settings");
            }

            [Fact]
            public void Generates_Multiple_Files_When_Provider_Wants_This()
            {
                // Arrange
                var sut = CreateSut();
                CodeGenerationProviderMock.SetupGet(x => x.GenerateMultipleFiles).Returns(true);

                // Act
                sut.Generate(CodeGenerationProviderMock.Object, CodeGenerationSettingsMock.Object);

                // Assert
                TemplateRendererMock.Verify(x => x.Render(It.IsAny<object>(), It.IsAny<IMultipleContentBuilder>(), It.IsAny<string>(), It.IsAny<object?>(), It.IsAny<object?>()), Times.Once);
            }

            [Fact]
            public void Generates_Single_File_When_Provider_Wants_This()
            {
                // Arrange
                var sut = CreateSut();
                CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(@"C:\");
                CodeGenerationProviderMock.SetupGet(x => x.DefaultFileName).Returns("MyFile.txt");
                CodeGenerationProviderMock.SetupGet(x => x.GenerateMultipleFiles).Returns(false);

                // Act
                sut.Generate(CodeGenerationProviderMock.Object, CodeGenerationSettingsMock.Object);

                // Assert
                TemplateRendererMock.Verify(x => x.Render(It.IsAny<object>(), It.IsAny<StringBuilder>(), It.IsAny<string>(), It.IsAny<object?>(), It.IsAny<object?>()), Times.Once);
            }

            [Fact]
            public void Processes_Result_On_TemplateFileManager()
            {
                // Arrange
                var sut = CreateSut();
                CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(@"C:\");
                CodeGenerationProviderMock.SetupGet(x => x.DefaultFileName).Returns("MyFile.txt");

                // Act
                sut.Generate(CodeGenerationProviderMock.Object, CodeGenerationSettingsMock.Object);

                // Assert
                TemplateFileManagerMock.Verify(x => x.Process(true, false), Times.Once);
            }

            [Fact]
            public void Saves_Result_When_BasePath_Is_Filled_And_DryRun_Is_False()
            {
                // Arrange
                var sut = CreateSut();
                CodeGenerationProviderMock.SetupGet(x => x.BasePath).Returns(@"C:\");
                CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(@"C:\");
                CodeGenerationProviderMock.SetupGet(x => x.DefaultFileName).Returns("MyFile.txt");
                CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(false);

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
                CodeGenerationProviderMock.SetupGet(x => x.BasePath).Returns(@"C:\");
                CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(@"C:\");
                CodeGenerationProviderMock.SetupGet(x => x.DefaultFileName).Returns("MyFile.txt");
                CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(true);

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
                CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(@"C:\");
                CodeGenerationProviderMock.SetupGet(x => x.DefaultFileName).Returns("MyFile.txt");
                CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(false);

                // Act
                sut.Generate(CodeGenerationProviderMock.Object, CodeGenerationSettingsMock.Object);

                // Assert
                TemplateFileManagerMock.Verify(x => x.SaveAll(), Times.Never);
            }

            [Fact]
            public void Deletes_Previous_LastGeneratedFiles_File_When_Provider_Has_NonEmpty_LastGeneratedFilesFileName()
            {
                // Arrange
                var sut = CreateSut();
                CodeGenerationProviderMock.SetupGet(x => x.BasePath).Returns(@"C:\");
                CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(@"C:\");
                CodeGenerationProviderMock.SetupGet(x => x.GenerateMultipleFiles).Returns(true);
                CodeGenerationProviderMock.SetupGet(x => x.LastGeneratedFilesFileName).Returns("GeneratedFiles.txt");
                CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(false);

                // Act
                sut.Generate(CodeGenerationProviderMock.Object, CodeGenerationSettingsMock.Object);

                // Assert
                TemplateFileManagerMock.Verify(x => x.DeleteLastGeneratedFiles(@"C:\GeneratedFiles.txt", false), Times.Once);
            }

            [Fact]
            public void Does_Not_Delete_Previous_LastGeneratedFiles_File_When_Provider_Has_Empty_LastGeneratedFilesFileName()
            {
                // Arrange
                var sut = CreateSut();
                CodeGenerationProviderMock.SetupGet(x => x.BasePath).Returns(@"C:\");
                CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(@"C:\");
                CodeGenerationProviderMock.SetupGet(x => x.GenerateMultipleFiles).Returns(true);
                CodeGenerationProviderMock.SetupGet(x => x.LastGeneratedFilesFileName).Returns(string.Empty);
                CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(false);

                // Act
                sut.Generate(CodeGenerationProviderMock.Object, CodeGenerationSettingsMock.Object);

                // Assert
                TemplateFileManagerMock.Verify(x => x.DeleteLastGeneratedFiles(@"C:\GeneratedFiles.txt", false), Times.Never);
            }

            [Fact]
            public void Writes_Next_LastGeneratedFiles_File_When_Provider_Has_NonEmpty_LastGeneratedFilesFileName()
            {
                // Arrange
                var sut = CreateSut();
                CodeGenerationProviderMock.SetupGet(x => x.BasePath).Returns(@"C:\");
                CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(@"C:\");
                CodeGenerationProviderMock.SetupGet(x => x.GenerateMultipleFiles).Returns(true);
                CodeGenerationProviderMock.SetupGet(x => x.LastGeneratedFilesFileName).Returns("GeneratedFiles.txt");
                CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(false);

                // Act
                sut.Generate(CodeGenerationProviderMock.Object, CodeGenerationSettingsMock.Object);

                // Assert
                TemplateFileManagerMock.Verify(x => x.SaveLastGeneratedFiles(@"C:\GeneratedFiles.txt"), Times.Once);
            }

            [Fact]
            public void Does_Not_Write_Next_LastGeneratedFiles_File_When_Provider_Has_Empty_LastGeneratedFilesFileName()
            {
                // Arrange
                var sut = CreateSut();
                CodeGenerationProviderMock.SetupGet(x => x.BasePath).Returns(@"C:\");
                CodeGenerationProviderMock.SetupGet(x => x.Path).Returns(@"C:\");
                CodeGenerationProviderMock.SetupGet(x => x.GenerateMultipleFiles).Returns(true);
                CodeGenerationProviderMock.SetupGet(x => x.LastGeneratedFilesFileName).Returns(string.Empty);
                CodeGenerationSettingsMock.SetupGet(x => x.DryRun).Returns(false);

                // Act
                sut.Generate(CodeGenerationProviderMock.Object, CodeGenerationSettingsMock.Object);

                // Assert
                TemplateFileManagerMock.Verify(x => x.SaveLastGeneratedFiles(@"C:\GeneratedFiles.txt"), Times.Never);
            }
        }
    }
}
