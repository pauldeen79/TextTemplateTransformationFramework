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
        public void Creates_Instance_With_BasePath_Argument()
        {
            // Act & Assert
            this.Invoking(_ => new CodeGenerationEngine<string>(TestData.BasePath)).Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_TemplateEngine()
        {
            // Act & Assert
            this.Invoking(_ => new CodeGenerationEngine<string>(templateEngine: null!, TemplateFileManagerFactoryMock.Object))
                .Should().Throw<ArgumentNullException>().WithParameterName("templateEngine");
        }

        [Fact]
        public void Throws_On_Null_BasePath()
        {
            // Act & Assert
            this.Invoking(_ => new CodeGenerationEngine<string>(TypedTemplateEngineMock.Object, TemplateFileManagerFactoryMock.Object, basePath: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("basePath");
        }
    }
}
