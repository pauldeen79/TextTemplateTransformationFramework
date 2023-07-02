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
    }
}
