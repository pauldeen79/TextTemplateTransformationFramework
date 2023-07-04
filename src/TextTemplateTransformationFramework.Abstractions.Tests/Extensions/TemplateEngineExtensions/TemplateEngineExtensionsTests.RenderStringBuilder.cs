namespace TextTemplateTransformationFramework.Abstractions.Tests.Extensions;

public partial class TemplateEngineExtensionsTests
{
    public class Render_StringBuilder : TemplateEngineExtensionsTests
    {
        [Fact]
        public void Template_And_GenerationEnvironment_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, StringBuilder);

            // Assert
            sut.Verify(x => x.Render(Template, StringBuilder, string.Empty, null), Times.Once);
        }

        [Fact]
        public void Template_GenerationEnvironment_And_DefaultFilename_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, StringBuilder, DefaultFilename);

            // Assert
            sut.Verify(x => x.Render(Template, StringBuilder, DefaultFilename, null), Times.Once);
        }

        [Fact]
        public void Template_GenerationEnvironment_And_AdditionalParameters_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, StringBuilder, AdditionalParameters);

            // Assert
            sut.Verify(x => x.Render(Template, StringBuilder, string.Empty, AdditionalParameters), Times.Once);
        }
    }
}
