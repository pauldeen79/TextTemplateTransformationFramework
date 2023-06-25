﻿namespace TextTemplateTransformationFramework.Core.Tests;

public partial class MultipleContentBuilderTests
{
    public class SaveLastGeneratedFiles : MultipleContentBuilderTests
    {
        [Fact]
        public void Throws_On_Empty_FullPath()
        {
            // Arrange
            var sut = new MultipleContentBuilder(FileSystemMock.Object, Encoding.UTF8, string.Empty);
            var c1 = sut.AddContent(string.Empty);
            c1.Builder.AppendLine("Test1");

            // Act & Assert
            sut.Invoking(x => x.SaveLastGeneratedFiles(string.Empty))
               .Should().Throw<InvalidOperationException>().WithMessage("Full path could not be determined");
        }

        [Fact]
        public void Creates_Directory_When_It_Does_Not_Exist_Yet()
        {
            // Arrange
            var sut = CreateSut(TestData.BasePath);
            FileSystemMock.Setup(x => x.DirectoryExists(TestData.BasePath)).Returns(false);

            // Act
            sut.SaveLastGeneratedFiles("LastGeneratedFiles.txt");

            // Assert
            FileSystemMock.Verify(x => x.CreateDirectory(TestData.BasePath), Times.Once);
        }

        [Fact]
        public void Creates_File_When_FileName_Does_Not_Contain_Asterisk_Using_NonEmpty_BasePath()
        {
            // Arrange
            var sut = CreateSut(TestData.BasePath);
            FileSystemMock.Setup(x => x.DirectoryExists(TestData.BasePath)).Returns(true);

            // Act
            sut.SaveLastGeneratedFiles("LastGeneratedFiles.txt");

            // Assert
            FileSystemMock.Verify(x => x.WriteAllLines(Path.Combine(TestData.BasePath, "LastGeneratedFiles.txt"), It.IsAny<IEnumerable<string>>(), It.IsAny<Encoding>()), Times.Once);
        }

        [Fact]
        public void Creates_File_When_FileName_Does_Not_Contain_Asterisk_Using_Empty_BasePath()
        {
            // Arrange
            var sut = CreateSut(string.Empty);
            FileSystemMock.Setup(x => x.DirectoryExists("MyDirectory")).Returns(true);

            // Act
            sut.SaveLastGeneratedFiles("LastGeneratedFiles.txt");

            // Assert
            FileSystemMock.Verify(x => x.WriteAllLines("LastGeneratedFiles.txt", It.IsAny<IEnumerable<string>>(), It.IsAny<Encoding>()), Times.Once);
        }

        [Fact]
        public void Does_Not_Create_File_When_FileName_Contains_Asterisk()
        {
            // Arrange
            var sut = CreateSut(TestData.BasePath);
            FileSystemMock.Setup(x => x.DirectoryExists(TestData.BasePath)).Returns(true);

            // Act
            sut.SaveLastGeneratedFiles("*.template.generated.cs");

            // Assert
            FileSystemMock.Verify(x => x.WriteAllLines(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<Encoding>()), Times.Never);
        }
    }
}
