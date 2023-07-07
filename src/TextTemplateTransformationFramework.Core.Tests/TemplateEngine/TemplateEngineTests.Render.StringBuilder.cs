namespace TextTemplateTransformationFramework.Core.Tests;

public partial class TemplateEngineTests
{
    public class Render_StringBuilder : TemplateEngineTests
    {
        [Fact]
        public void Throws_On_Null_Template()
        {
            // Arrange
            var sut = CreateSut();
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
            var sut = CreateSut();
            object? template = this;
            StringBuilder? generationEnvironment = null;

            // Act & Assert
            sut.Invoking(x => x.Render(template!, generationEnvironment!))
               .Should().Throw<ArgumentNullException>().WithParameterName(nameof(generationEnvironment));
        }

        [Fact]
        public void Constructs_Template_When_Possible()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.TextTransformTemplate(() => "Hello world!");
            StringBuilder? generationEnvironment = StringBuilder;

            // Act
            sut.Render(template, generationEnvironment);

            // Assert
            generationEnvironment.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void Sets_AdditionalParameters_On_Template_When_Possible()
        {
            // Arrange
            ITemplateEngine sut = new TemplateEngine();
            var template = new TestData.PlainTemplateWithAdditionalParameters();
            StringBuilder? generationEnvironment = StringBuilder;

            // Act
            sut.Render(template, generationEnvironment, additionalParameters: new { AdditionalParameter = "Some value" });

            // Assert
            template.AdditionalParameter.Should().Be("Some value");
        }
    }
}
