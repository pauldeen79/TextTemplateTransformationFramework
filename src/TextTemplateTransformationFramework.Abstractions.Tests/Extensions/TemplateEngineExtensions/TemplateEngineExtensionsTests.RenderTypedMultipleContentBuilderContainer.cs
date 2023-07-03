namespace TextTemplateTransformationFramework.Abstractions.Tests.Extensions;

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
            sut.Verify(x => x.Render(Template, MultipleContentBuilderContainerMock.Object, Model, string.Empty, null), Times.Once);
        }

        [Fact]
        public void Template_GenerationEnvironment_And_DefaultFileName_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateTypedSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderContainerMock.Object, Model, DefaultFileName);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderContainerMock.Object, Model, DefaultFileName, null), Times.Once);
        }

        [Fact]
        public void Template_GenerationEnvironment_And_AdditionalParameters_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateTypedSut();

            // Act
            sut.Object.Render(Template, MultipleContentBuilderContainerMock.Object, Model, AdditionalParameters);

            // Assert
            sut.Verify(x => x.Render(Template, MultipleContentBuilderContainerMock.Object, Model, string.Empty, AdditionalParameters), Times.Once);
        }
    }
}
