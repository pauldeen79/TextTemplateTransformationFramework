namespace TextTemplateTransformationFramework.Core.Tests;

public partial class TemplateFileManagerTests
{
    public class StartNewFile : TemplateFileManagerTests
    {
        [Fact]
        public void Throws_On_FileName_Null()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.StartNewFile(fileName: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("fileName");
        }

        [Fact]
        public void Adds_New_Content_To_MultipleContentBuilder()
        {
            // Arrange
            var sut = CreateSut();
            MultipleContentBuilderMock.Setup(x => x.AddContent("MyFile.txt", false, It.IsAny<StringBuilder>())).Returns<string, bool, StringBuilder>((fileName, skipFileWhenExists, builder) => CreateContent(fileName, skipFileWhenExists, builder));

            // Act
            sut.StartNewFile("MyFile.txt");

            // Assert
            MultipleContentBuilderMock.Verify(x => x.AddContent("MyFile.txt", false, It.IsAny<StringBuilder>()), Times.Once);
        }

        [Fact]
        public void Sets_GenerationEnvironment_To_Builder_Of_Added_Content()
        {
            // Arrange
            var sut = CreateSut();
            IContent? createdContent = null;
            MultipleContentBuilderMock.Setup(x => x.AddContent("MyFile.txt", false, It.IsAny<StringBuilder>())).Returns<string, bool, StringBuilder>((fileName, skipFileWhenExists, builder) => { createdContent = CreateContent(fileName, skipFileWhenExists, builder); return createdContent; });

            // Act
            sut.StartNewFile("MyFile.txt");

            // Assert
            sut.GenerationEnvironment.Should().BeSameAs(createdContent?.Builder);
        }

        [Fact]
        public void Returns_Builder_Of_Added_Content()
        {
            // Arrange
            var sut = CreateSut();
            IContent? createdContent = null;
            MultipleContentBuilderMock.Setup(x => x.AddContent("MyFile.txt", false, It.IsAny<StringBuilder>())).Returns<string, bool, StringBuilder>((fileName, skipFileWhenExists, builder) => { createdContent = CreateContent(fileName, skipFileWhenExists, builder); return createdContent; });

            // Act
            var result = sut.StartNewFile("MyFile.txt");

            // Assert
            result.Should().BeSameAs(createdContent?.Builder);
        }
    }
}
