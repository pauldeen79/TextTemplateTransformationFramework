namespace TemplateFramework.Core.Tests;

public partial class TemplateEngineTests
{
    public class Render_MultipleContentBuilder : TemplateEngineTests
    {
        [Fact]
        public void Throws_On_Null_Request()
        {
            // Arrange
            IRenderTemplateRequest<object?> request = null!;
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Render(request))
               .Should().Throw<ArgumentNullException>().WithParameterName(nameof(request));
        }

        [Fact]
        public void Initializes_Template_Correctly()
        {
            // Arrange
            var sut = new TemplateEngine(TemplateInitializerMock.Object, DefaultTemplateRenderers);
            var template = new TestData.PlainTemplateWithAdditionalParameters();
            IMultipleContentBuilder? generationEnvironment = MultipleContentBuilderMock.Object;
            var additionalParameters = new { AdditionalParameter = "Some value" };
            var request = new RenderTemplateRequest<object?>(template, generationEnvironment, string.Empty, null, additionalParameters, null);

            // Act
            sut.Render(request);

            // Assert
            TemplateInitializerMock.Verify(x => x.Initialize(request, sut), Times.Once);
        }

        [Fact]
        public void Renders_Template_Correctly()
        {
            // Arrange
            var sut = new TemplateEngine(TemplateInitializerMock.Object, new[] { TemplateRendererMock.Object });
            var template = new TestData.PlainTemplateWithAdditionalParameters();
            IMultipleContentBuilder? generationEnvironment = MultipleContentBuilderMock.Object;
            var additionalParameters = new { AdditionalParameter = "Some value" };
            TemplateRendererMock.Setup(x => x.Supports(It.IsAny<IGenerationEnvironment>())).Returns(true);
            var request = new RenderTemplateRequest<object?>(template, generationEnvironment, string.Empty, null, additionalParameters, null);

            // Act
            sut.Render(request);

            // Assert
            TemplateRendererMock.Verify(x => x.Render(template, It.Is<IGenerationEnvironment>(x => x.Type == GenerationEnvironmentType.MultipleContentBuilder), string.Empty), Times.Once);
        }
    }
}
