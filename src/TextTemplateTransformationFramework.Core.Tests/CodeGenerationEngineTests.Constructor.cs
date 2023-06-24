namespace TextTemplateTransformationFramework.Core.Tests
{
    public partial class CodeGenerationEngineTests
    {
        public class Constructor
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
                this.Invoking(_ => new CodeGenerationEngine(null!, new Mock<ITemplateFileManager>().Object))
                    .Should().Throw<ArgumentNullException>().WithParameterName("templateRenderer");
            }

            [Fact]
            public void Throws_On_Null_TemplateFileManager()
            {
                this.Invoking(_ => new CodeGenerationEngine(new Mock<ITemplateRenderer>().Object, null!))
                    .Should().Throw<ArgumentNullException>().WithParameterName("templateFileManager");
            }
        }
    }
}
