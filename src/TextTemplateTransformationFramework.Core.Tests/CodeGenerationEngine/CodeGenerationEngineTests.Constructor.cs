namespace TextTemplateTransformationFramework.Core.Tests;

public partial class CodeGenerationEngineTests
{
    public class Constructor : CodeGenerationEngineTests
    {
        [Fact]
        public void Creates_Instance_Without_Arguments()
        {
            // Act & Assert
            this.Invoking(_ => new CodeGenerationEngine()).Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_TemplateEngine()
        {
            // Act & Assert
            this.Invoking(_ => new CodeGenerationEngine(templateEngine: null!, TemplateFileManagerFactoryMock.Object))
                .Should().Throw<ArgumentNullException>().WithParameterName("templateEngine");
        }
    }
}
