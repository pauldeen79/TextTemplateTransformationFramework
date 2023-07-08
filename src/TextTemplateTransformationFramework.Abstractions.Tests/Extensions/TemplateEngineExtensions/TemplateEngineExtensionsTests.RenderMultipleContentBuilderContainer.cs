namespace TemplateFramework.Abstractions.Tests.Extensions;

public partial class TemplateEngineExtensionsTests
{
    public class Render_MultipleContentBuilderContainer : TemplateEngineExtensionsTests
    {
        [Fact]
        public void Template_And_GenerationEnvironment_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderContainerMock.Object);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderContainerMock.Object, string.Empty, null, null), Times.Once);
        }

        [Fact]
        public void Template_GenerationEnvironment_And_DefaultFilename_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderContainerMock.Object, DefaultFilename);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderContainerMock.Object, DefaultFilename, null, null), Times.Once);
        }

        [Fact]
        public void Template_GenerationEnvironment_And_AdditionalParameters_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderContainerMock.Object, AdditionalParameters);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderContainerMock.Object, string.Empty, AdditionalParameters, null), Times.Once);
        }

        [Fact]
        public void Template_And_GenerationEnvironment_And_TemplateContext_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderContainerMock.Object, TemplateContextMock.Object);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderContainerMock.Object, string.Empty, null, TemplateContextMock.Object), Times.Once);
        }

        [Fact]
        public void Template_GenerationEnvironment_And_DefaultFilename_And_TemplateContext_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderContainerMock.Object, DefaultFilename, TemplateContextMock.Object);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderContainerMock.Object, DefaultFilename, null, TemplateContextMock.Object), Times.Once);
        }

        [Fact]
        public void Template_GenerationEnvironment_And_AdditionalParameters_And_TemplateContext_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderContainerMock.Object, AdditionalParameters, TemplateContextMock.Object);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderContainerMock.Object, string.Empty, AdditionalParameters, TemplateContextMock.Object), Times.Once);
        }
    }
}
