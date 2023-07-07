namespace TextTemplateTransformationFramework.Core.Tests;

public partial class ChildTemplateFactoryTests
{
    public class CreateByName : ChildTemplateFactoryTests
    {
        [Fact]
        public void Throws_On_Null_Argument()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.CreateByName(name: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("name");
        }

        [Fact]
        public void Throws_When_Model_Is_Not_Supported()
        {
            // Arrange
            var sut = CreateSut();
            ChildTemplateCreatorMock.Setup(x => x.SupportsName(It.IsAny<string>())).Returns(false);

            // Act & Assert
            sut.Invoking(x => x.CreateByName("test"))
               .Should().Throw<NotSupportedException>().WithMessage("Name test is not supported");
        }

        [Fact]
        public void Throws_When_TemplateCreator_Returns_Null_Instance()
        {
            // Arrange
            var sut = CreateSut();
            ChildTemplateCreatorMock.Setup(x => x.SupportsName(It.IsAny<string>())).Returns(true);
            ChildTemplateCreatorMock.Setup(x => x.CreateByName(It.IsAny<string>())).Returns(null!);

            // Act & Assert
            sut.Invoking(x => x.CreateByName("test"))
               .Should().Throw<InvalidOperationException>().WithMessage("Child template creator returned a null instance");
        }

        [Fact]
        public void Returns_Instance_When_Model_Is_Supported()
        {
            // Arrange
            var sut = CreateSut();
            var template = new object();
            ChildTemplateCreatorMock.Setup(x => x.SupportsName(It.IsAny<string>())).Returns<string>(x => x == "test");
            ChildTemplateCreatorMock.Setup(x => x.CreateByName(It.IsAny<string>())).Returns(template);

            // Act
            var result = sut.CreateByName("test");

            // Assert
            result.Should().BeSameAs(template);
        }
    }
}
