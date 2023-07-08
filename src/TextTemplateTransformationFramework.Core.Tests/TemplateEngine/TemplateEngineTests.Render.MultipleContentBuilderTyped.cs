namespace TextTemplateTransformationFramework.Core.Tests;

public partial class TemplateEngineTests
{
    public class Render_MultipleContentBuilder_Typed : TemplateEngineTests
    {
        [Fact]
        public void Throws_On_Null_Template()
        {
            // Arrange
            var sut = CreateTypedSut();
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
            var sut = CreateTypedSut();
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
            var sut = new TemplateEngine<string>(TemplateInitializerMock.Object, DefaultTemplateRenderers);
            var template = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            IMultipleContentBuilder? generationEnvironment = MultipleContentBuilderMock.Object;
            var model = "Hello world";
            var additionalParameters = new { AdditionalParameter = "Some value", Model = "Ignored" };

            // Act
            sut.Render(template, generationEnvironment, model, additionalParameters);

            // Assert
            TemplateInitializerMock.Verify(x => x.Initialize(template, string.Empty, sut, model, additionalParameters, null), Times.Once);
        }

        [Fact]
        public void Renders_Template_Correctly()
        {
            // Arrange
            var sut = new TemplateEngine<string>(TemplateInitializerMock.Object, new[] { TemplateRendererMock.Object });
            var template = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            IMultipleContentBuilder? generationEnvironment = MultipleContentBuilderMock.Object;
            var model = "Hello world";
            var additionalParameters = new { AdditionalParameter = "Some value", Model = "Ignored" };
            TemplateRendererMock.Setup(x => x.Supports(It.IsAny<object>())).Returns(true);

            // Act
            sut.Render(template, generationEnvironment, model, additionalParameters);

            // Assert
            TemplateRendererMock.Verify(x => x.Render(template, generationEnvironment, string.Empty), Times.Once);
        }
    }
}
