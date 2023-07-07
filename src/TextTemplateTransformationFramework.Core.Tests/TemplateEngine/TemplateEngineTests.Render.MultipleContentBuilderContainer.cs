namespace TextTemplateTransformationFramework.Core.Tests;

public partial class TemplateEngineTests
{
    public class Render_MultipleContentBuilderContainer : TemplateEngineTests
    {
        [Fact]
        public void Throws_On_Null_Template()
        {
            // Arrange
            var sut = CreateSut();
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
            var sut = CreateSut();
            object? template = this;
            IMultipleContentBuilderContainer? generationEnvironment = null;

            // Act & Assert
            sut.Invoking(x => x.Render(template!, generationEnvironment!))
               .Should().Throw<ArgumentNullException>().WithParameterName(nameof(generationEnvironment));
        }

        [Fact]
        public void Sets_AdditionalParameters_On_Template_When_Possible()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.PlainTemplateWithAdditionalParameters();
            IMultipleContentBuilderContainer? generationEnvironment = MultipleContentBuilderContainerMock.Object;
            MultipleContentBuilderContainerMock.SetupGet(x => x.MultipleContentBuilder).Returns(MultipleContentBuilderMock.Object);

            // Act
            sut.Render(template, generationEnvironment, additionalParameters: new { AdditionalParameter = "Some value" });

            // Assert
            template.AdditionalParameter.Should().Be("Some value");
        }
    }
}
