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
        public void Throws_On_Null_TemplateRenderer()
        {
            // Act & Assert
            this.Invoking(_ => new CodeGenerationEngine(null!, TemplateFileManagerMock.Object))
                .Should().Throw<ArgumentNullException>().WithParameterName("templateRenderer");
        }

        [Fact]
        public void Throws_On_Null_TemplateFileManager()
        {
            // Act & Assert
            this.Invoking(_ => new CodeGenerationEngine(TemplateRendererMock.Object, null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("templateFileManager");
        }
    }
}
