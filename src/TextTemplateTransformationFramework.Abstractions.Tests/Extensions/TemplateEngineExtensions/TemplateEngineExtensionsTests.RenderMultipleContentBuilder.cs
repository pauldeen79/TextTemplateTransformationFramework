namespace TextTemplateTransformationFramework.Abstractions.Tests.Extensions;

public partial class TemplateEngineExtensionsTests
{
    public class Render_MultipleContentBuilder : TemplateEngineExtensionsTests
    {
        [Fact]
        public void Template_And_GenerationEnvironment_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderMock.Object);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderMock.Object, string.Empty, null), Times.Once);
        }

        [Fact]
        public void Template_GenerationEnvironment_And_DefaultFilename_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderMock.Object, DefaultFilename);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderMock.Object, DefaultFilename, null), Times.Once);
        }

        [Fact]
        public void Template_GenerationEnvironment_And_AdditionalParameters_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderMock.Object, AdditionalParameters);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderMock.Object, string.Empty, AdditionalParameters), Times.Once);
        }
    }
}
