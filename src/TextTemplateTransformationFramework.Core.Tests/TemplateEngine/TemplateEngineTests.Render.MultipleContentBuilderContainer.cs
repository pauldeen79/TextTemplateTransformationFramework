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
        public void Constructs_Template_When_Possible()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.TextTransformTemplate(() => "Hello world!");
            IMultipleContentBuilderContainer? generationEnvironment = MultipleContentBuilderContainerMock.Object;
            MultipleContentBuilderContainerMock.SetupGet(x => x.MultipleContentBuilder).Returns(MultipleContentBuilderMock.Object);
            var contentBuilderMock = new Mock<IContentBuilder>();
            MultipleContentBuilderMock.Setup(x => x.AddContent(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<StringBuilder?>()))
                                      .Returns<string, bool, StringBuilder?>((filename, skipWhenFileExists, b) =>
                                      {
                                          contentBuilderMock.SetupGet(x => x.Builder).Returns(b ?? new StringBuilder());

                                          return contentBuilderMock.Object;
                                      });

            // Act
            sut.Render(template, generationEnvironment);

            // Assert
            contentBuilderMock.Object.Builder.Should().NotBeNull();
            contentBuilderMock.Object.Builder.ToString().Should().Be("Hello world!");
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
