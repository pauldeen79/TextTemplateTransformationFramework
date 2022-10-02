using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FluentAssertions;
using Moq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.RenderTokens;
using TextTemplateTransformationFramework.Runtime;
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
            var result = GenerateCode.For<T4PlusCSharp>(new CodeGenerationSettings(null, true));

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void Can_Save_Generated_Code()
        {
            // Arrange
            var multipleContentBuilderMock = new Mock<IMultipleContentBuilder>();
            multipleContentBuilderMock.Setup(x => x.AddContent(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<StringBuilder>())).Returns(new Content());

            // Act
            GenerateCode.For<T4PlusCSharp>(new CodeGenerationSettings(@"C:\", false), multipleContentBuilderMock.Object);

            // Assert
            multipleContentBuilderMock.Verify(x => x.DeleteLastGeneratedFiles(@"/Generated.cs", false), Times.Once);
            multipleContentBuilderMock.Verify(x => x.SaveLastGeneratedFiles(@"/Generated.cs"), Times.Once);
            multipleContentBuilderMock.Verify(x => x.SaveAll(), Times.Once);
        }

        [ExcludeFromCodeCoverage]
        private class T4PlusCSharp : ICodeGenerationProvider
        {
            public bool GenerateMultipleFiles { get; private set; }

            public bool SkipWhenFileExists { get; private set; }

            public string BasePath { get; private set; }

            public string Path => string.Empty;

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

            public void Initialize(bool generateMultipleFiles, bool skipWhenFileExists, string basePath)
            {
                GenerateMultipleFiles = generateMultipleFiles;
                SkipWhenFileExists = skipWhenFileExists;
                BasePath = basePath;
            }
        }
    }
}
