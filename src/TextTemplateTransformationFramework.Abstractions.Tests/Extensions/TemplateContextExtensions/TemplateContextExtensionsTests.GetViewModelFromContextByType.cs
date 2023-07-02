namespace TextTemplateTransformationFramework.Abstractions.Tests.Extensions.TemplateContextExtensions;

public partial class TemplateContextExtensionsTests
{
    public class GetViewModelFromContextByType : TemplateContextExtensionsTests
    {
        [Fact]
        public void Without_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            sut.Object.GetViewModelFromContextByType<string>();

            // Assert
            sut.Verify(x => x.GetViewModelFromContextByType<string>(null), Times.Once);
        }
    }
}
