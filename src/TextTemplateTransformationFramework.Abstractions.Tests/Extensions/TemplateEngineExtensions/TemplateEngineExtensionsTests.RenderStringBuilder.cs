namespace TemplateFramework.Abstractions.Tests.Extensions;

public partial class TemplateEngineExtensionsTests
{
    public class Render_StringBuilder : TemplateEngineExtensionsTests
    {
        [Fact]
        public void Template_And_GenerationEnvironment_And_Model_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, StringBuilder, Model);

            // Assert
            sut.Verify(x => x.Render(Template, StringBuilder, Model, string.Empty, null, null), Times.Once);
        }

        [Fact]
        public void Template_And_GenerationEnvironment_And_Model_And_DefaultFilename_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, StringBuilder, Model, DefaultFilename);

            // Assert
            sut.Verify(x => x.Render(Template, StringBuilder, Model, DefaultFilename, null, null), Times.Once);
        }

        [Fact]
        public void Template_And_GenerationEnvironment_And_Model_And_AdditionalParameters_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, StringBuilder, Model, AdditionalParameters);

            // Assert
            sut.Verify(x => x.Render(Template, StringBuilder, Model, string.Empty, AdditionalParameters, null), Times.Once);
        }

        [Fact]
        public void Template_And_GenerationEnvironment_And_Model_And_TemplateContext_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, StringBuilder, Model, TemplateContextMock.Object);

            // Assert
            sut.Verify(x => x.Render(Template, StringBuilder, Model, string.Empty, null, TemplateContextMock.Object), Times.Once);
        }

        [Fact]
        public void Template_And_GenerationEnvironment_And_Model_And_DefaultFilename_And_TemplateContext_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, StringBuilder, Model, DefaultFilename, TemplateContextMock.Object);

            // Assert
            sut.Verify(x => x.Render(Template, StringBuilder, Model, DefaultFilename, null, TemplateContextMock.Object), Times.Once);
        }

        [Fact]
        public void Template_And_GenerationEnvironment_And_Model_And_AdditionalParameters_And_TemplateContext_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, StringBuilder, Model, AdditionalParameters, TemplateContextMock.Object);

            // Assert
            sut.Verify(x => x.Render(Template, StringBuilder, Model, string.Empty, AdditionalParameters, TemplateContextMock.Object), Times.Once);
        }

        [Fact]
        public void Template_And_GenerationEnvironment_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, StringBuilder);

            // Assert
            sut.Verify(x => x.Render(Template, StringBuilder, default(object?), string.Empty, null, null), Times.Once);
        }

        [Fact]
        public void Template_And_GenerationEnvironment_And_DefaultFileName_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, StringBuilder, DefaultFilename);

            // Assert
            sut.Verify(x => x.Render(Template, StringBuilder, default(object?), DefaultFilename, null, null), Times.Once);
        }

        [Fact]
        public void Template_And_GenerationEnvironment_And_TemplateContext_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, StringBuilder, TemplateContextMock.Object);

            // Assert
            sut.Verify(x => x.Render(Template, StringBuilder, default(object?), string.Empty, null, TemplateContextMock.Object), Times.Once);
        }

        [Fact]
        public void Template_And_GenerationEnvironment_And_DefaultFileName_And_TemplateContext_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, StringBuilder, DefaultFilename, TemplateContextMock.Object);

            // Assert
            sut.Verify(x => x.Render(Template, StringBuilder, default(object?), DefaultFilename, null, TemplateContextMock.Object), Times.Once);
        }
    }
}
