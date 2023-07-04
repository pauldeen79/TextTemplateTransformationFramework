namespace TextTemplateTransformationFramework.Abstractions.Tests.Extensions;

public partial class TemplateEngineExtensionsTests
{
    public class RenderTyped_MultipleContentBuilder : TemplateEngineExtensionsTests
    {
        [Fact]
        public void Template_And_GenerationEnvironment_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateTypedSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderMock.Object, Model);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderMock.Object, Model, string.Empty, null), Times.Once);
        }

        [Fact]
        public void Template_GenerationEnvironment_And_DefaultFilename_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateTypedSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderMock.Object, Model, DefaultFilename);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderMock.Object, Model, DefaultFilename, null), Times.Once);
        }

        [Fact]
        public void Template_GenerationEnvironment_And_AdditionalParameters_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateTypedSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderMock.Object, Model, AdditionalParameters);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderMock.Object, Model, string.Empty, AdditionalParameters), Times.Once);
        }
    }
}
