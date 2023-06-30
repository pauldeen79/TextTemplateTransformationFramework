namespace TextTemplateTransformationFramework.Core.Tests;

public partial class TemplateRendererTests
{
    public class Render_MultipleContentBuilder : TemplateRendererTests
    {
        [Fact]
        public void Throws_On_Null_Template()
        {
            // Arrange
            var sut = new TemplateRenderer();
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
            var sut = new TemplateRenderer();
            object? template = this;
            IMultipleContentBuilder? generationEnvironment = null;

            // Act & Assert
            sut.Invoking(x => x.Render(template!, generationEnvironment!))
               .Should().Throw<ArgumentNullException>().WithParameterName(nameof(generationEnvironment));
        }

        [Fact]
        public void Constructs_And_Sets_Model_On_Template_When_Possible()
        {
            // Arrange
            var sut = new TemplateRenderer();
            var template = new TestData.TemplateWithModel(_ => { });
            IMultipleContentBuilder? generationEnvironment = MultipleContentBuilderMock.Object;

            // Act
            sut.Render(template, generationEnvironment, model: "Hello world");

            // Assert
            template.Model.Should().NotBeNull();
            template.Model.Get().Should().BeEquivalentTo("Hello world");
        }

        [Fact]
        public void Initializes_And_Sets_Model_On_Template_When_Possible()
        {
            // Arrange
            var sut = new TemplateRenderer();
            var template = new TestData.TemplateWithModel(_ => { });
            var modelMock = new Mock<IModel>();
            object? modelValue = null;
            modelMock.Setup(x => x.Get()).Returns(() => modelValue);
            modelMock.Setup(x => x.Set(It.IsAny<object?>())).Callback<object?>(x => modelValue = x);
            template.Model = modelMock.Object;
            IMultipleContentBuilder ? generationEnvironment = MultipleContentBuilderMock.Object;

            // Act
            sut.Render(template, generationEnvironment, model: "Hello world");

            // Assert
            modelMock.Verify(x => x.Initialize(), Times.Once);
            template.Model.Should().NotBeNull();
            template.Model.Get().Should().BeEquivalentTo("Hello world");
        }
        [Fact]
        public void Sets_AdditionalParameters_On_Template_When_Possible()
        {
            // Arrange
            var sut = new TemplateRenderer();
            var template = new TestData.PlainTemplateWithAdditionalParameters();
            IMultipleContentBuilder? generationEnvironment = MultipleContentBuilderMock.Object;

            // Act
            sut.Render(template, generationEnvironment, additionalParameters: new { AdditionalParameter = "Some value" });

            // Assert
            template.AdditionalParameter.Should().Be("Some value");
        }

        [Fact]
        public void Sets_Model_And_AdditionalParameters_On_Template_When_Possible()
        {
            // Arrange
            var sut = new TemplateRenderer();
            var template = new TestData.PlainTemplateWithModelAndAdditionalParameters();
            IMultipleContentBuilder? generationEnvironment = MultipleContentBuilderMock.Object;

            // Act
            sut.Render(template, generationEnvironment, model: "Hello world", additionalParameters: new { AdditionalParameter = "Some value", Model = "Ignored" });

            // Assert
            template.Model.Should().NotBeNull();
            template.Model.Get().Should().BeEquivalentTo("Hello world");
            template.AdditionalParameter.Should().Be("Some value");
        }
    }
}
