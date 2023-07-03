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
        public void Template_GenerationEnvironment_And_DefaultFileName_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderMock.Object, DefaultFileName);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderMock.Object, DefaultFileName, null), Times.Once);
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
