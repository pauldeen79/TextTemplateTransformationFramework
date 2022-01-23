using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.RenderTokens;
using TextTemplateTransformationFramework.Runtime.CodeGeneration;
using TextTemplateTransformationFramework.T4.Plus.CodeGenerators;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.CodeGeneration
{
    [ExcludeFromCodeCoverage]
    public class GenerateCodeTests
    {
        [Fact]
        public void Can_Generate_Code()
        {
            // Act
            var result = GenerateCode.For<T4PlusCSharp>(new CodeGenerationSettings(null, false, true));

            // Assert
            result.Should().NotBeNull();
        }

        [ExcludeFromCodeCoverage]
        private class T4PlusCSharp : ICodeGenerationProvider
        {
            public bool GenerateMultipleFiles { get; private set; }

            public string BasePath { get; private set; }

            public string Path => null;

            public string DefaultFileName => "Generated.cs";

            public bool RecurseOnDeleteGeneratedFiles => false;

            public string LastGeneratedFilesFileName => "Generated.cs";

            public Action AdditionalActionDelegate => null;

            public object CreateAdditionalParameters()
            {
                return null;
            }

            public object CreateGenerator()
            {
                return new T4PlusCSharpCodeGenerator();
            }

            public object CreateModel()
            {
                return new[] { new RenderCodeToken<TokenParserState>(SectionContext.FromCurrentMode(Mode.CodeRender, new TokenParserState()), "// hello world!") };
            }

            public void Initialize(bool generateMultipleFiles, string basePath)
            {
                GenerateMultipleFiles = generateMultipleFiles;
                BasePath = basePath;
            }
        }
    }
}
