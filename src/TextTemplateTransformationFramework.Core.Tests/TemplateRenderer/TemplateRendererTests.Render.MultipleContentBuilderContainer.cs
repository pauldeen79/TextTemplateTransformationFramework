namespace TextTemplateTransformationFramework.Core.Tests;

public partial class TemplateRendererTests
{
    public class Render_MultipleContentBuilderContainer : TemplateRendererTests
    {
        [Fact]
        public void Throws_On_Null_Template()
        {
            // Arrange
            var sut = new TemplateRenderer();
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
            var sut = new TemplateRenderer();
            object? template = this;
            IMultipleContentBuilderContainer? generationEnvironment = null;

            // Act & Assert
            sut.Invoking(x => x.Render(template!, generationEnvironment!))
               .Should().Throw<ArgumentNullException>().WithParameterName(nameof(generationEnvironment));
        }
    }
}
