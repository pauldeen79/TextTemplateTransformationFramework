namespace TemplateFramework.Core.Tests.CodeGeneration;

public partial class CodeGenerationAssemblyTests
{
    public class Generate : CodeGenerationAssemblyTests
    {
        [Fact]
        public void Runs_All_CodeGenerators_In_Specified_Assembly()
        {
            // Arrange
            using var sut = new CodeGenerationAssembly(CodeGenerationEngineMock.Object, GetAssemblyName(), TestData.BasePath, true, true);

            // Act
            _ = sut.Generate();

            // Assert
            CodeGenerationEngineMock.Verify(x => x.Generate(It.IsAny<ICodeGenerationProvider>(), It.Is<ICodeGenerationSettings>(x => x.BasePath == TestData.BasePath)), Times.Once);
        }

        [Fact]
        public void Runs_Filtered_CodeGenerators_In_Specified_Assembly()
        {
            // Arrange
            using var sut = new CodeGenerationAssembly(CodeGenerationEngineMock.Object, GetAssemblyName(), TestData.BasePath, true, true, classNameFilter: new[] { typeof(MyGeneratorProvider).FullName! });

            // Act
            _ = sut.Generate();

            // Assert
            CodeGenerationEngineMock.Verify(x => x.Generate(It.IsAny<ICodeGenerationProvider>(), It.Is<ICodeGenerationSettings>(x => x.BasePath == TestData.BasePath)), Times.Once);
        }

        [Fact]
        public void Runs_Filtered_CodeGenerators_In_Specified_Assembly_No_Matches()
        {
            // Arrange
            using var sut = new CodeGenerationAssembly(CodeGenerationEngineMock.Object, GetAssemblyName(), TestData.BasePath, true, true, classNameFilter: new[] { "WrongName" });

            // Act
            _ = sut.Generate();

            // Assert
            CodeGenerationEngineMock.Verify(x => x.Generate(It.IsAny<ICodeGenerationProvider>(), It.Is<ICodeGenerationSettings>(x => x.BasePath == TestData.BasePath)), Times.Never);
        }
    }
}
