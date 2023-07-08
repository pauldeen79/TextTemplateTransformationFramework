namespace TemplateFramework.Abstractions.Tests.Extensions;

public partial class TemplateEngineExtensionsTests
{
    public class RenderTyped_MultipleContentBuilderContainer : TemplateEngineExtensionsTests
    {
        [Fact]
        public void Template_And_GenerationEnvironment_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateTypedSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderContainerMock.Object, Model);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderContainerMock.Object, Model, string.Empty, null, null), Times.Once);
        }

        [Fact]
        public void Template_GenerationEnvironment_And_DefaultFilename_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateTypedSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderContainerMock.Object, Model, DefaultFilename);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderContainerMock.Object, Model, DefaultFilename, null, null), Times.Once);
        }

        [Fact]
        public void Template_GenerationEnvironment_And_AdditionalParameters_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateTypedSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderContainerMock.Object, Model, AdditionalParameters);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderContainerMock.Object, Model, string.Empty, AdditionalParameters, null), Times.Once);
        }

        [Fact]
        public void Template_And_GenerationEnvironment_And_TemplateContext_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateTypedSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderContainerMock.Object, Model, TemplateContextMock.Object);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderContainerMock.Object, Model, string.Empty, null, TemplateContextMock.Object), Times.Once);
        }

        [Fact]
        public void Template_GenerationEnvironment_And_DefaultFilename_And_TemplateContext_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateTypedSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderContainerMock.Object, Model, DefaultFilename, TemplateContextMock.Object);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderContainerMock.Object, Model, DefaultFilename, null, TemplateContextMock.Object), Times.Once);
        }

        [Fact]
        public void Template_GenerationEnvironment_And_AdditionalParameters_And_TemplateContext_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateTypedSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderContainerMock.Object, Model, AdditionalParameters, TemplateContextMock.Object);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderContainerMock.Object, Model, string.Empty, AdditionalParameters, TemplateContextMock.Object), Times.Once);
        }
    }
}
