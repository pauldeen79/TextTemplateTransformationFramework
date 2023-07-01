namespace TextTemplateTransformationFramework.Core.Tests;

public partial class CodeGenerationEngineTests
{
    public class Constructor_Typed : CodeGenerationEngineTests
    {
        [Fact]
        public void Creates_Instance_Without_Arguments()
        {
            // Act & Assert
            this.Invoking(_ => new CodeGenerationEngine<string>()).Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_TemplateRenderer()
        {
            // Act & Assert
            this.Invoking(_ => new CodeGenerationEngine<string>(templateRenderer: null!, TemplateFileManagerFactoryMock.Object))
                .Should().Throw<ArgumentNullException>().WithParameterName("templateRenderer");
        }

        [Fact]
        public void Throws_On_Null_BasePath()
        {
            // Act & Assert
            this.Invoking(_ => new CodeGenerationEngine<string>(TypedTemplateRendererMock.Object, TemplateFileManagerFactoryMock.Object, basePath: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("basePath");
        }
    }
}
