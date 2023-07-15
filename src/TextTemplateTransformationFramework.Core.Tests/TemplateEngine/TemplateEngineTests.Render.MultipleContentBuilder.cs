namespace TemplateFramework.Core.Tests;

public partial class TemplateEngineTests
{
    public class Render_MultipleContentBuilder : TemplateEngineTests
    {
        [Fact]
        public void Throws_On_Null_Template()
        {
            // Arrange
            var sut = CreateSut();
            object? template = null;
            IMultipleContentBuilder? generationEnvironment = MultipleContentBuilderMock.Object;

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
            IMultipleContentBuilder? generationEnvironment = null;

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
            IMultipleContentBuilder? generationEnvironment = MultipleContentBuilderMock.Object;
            var additionalParameters = new { AdditionalParameter = "Some value" };

            // Act
            sut.Render(template, generationEnvironment, null, additionalParameters);

            // Assert
            TemplateInitializerMock.Verify(x => x.Initialize(template, string.Empty, sut, It.IsAny<object?>(), additionalParameters, null), Times.Once);
        }

        [Fact]
        public void Renders_Template_Correctly()
        {
            // Arrange
            var sut = new TemplateEngine(TemplateInitializerMock.Object, new[] { TemplateRendererMock.Object });
            var template = new TestData.PlainTemplateWithAdditionalParameters();
            IMultipleContentBuilder? generationEnvironment = MultipleContentBuilderMock.Object;
            var additionalParameters = new { AdditionalParameter = "Some value" };
            TemplateRendererMock.Setup(x => x.Supports(It.IsAny<object>())).Returns(true);

            // Act
            sut.Render(template, generationEnvironment, additionalParameters);

            // Assert
            TemplateRendererMock.Verify(x => x.Render(template, generationEnvironment, string.Empty), Times.Once);
        }
    }
}
