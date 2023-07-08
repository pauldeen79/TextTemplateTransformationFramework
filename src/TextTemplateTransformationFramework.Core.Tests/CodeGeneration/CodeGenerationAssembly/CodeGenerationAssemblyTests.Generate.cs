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
            var actual = sut.Generate();

            // Assert
            actual.Should().Be(@"<?xml version=""1.0"" encoding=""utf-16""?>
<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TemplateFramework"">
  <BasePath>" + TestData.BasePath + @"</BasePath>
  <Contents>
    <Contents>
      <FileName>MyFile.txt</FileName>
      <Lines xmlns:d4p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
        <d4p1:string>Here is code from MyGenerator</d4p1:string>
      </Lines>
      <SkipWhenFileExists>false</SkipWhenFileExists>
    </Contents>
  </Contents>
</MultipleContents>");
        }

        [Fact]
        public void Runs_Filtered_CodeGenerators_In_Specified_Assembly()
        {
            // Arrange
            using var sut = new CodeGenerationAssembly(CodeGenerationEngineMock.Object, GetAssemblyName(), TestData.BasePath, true, true, classNameFilter: new[] { typeof(TestData.MyGeneratorProvider).FullName! });

            // Act
            var actual = sut.Generate();

            // Assert
            actual.Should().Be(@"<?xml version=""1.0"" encoding=""utf-16""?>
<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TemplateFramework"">
  <BasePath>" + TestData.BasePath + @"</BasePath>
  <Contents>
    <Contents>
      <FileName>MyFile.txt</FileName>
      <Lines xmlns:d4p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
        <d4p1:string>Here is code from MyGenerator</d4p1:string>
      </Lines>
      <SkipWhenFileExists>false</SkipWhenFileExists>
    </Contents>
  </Contents>
</MultipleContents>");
        }

        [Fact]
        public void Runs_Filtered_CodeGenerators_In_Specified_Assembly_No_Matches()
        {
            // Arrange
            using var sut = new CodeGenerationAssembly(CodeGenerationEngineMock.Object, GetAssemblyName(), TestData.BasePath, true, true, classNameFilter: new[] { "WrongName" });

            // Act
            var actual = sut.Generate();

            // Assert
            actual.Should().Be(@"<?xml version=""1.0"" encoding=""utf-16""?>
<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TemplateFramework"">
  <BasePath>" + TestData.BasePath + @"</BasePath>
  <Contents />
</MultipleContents>");
        }
    }
}
