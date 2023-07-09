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
            CodeGenerationEngineMock.Verify(x => x.Generate(It.IsAny<ICodeGenerationProvider>(), It.IsAny<ITemplateFileManager>(), It.Is<ICodeGenerationSettings>(x => x.BasePath == TestData.BasePath)), Times.Once);
        }

        [Fact]
        public void Runs_Filtered_CodeGenerators_In_Specified_Assembly()
        {
            // Arrange
            using var sut = new CodeGenerationAssembly(CodeGenerationEngineMock.Object, GetAssemblyName(), TestData.BasePath, true, true, classNameFilter: new[] { typeof(MyGeneratorProvider).FullName! });

            // Act
            _ = sut.Generate();

            // Assert
            CodeGenerationEngineMock.Verify(x => x.Generate(It.IsAny<ICodeGenerationProvider>(), It.IsAny<ITemplateFileManager>(), It.Is<ICodeGenerationSettings>(x => x.BasePath == TestData.BasePath)), Times.Once);
        }

        [Fact]
        public void Runs_Filtered_CodeGenerators_In_Specified_Assembly_No_Matches()
        {
            // Arrange
            using var sut = new CodeGenerationAssembly(CodeGenerationEngineMock.Object, GetAssemblyName(), TestData.BasePath, true, true, classNameFilter: new[] { "WrongName" });

            // Act
            _ = sut.Generate();

            // Assert
            CodeGenerationEngineMock.Verify(x => x.Generate(It.IsAny<ICodeGenerationProvider>(), It.IsAny<ITemplateFileManager>(), It.Is<ICodeGenerationSettings>(x => x.BasePath == TestData.BasePath)), Times.Never);
        }

        [Fact]
        public void Returns_Output()
        {
            // Arrange
            using var sut = new CodeGenerationAssembly(CodeGenerationEngineMock.Object, GetAssemblyName(), TestData.BasePath, true, true);

            // Act
            var result = sut.Generate();

            // Assert
            result.Should().Be(@"<?xml version=""1.0"" encoding=""utf-16""?>
<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TemplateFramework"">
  <BasePath>" + TestData.BasePath + @"</BasePath>
  <Contents />
</MultipleContents>");
        }
    }
}
