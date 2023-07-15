namespace TemplateFramework.Abstractions.Tests.Extensions;

public partial class TemplateEngineExtensionsTests
{
    public class Render_MultipleContentBuilder : TemplateEngineExtensionsTests
    {
        [Fact]
        public void Template_And_GenerationEnvironment_And_Model_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderMock.Object, Model);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderMock.Object, Model, string.Empty, null, null), Times.Once);
        }

        [Fact]
        public void Template_And_GenerationEnvironment_And_Model_And_DefaultFilename_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderMock.Object, Model, DefaultFilename);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderMock.Object, Model, DefaultFilename, null, null), Times.Once);
        }

        [Fact]
        public void Template_And_GenerationEnvironment_And_Model_And_AdditionalParameters_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderMock.Object, Model, AdditionalParameters);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderMock.Object, Model, string.Empty, AdditionalParameters, null), Times.Once);
        }

        [Fact]
        public void Template_And_GenerationEnvironment_And_Model_And_TemplateContext_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderMock.Object, Model, TemplateContextMock.Object);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderMock.Object, Model, string.Empty, null, TemplateContextMock.Object), Times.Once);
        }

        [Fact]
        public void Template_And_GenerationEnvironment_And_Model_And_DefaultFilename_And_TemplateContext_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderMock.Object, Model, DefaultFilename, TemplateContextMock.Object);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderMock.Object, Model, DefaultFilename, null, TemplateContextMock.Object), Times.Once);
        }

        [Fact]
        public void Template_And_GenerationEnvironment_And_Model_And_AdditionalParameters_And_TemplateContext_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderMock.Object, Model, AdditionalParameters, TemplateContextMock.Object);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderMock.Object, Model, string.Empty, AdditionalParameters, TemplateContextMock.Object), Times.Once);
        }

        [Fact]
        public void Template_And_GenerationEnvironment_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderMock.Object);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderMock.Object, default(object?), string.Empty, null, null), Times.Once);
        }

        [Fact]
        public void Template_And_GenerationEnvironment_And_DefaultFileName_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderMock.Object, DefaultFilename);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderMock.Object, default(object?), DefaultFilename, null, null), Times.Once);
        }

        [Fact]
        public void Template_And_GenerationEnvironment_And_TemplateContext_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderMock.Object, TemplateContextMock.Object);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderMock.Object, default(object?), string.Empty, null, TemplateContextMock.Object), Times.Once);
        }

        [Fact]
        public void Template_And_GenerationEnvironment_And_DefaultFileName_And_TemplateContext_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderMock.Object, DefaultFilename, TemplateContextMock.Object);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderMock.Object, default(object?), DefaultFilename, null, TemplateContextMock.Object), Times.Once);
        }
    }
}
