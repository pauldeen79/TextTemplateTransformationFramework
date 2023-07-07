namespace TextTemplateTransformationFramework.Core.Tests;

public partial class TemplateEngineTests
{
    public class Render_MultipleContentBuilderContainer : TemplateEngineTests
    {
        [Fact]
        public void Throws_On_Null_Template()
        {
            // Arrange
            var sut = CreateSut();
            object? template = null;
            IMultipleContentBuilderContainer? generationEnvironment = MultipleContentBuilderContainerMock.Object;

            // Act & Assert
            sut.Invoking(x => x.Render(template!, generationEnvironment!))
               .Should().Throw<ArgumentNullException>().WithParameterName(nameof(template));
        }

        [Fact]
        public void Throws_On_Null_GenerationEnvironment()
        {
            // Arrange
            var sut = CreateSut();
            object? template = this;
            IMultipleContentBuilderContainer? generationEnvironment = null;

            // Act & Assert
            sut.Invoking(x => x.Render(template!, generationEnvironment!))
               .Should().Throw<ArgumentNullException>().WithParameterName(nameof(generationEnvironment));
        }

        [Fact]
        public void Initializes_Template_Correctly()
        {
            // Arrange
            var sut = new TemplateEngine(TemplateInitializerMock.Object, DefaultTemplateRenderers);
            var template = new TestData.PlainTemplateWithAdditionalParameters();
            IMultipleContentBuilderContainer? generationEnvironment = MultipleContentBuilderContainerMock.Object;
            MultipleContentBuilderContainerMock.SetupGet(x => x.MultipleContentBuilder).Returns(MultipleContentBuilderMock.Object);
            var additionalParameters = new { AdditionalParameter = "Some value" };

            // Act
            sut.Render(template, generationEnvironment, additionalParameters);

            // Assert
            TemplateInitializerMock.Verify(x => x.Initialize(template, string.Empty, default(object), additionalParameters, null), Times.Once);
        }

        [Fact]
        public void Renders_Template_Correctly()
        {
            // Arrange
            var sut = new TemplateEngine(TemplateInitializerMock.Object, new[] { TemplateRendererMock.Object });
            var template = new TestData.PlainTemplateWithAdditionalParameters();
            IMultipleContentBuilderContainer? generationEnvironment = MultipleContentBuilderContainerMock.Object;
            MultipleContentBuilderContainerMock.SetupGet(x => x.MultipleContentBuilder).Returns(MultipleContentBuilderMock.Object);
            var additionalParameters = new { AdditionalParameter = "Some value" };
            TemplateRendererMock.Setup(x => x.Supports(It.IsAny<object>())).Returns(true);

            // Act
            sut.Render(template, generationEnvironment, additionalParameters);

            // Assert
            TemplateRendererMock.Verify(x => x.Render(template, generationEnvironment, string.Empty), Times.Once);
        }
    }
}
