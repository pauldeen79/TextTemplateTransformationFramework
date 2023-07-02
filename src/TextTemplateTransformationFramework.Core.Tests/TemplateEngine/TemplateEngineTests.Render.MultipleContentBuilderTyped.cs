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
        public void Constructs_And_Sets_Model_On_Template_When_Possible()
        {
            // Arrange
            var sut = CreateTypedSut();
            var template = new TestData.TemplateWithModel<string>(_ => { });
            IMultipleContentBuilder? generationEnvironment = MultipleContentBuilderMock.Object;

            // Act
            sut.Render(template, generationEnvironment, model: "Hello world");

            // Assert
            template.Model.Should().NotBeNull();
            template.Model.Should().Be("Hello world");
        }

        [Fact]
        public void Sets_AdditionalParameters_On_Template_When_Possible()
        {
            // Arrange
            var sut = CreateTypedSut();
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
            var sut = CreateTypedSut();
            var template = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();
            IMultipleContentBuilder? generationEnvironment = MultipleContentBuilderMock.Object;

            // Act
            sut.Render(template, generationEnvironment, model: "Hello world", additionalParameters: new { AdditionalParameter = "Some value", Model = "Ignored" });

            // Assert
            template.Model.Should().Be("Hello world");
            template.AdditionalParameter.Should().Be("Some value");
        }
    }
}
