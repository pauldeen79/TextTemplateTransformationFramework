namespace TextTemplateTransformationFramework.Core.Tests.TemplateRenderers;

public partial class MultipleContentTemplateRendererTests
{
    public class Supports : MultipleContentTemplateRendererTests
    {
        [Fact]
        public void Returns_False_When_GenerationEnvironment_Is_StringBuilder()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Supports(new StringBuilder());

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_False_When_GenerationEnvironment_Is_Null()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Supports(null!);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_True_When_GenerationEnvironment_Is_MultipleContentBuilder()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Supports(new Mock<IMultipleContentBuilder>().Object);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Returns_True_When_GenerationEnvironment_Is_MultipleContentBuilderContainer()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Supports(new Mock<IMultipleContentBuilderContainer>().Object);

            // Assert
            result.Should().BeTrue();
        }

    }
}
