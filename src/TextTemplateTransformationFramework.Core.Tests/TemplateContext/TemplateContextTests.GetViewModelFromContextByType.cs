namespace TemplateFramework.Core.Tests;

public partial class TemplateContextTests
{
    public class GetViewModelFromContextByType : TemplateContextTests
    {
        [Fact]
        public void Returns_Null_When_Model_Type_Could_Not_Be_Found()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetViewModelFromContextByType<GetViewModelFromContextByType>();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Returns_ViewModel_From_Parent_When_Correct_Not_Using_Predicate()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetViewModelFromContextByType<string>();

            // Assert
            result.Should().Be("test viewmodel");
        }

        [Fact]
        public void Returns_ViewModel_From_Root_When_Correct_Using_Predicate_Returns_True()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetViewModelFromContextByType<int>(_ => true);

            // Assert
            result.Should().Be(2);
        }

        [Fact]
        public void Returns_ViewModel_From_Root_When_Correct_Using_Predicate_Returns_False()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetViewModelFromContextByType<int>(_ => false);

            // Assert
            result.Should().Be(default);
        }
    }
}
