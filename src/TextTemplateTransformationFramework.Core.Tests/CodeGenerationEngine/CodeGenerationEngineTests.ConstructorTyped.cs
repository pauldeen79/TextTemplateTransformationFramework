namespace TextTemplateTransformationFramework.Core.Tests;

public partial class CodeGenerationEngineTests
{
    public class Constructor_Typed : CodeGenerationEngineTests
    {
        [Fact]
        public void Creates_Instance_When_All_Arguments_Are_Valid()
        {
            // Act & Assert
            this.Invoking(_ => new CodeGenerationEngine<string>(TypedTemplateEngineMock.Object, TemplateFileManagerFactoryMock.Object)).Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_TemplateEngine()
        {
            // Act & Assert
            this.Invoking(_ => new CodeGenerationEngine<string>(templateEngine: null!, TemplateFileManagerFactoryMock.Object))
                .Should().Throw<ArgumentNullException>().WithParameterName("templateEngine");
        }
    }
}
