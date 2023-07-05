namespace TextTemplateTransformationFramework.Core.Tests;

public partial class TemplateEngineTests
{
    public class Render_StringBuilder_Typed : TemplateEngineTests
    {
        [Fact]
        public void Throws_On_Null_Template()
        {
            // Arrange
            var sut = CreateTypedSut();
            object? template = null;
            StringBuilder? generationEnvironment = StringBuilder;

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
            StringBuilder? generationEnvironment = null;

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
            StringBuilder? generationEnvironment = StringBuilder;

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
            StringBuilder? generationEnvironment = StringBuilder;

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
            StringBuilder? generationEnvironment = StringBuilder;

            // Act
            sut.Render(template, generationEnvironment, model: "Hello world", additionalParameters: new { AdditionalParameter = "Some value", Model = "Ignored" });

            // Assert
            template.Model.Should().Be("Hello world");
            template.AdditionalParameter.Should().Be("Some value");
        }
    }
}
