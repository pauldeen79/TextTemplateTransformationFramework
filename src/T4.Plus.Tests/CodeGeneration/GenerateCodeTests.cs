using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.RenderTokens;
using TextTemplateTransformationFramework.Runtime;
using TextTemplateTransformationFramework.Runtime.CodeGeneration;
using TextTemplateTransformationFramework.T4.Plus.CodeGenerators;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.CodeGeneration
{
    [ExcludeFromCodeCoverage]
    public class GenerateCodeTests : TestBase
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
            var multipleContentBuilderMock = Fixture.Freeze<IMultipleContentBuilder>();
            multipleContentBuilderMock.AddContent(Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<StringBuilder>()).Returns(new Content());

            // Act
            GenerateCode.For<T4PlusCSharp>(new CodeGenerationSettings(@"C:\", false), multipleContentBuilderMock);

            // Assert
            multipleContentBuilderMock.Received(1).DeleteLastGeneratedFiles(@"/Generated.cs", false);
            multipleContentBuilderMock.Received(1).SaveLastGeneratedFiles(@"/Generated.cs");
            multipleContentBuilderMock.Received(1).SaveAll();
        }

        [ExcludeFromCodeCoverage]
        private sealed class T4PlusCSharp : ICodeGenerationProvider
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
