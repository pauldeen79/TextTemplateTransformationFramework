namespace TextTemplateTransformationFramework.Core.Tests;

public partial class MultipleContentBuilderTests
{
    public class SaveAll : MultipleContentBuilderTests
    {
        [Fact]
        public void Uses_Content_FileName_When_BasePath_Is_Empty()
        {
            // Arrange
            var sut = CreateSut(string.Empty);

            // Act
            sut.SaveAll();

            // Assert
            FileSystemMock.Verify(x => x.WriteAllText("File1.txt", "Test1" + Environment.NewLine, It.IsAny<Encoding>()), Times.Once);
            FileSystemMock.Verify(x => x.WriteAllText("File2.txt", "Test2" + Environment.NewLine, It.IsAny<Encoding>()), Times.Once);
        }

        [Fact]
        public void Uses_Content_FileName_When_Content_FileName_Is_A_Full_Path()
        {
            // Arrange
            var sut = new MultipleContentBuilder(FileSystemMock.Object, Encoding.UTF8, string.Empty);
            var c1 = sut.AddContent(Path.Combine(TestData.BasePath, "File1.txt"));
            c1.Builder.AppendLine("Test1");

            // Act
            sut.SaveAll();

            // Assert
            FileSystemMock.Verify(x => x.WriteAllText(Path.Combine(TestData.BasePath, "File1.txt"), "Test1" + Environment.NewLine, Encoding.UTF8), Times.Once);
        }

        [Fact]
        public void Uses_Combined_Path_When_Content_FileName_Is_Not_A_Full_Path_And_BasePath_Is_Filled()
        {
            // Arrange
            var sut = CreateSut(TestData.BasePath);

            // Act
            sut.SaveAll();

            // Assert
            FileSystemMock.Verify(x => x.WriteAllText(Path.Combine(TestData.BasePath, "File1.txt"), "Test1" + Environment.NewLine, It.IsAny<Encoding>()), Times.Once);
            FileSystemMock.Verify(x => x.WriteAllText(Path.Combine(TestData.BasePath, "File2.txt"), "Test2" + Environment.NewLine, It.IsAny<Encoding>()), Times.Once);
        }

        [Fact]
        public void Throws_When_FileName_Is_Empty()
        {
            // Arrange
            var sut = new MultipleContentBuilder(FileSystemMock.Object, Encoding.UTF8, string.Empty);
            var c1 = sut.AddContent(string.Empty);
            c1.Builder.AppendLine("Test1");

            // Act & Assert
            sut.Invoking(x => x.SaveAll())
               .Should().Throw<InvalidOperationException>().WithMessage("Path could not be determined");
        }

        [Fact]
        public void Skips_Writing_File_When_SkipWhenFileExists_Is_True_And_File_Already_Exists()
        {
            // Arrange
            var sut = CreateSut(string.Empty, skipWhenFileExists: true);
            FileSystemMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);

            // Act
            sut.SaveAll();

            // Assert
            FileSystemMock.Verify(x => x.WriteAllText(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Encoding>()), Times.Never);
        }

        [Fact]
        public void Creates_Directory_When_It_Does_Not_Exist_Yet()
        {
            // Arrange
            var sut = CreateSut(TestData.BasePath);
            int counter = 0;
            FileSystemMock.Setup(x => x.DirectoryExists(TestData.BasePath)).Returns(() =>
            {
                counter++;
                return counter == 1;
            });

            // Act
            sut.SaveAll();

            // Assert
            FileSystemMock.Verify(x => x.CreateDirectory(TestData.BasePath), Times.Once);
        }

        [Fact]
        public void Uses_Specified_Encoding()
        {
            // Arrange
            var sut = CreateSut(TestData.BasePath, encoding: Encoding.UTF32);

            // Act
            sut.SaveAll();

            // Assert
            FileSystemMock.Verify(x => x.WriteAllText(Path.Combine(TestData.BasePath, "File1.txt"), "Test1" + Environment.NewLine, Encoding.UTF32), Times.Once);
            FileSystemMock.Verify(x => x.WriteAllText(Path.Combine(TestData.BasePath, "File2.txt"), "Test2" + Environment.NewLine, Encoding.UTF32), Times.Once);
        }

        [Fact]
        public void Uses_Utf8Encoding_When_Not_Specified()
        {
            // Arrange
            var sut = CreateSut(TestData.BasePath, encoding: default);

            // Act
            sut.SaveAll();

            // Assert
            FileSystemMock.Verify(x => x.WriteAllText(Path.Combine(TestData.BasePath, "File1.txt"), "Test1" + Environment.NewLine, Encoding.UTF8), Times.Once);
            FileSystemMock.Verify(x => x.WriteAllText(Path.Combine(TestData.BasePath, "File2.txt"), "Test2" + Environment.NewLine, Encoding.UTF8), Times.Once);
        }
    }
}
