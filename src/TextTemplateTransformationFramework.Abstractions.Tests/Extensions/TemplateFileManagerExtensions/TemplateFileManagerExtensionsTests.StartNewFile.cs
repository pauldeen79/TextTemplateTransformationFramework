namespace TextTemplateTransformationFramework.Abstractions.Tests.Extensions;

public partial class TemplateFileManagerExtensionsTests
{
    public class StartNewFile : TemplateFileManagerExtensionsTests
    {
        [Fact]
        public void No_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            _ = sut.Object.StartNewFile();

            // Assert
            sut.Verify(x => x.StartNewFile(string.Empty, false), Times.Once);
        }

        [Fact]
        public void Only_FileName_Argument_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            _ = sut.Object.StartNewFile(FileName);

            // Assert
            sut.Verify(x => x.StartNewFile(FileName, false), Times.Once);
        }
    }
}
