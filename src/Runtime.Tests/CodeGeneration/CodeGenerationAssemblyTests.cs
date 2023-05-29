using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using TextTemplateTransformationFramework.Runtime.CodeGeneration;
using Xunit;

namespace TextTemplateTransformationFramework.Runtime.Tests.CodeGeneration
{
    public class CodeGenerationAssemblyTests
    {
        [Fact]
        public void Generate_Throws_On_Null_AssemblyName()
        {
            // Act & Assert
            this.Invoking(_ => new CodeGenerationAssembly(null, @"C:\", true, true))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Generate_Runs_All_CodeGenerators_In_Specified_Assembly()
        {
            // Arrange
            using var sut = new CodeGenerationAssembly(GetType().Assembly.FullName, @"C:\", true, true);

            // Act
            var actual = sut.Generate();

            // Assert
            actual.Should().Be(@"<?xml version=""1.0"" encoding=""utf-16""?>
<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TextTemplateTransformationFramework"">
  <BasePath>C:\</BasePath>
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
        public void Generate_Runs_Filtered_CodeGenerators_In_Specified_Assembly()
        {
            // Arrange
            using var sut = new CodeGenerationAssembly(GetType().Assembly.FullName, @"C:\", true, true, classNameFilter: new[] { typeof(MyGeneratorProvider).FullName });

            // Act
            var actual = sut.Generate();

            // Assert
            actual.Should().Be(@"<?xml version=""1.0"" encoding=""utf-16""?>
<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TextTemplateTransformationFramework"">
  <BasePath>C:\</BasePath>
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
    }

    [ExcludeFromCodeCoverage]
    public class MyGeneratorProvider : ICodeGenerationProvider
    {
        public bool GenerateMultipleFiles { get; private set; }

        public bool SkipWhenFileExists { get; private set; }

        public string BasePath { get; private set; }

        public string Path { get; }

        public string DefaultFileName => "MyFile.txt";

        public bool RecurseOnDeleteGeneratedFiles { get; }

        public string LastGeneratedFilesFileName { get; }

        public Action AdditionalActionDelegate { get; }

        public object CreateAdditionalParameters() => null;

        public object CreateGenerator() => new MyGenerator();

        public object CreateModel() => null;

        public void Initialize(bool generateMultipleFiles, bool skipWhenFileExists, string basePath)
        {
            GenerateMultipleFiles = generateMultipleFiles;
            SkipWhenFileExists = skipWhenFileExists;
            BasePath = basePath;
        }
    }

    [ExcludeFromCodeCoverage]
    public class MyGenerator
    {
        public override string ToString() => "Here is code from MyGenerator";
    }
}
