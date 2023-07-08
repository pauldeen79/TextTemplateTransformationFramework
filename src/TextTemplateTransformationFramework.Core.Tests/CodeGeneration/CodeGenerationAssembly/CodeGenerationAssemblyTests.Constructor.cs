namespace TemplateFramework.Core.Tests.CodeGeneration;

public partial class CodeGenerationAssemblyTests
{
    public class Constructor : CodeGenerationAssemblyTests
    {
        [Fact]
        public void Throws_On_Null_CodeGenerationEngine()
        {
            // Act & Assert
            this.Invoking(_ => new CodeGenerationAssembly(codeGenerationEngine: null!, GetAssemblyName(), TestData.BasePath, true, false))
                .Should().Throw<ArgumentNullException>().WithParameterName("codeGenerationEngine");
        }

        [Fact]
        public void Throws_On_Null_AssemblyName()
        {
            // Act & Assert
            this.Invoking(_ => new CodeGenerationAssembly(CodeGenerationEngineMock.Object, assemblyName: null!, TestData.BasePath, true, false))
                .Should().Throw<ArgumentNullException>().WithParameterName("assemblyName");
        }

        [Fact]
        public void Throws_On_Empty_AssemblyName()
        {
            // Act & Assert
            this.Invoking(_ => new CodeGenerationAssembly(CodeGenerationEngineMock.Object, assemblyName: string.Empty, TestData.BasePath, true, false))
                .Should().Throw<ArgumentException>().WithParameterName("assemblyName");
        }

        [Fact]
        public void Throws_On_WhiteSpace_AssemblyName()
        {
            // Act & Assert
            this.Invoking(_ => new CodeGenerationAssembly(CodeGenerationEngineMock.Object, assemblyName: "     ", TestData.BasePath, true, false))
                .Should().Throw<ArgumentException>().WithParameterName("assemblyName");
        }

        [Fact]
        public void Throws_On_Null_BasePath()
        {
            // Act & Assert
            this.Invoking(_ => new CodeGenerationAssembly(CodeGenerationEngineMock.Object, GetAssemblyName(), basePath: null!, true, false))
                .Should().Throw<ArgumentException>().WithParameterName("basePath");
        }
    }
}
