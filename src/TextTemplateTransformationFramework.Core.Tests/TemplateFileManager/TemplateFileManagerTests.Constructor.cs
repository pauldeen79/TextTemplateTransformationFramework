namespace TextTemplateTransformationFramework.Core.Tests;

public partial class TemplateFileManagerTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_StringBuilder()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateFileManager(stringBuilder: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("stringBuilder");
        }

        [Fact]
        public void Creates_Instance_With_Empty_BasePath()
        {
            // Arrange
            var stringBuilder = new StringBuilder();

            // Act
            var sut = new TemplateFileManager(stringBuilder: stringBuilder, basePath: string.Empty);
            sut.ResetToDefaultOutput(); // needed to check the sent StringBuilder

            // Assert
            sut.MultipleContentBuilder.BasePath.Should().BeEmpty();
            sut.GenerationEnvironment.Should().BeSameAs(stringBuilder);
        }
        
        [Fact]
        public void Creates_Instance_With_Filled_BasePath()
        {
            // Arrange
            var basePath = TestData.BasePath;
            var stringBuilder = new StringBuilder();

            // Act
            var sut = new TemplateFileManager(stringBuilder: stringBuilder, basePath: basePath);
            sut.ResetToDefaultOutput(); // needed to check the sent StringBuilder

            // Assert
            sut.MultipleContentBuilder.BasePath.Should().Be(basePath);
            sut.GenerationEnvironment.Should().BeSameAs(stringBuilder);
        }

        [Fact]
        public void Creates_Instance_Without_BasePath_Argument()
        {
            // Arrange
            var stringBuilder = new StringBuilder();

            // Act
            var sut = new TemplateFileManager(stringBuilder: stringBuilder);
            sut.ResetToDefaultOutput(); // needed to check the sent StringBuilder

            // Assert
            sut.MultipleContentBuilder.BasePath.Should().BeEmpty();
            sut.GenerationEnvironment.Should().BeSameAs(stringBuilder);
        }
    }
}
