namespace TemplateFramework.Core.Tests.TemplateRenderers;

public partial class SingleContentTemplateRendererTests
{
    public class Supports : SingleContentTemplateRendererTests
    {
        [Fact]
        public void Returns_True_When_GenerationEnvironment_Is_StringBuilder()
        {
            // Arrange
            var sut = CreateSut();
            var environmentMock = new Mock<IGenerationEnvironment>();
            environmentMock.SetupGet(x => x.Type).Returns(GenerationEnvironmentType.StringBuilder);

            // Act
            var result = sut.Supports(environmentMock.Object);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Returns_False_When_GenerationEnvironment_Is_Not_StringBuilder()
        {
            // Arrange
            var sut = CreateSut();
            var environmentMock = new Mock<IGenerationEnvironment>();
            environmentMock.SetupGet(x => x.Type).Returns(GenerationEnvironmentType.MultipleContentBuilder);

            // Act
            var result = sut.Supports(environmentMock.Object);

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
    }
}
