namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class SingleContentTemplateRendererTests
{
    public class Render : SingleContentTemplateRendererTests
    {
        [Fact]
        public void Throws_When_GenerationEnvironment_Is_Not_StringBuilder()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Render(new TestData.Template(_ => { }), generationEnvironment: this, DefaultFilename))
               .Should().Throw<ArgumentException>().WithParameterName("generationEnvironment");
        }

        [Fact]
        public void Throws_When_GenerationEnvironment_Is_Null()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Render(new TestData.Template(_ => { }), generationEnvironment: null!, DefaultFilename))
               .Should().Throw<ArgumentException>().WithParameterName("generationEnvironment");
        }

        [Fact]
        public void Renders_Template_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.Template(b => b.Append("Hello world!"));
            var generationEnvironment = new StringBuilder();

            // Act
            sut.Render(template, generationEnvironment, DefaultFilename);

            // Assert
            generationEnvironment.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void Renders_TextTransformTemplate_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.TextTransformTemplate(() => "Hello world!");
            var generationEnvironment = new StringBuilder();

            // Act
            sut.Render(template, generationEnvironment, DefaultFilename);

            // Assert
            generationEnvironment.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void Renders_NonTemplateType_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.PlainTemplate(() => "Hello world!");
            var generationEnvironment = new StringBuilder();

            // Act
            sut.Render(template, generationEnvironment, DefaultFilename);

            // Assert
            generationEnvironment.ToString().Should().Be("Hello world!");
        }
    }
}
