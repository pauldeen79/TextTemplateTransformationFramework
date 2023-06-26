namespace TextTemplateTransformationFramework.Core.Tests;

public partial class TemplateFileManagerFactoryTests
{
    public class Create
    {
        [Fact]
        public void Creates_New_TemplateFileManager_Instance()
        {
            // Arrange
            var sut = new TemplateFileManagerFactory();

            // Act
            var result = sut.Create(TestData.BasePath);

            // Assert
            result.Should().BeOfType<Core.TemplateFileManager>();
        }
    }
}
