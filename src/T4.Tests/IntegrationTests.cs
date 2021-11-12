using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.Core.Extensions;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Tests
{
    [ExcludeFromCodeCoverage]
    public sealed class IntegrationTests : IDisposable
    {
        private readonly ServiceProvider _provider;

        public IntegrationTests()
        {
            var serviceCollection = new ServiceCollection()
                .AddTextTemplateTransformationT4NetCore()
                .AddSingleton<IFileContentsProvider, FileContentsProviderMock>();
            _provider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public void NewLineHandlingTest1()
        {
            // Arrange
            const string Src = @"<#@ template language=""c#"" #>
Hello world!
<# WriteLine(""I'm here""); #>
<#= ""some expression"" #>
<#+ void UnusedMethod() { } #>";
            var sut = _provider.GetRequiredService<ITextTemplateProcessor>();

            // Act
            var actual = sut.Process(new TextTemplate(Src));

            // Assert
            const string expected = @"Hello world!
I'm here
some expression
";
            actual.ToString().Should().Be(expected);
        }

        [Fact]
        public void NewLineHandlingTest2()
        {
            // Arrange
            const string Src = @"<#@ template language=""c#"" #>
<#= ""Hello world!"" #>
New line needed
<#= ""Some more text"" #>";
            var sut = _provider.GetRequiredService<ITextTemplateProcessor>();

            // Act
            var actual = sut.Process(new TextTemplate(Src));

            // Assert
            const string expected = @"Hello world!
New line needed
Some more text";
            actual.ToString().Should().Be(expected);
        }

        [Fact]
        public void NewLineHandlingTest3()
        {
            // Arrange
            const string Src = @"<#@ template language=""c#"" #>
Line 1
<# WriteLine(""Line 2""); #>
Line 3
<# WriteLine(""Line 4""); #>
<# WriteLine(""Line 5""); #>

<# WriteLine(""Line 6""); #>
Some <#= ""Text"" #><# if (true) { #> added <# } #>

Second line
Some <#= ""Text"" #><# if (true) { #> added <# } #>


Third line";
            var sut = _provider.GetRequiredService<ITextTemplateProcessor>();

            // Act
            var actual = sut.Process(new TextTemplate(Src));

            // Assert
            const string expected = @"Line 1
Line 2
Line 3
Line 4
Line 5

Line 6
Some Text added 
Second line
Some Text added 

Third line";
            actual.ToString().Should().Be(expected);
        }

        [Fact]
        public void NewLineHandlingTest4()
        {
            // Arrange
            const string Src = @"<#@ template debug=""false"" hostspecific=""false"" language=""c#"" #>
<#@ assembly name=""System.Core"" #>
<#@ import namespace=""System.Linq"" #>
<#@ import namespace=""System.Text"" #>
<#@ import namespace=""System.Collections.Generic"" #>
<#@ output extension="".txt"" #>
Line 1
<# WriteLine(""Line 2""); #>
Line 3
<#= ""Line 4"" #>
Line 5
<# WriteLine(""Line 6""); #>
<#= ""Line 7"" #>
<# WriteLine(""Line 8""); #>
Line 9
<#= ""Line 10"" #>
<# WriteLine(""Line 11""); #>
<#= ""Line 12"" #>
Line 13
<#= ""Line 14"" #>
<#= ""Line 15"" #>
<# WriteLine(""Line 16""); #>
<# WriteLine(""Line 17""); #>
Line 18
Line 19";

            var sut = _provider.GetRequiredService<ITextTemplateProcessor>();

            // Act
            var actual = sut.Process(new TextTemplate(Src));

            // Assert
            const string expected = @"Line 1
Line 2
Line 3
Line 4
Line 5
Line 6
Line 7
Line 8
Line 9
Line 10
Line 11
Line 12
Line 13
Line 14
Line 15
Line 16
Line 17
Line 18
Line 19";
            actual.ToString().Should().Be(expected);
        }

        [Fact]
        public void NewLineHandlingTest5()
        {
            // Arrange
            const string Src = @"<#= ""Line 7"" #>
<# Write(""Line 8""); #>";
            var sut = _provider.GetRequiredService<ITextTemplateProcessor>();

            // Act
            var actual = sut.Process(new TextTemplate(Src));

            // Assert
            const string expected = @"Line 7
Line 8";
            actual.ToString().Should().Be(expected);
        }

        [Fact]
        public void CanRenderTemplateWithDefaultVisualStudioT4Contents()
        {
            // Arrange
            const string Src = @"<#@ template debug=""false"" hostspecific=""false"" language=""C#"" #>
<#@ assembly name=""System.Core"" #>
<#@ import namespace=""System.Linq"" #>
<#@ import namespace=""System.Text"" #>
<#@ import namespace=""System.Collections.Generic"" #>
<#@ output extension="".txt"" #>
Hello world!";
            var sut = _provider.GetRequiredService<ITextTemplateProcessor>();

            // Act
            var actual = sut.Process(new TextTemplate(Src));

            // Assert
            const string expected = "Hello world!";
            actual.ToString().Should().Be(expected);
        }

        [Fact]
        public void ValidationErrorsArePreservedOnCompilationErrors()
        {
            // Arrange
            const string Src = @"<#@ template language=""c#"" #>
<#@ output #>
<# CompilationErrorGoesHere(); #>";
            var sut = _provider.GetRequiredService<ITextTemplateProcessor>();

            // Act
            var actual = sut.Process(new TextTemplate(Src));

            // Assert
            actual.CompilerErrors.Should().Contain(e => e.ErrorText == "The Extension field is required.");
        }

        [Fact]
        public void CanUseCodeInClassFooter()
        {
            // Arrange
            const string Src = @"<#@ template language=""c#"" #>
Hello <#= Function() #>!
<#+ string Function() { return ""world""; }";
            var sut = _provider.GetRequiredService<ITextTemplateProcessor>();

            // Act
            var actual = sut.Process(new TextTemplate(Src));

            // Assert
            actual.ToString().Should().Be(@"Hello world!
");
        }

        [Fact]
        public void CanUseErrorInClassFooter()
        {
            // Arrange
            const string Src = @"<#@ template language=""c#"" #>
Hello <#= Function() #>!
<#+ string Function() { Error(""Kaboom""); return ""world""; }";
            var sut = _provider.GetRequiredService<ITextTemplateProcessor>();

            // Act
            var actual = sut.Process(new TextTemplate(Src));

            // Assert
            actual.Output.Should().Be("ErrorGeneratingOutput");
            actual.CompilerErrors.Should().HaveCount(1);
            actual.CompilerErrors[0].ErrorText.Should().Be("Kaboom");
            actual.CompilerErrors[0].IsWarning.Should().BeFalse();
        }

        [Fact]
        public void CanUseExpressionInClassFooter()
        {
            // Arrange
            const string Src = @"<#@ template language=""c#"" #>
Hello <#= Function() #>!
<#+ string Function() { var world = ""world"";#><#= world #><#+; return string.Empty; }#>";
            var sut = _provider.GetRequiredService<ITextTemplateProcessor>();

            // Act
            var actual = sut.Process(new TextTemplate(Src));

            // Assert
            actual.ToString().Should().Be(@"Hello world!
");
        }

        [Fact]
        public void CanUseTextInClassFooter()
        {
            // Arrange
            const string Src = @"<#@ template language=""c#"" #>
Hello <#= Function() #>!
<#+ string Function() { #>world<#+ return string.Empty; }";
            var sut = _provider.GetRequiredService<ITextTemplateProcessor>();

            // Act
            var actual = sut.Process(new TextTemplate(Src));

            // Assert
            actual.ToString().Should().Be(@"Hello world!
");
        }

        [Fact]
        public void CanUseWarningInClassFooter()
        {
            // Arrange
            const string Src = @"<#@ template language=""c#"" #>
Hello <#= Function() #>!
<#+ string Function() { Warning(""Kaboom""); return ""world""; }";
            var sut = _provider.GetRequiredService<ITextTemplateProcessor>();

            // Act
            var actual = sut.Process(new TextTemplate(Src));

            // Assert
            actual.Output.Should().Be(@"Hello world!
");
            actual.CompilerErrors.Should().HaveCount(1);
            actual.CompilerErrors[0].ErrorText.Should().Be("Kaboom");
            actual.CompilerErrors[0].IsWarning.Should().BeTrue();
        }

        [Fact]
        public void MultipleIncludesGetFilteredWhenOnceIsSetToTrue()
        {
            // Arrange
            var sut = _provider.GetRequiredService<ITextTemplateProcessor>();
            const string Src = @"<#@ template language=""c#"" #>
Hello <#= ""world"" #><#@ include file=""file.ttinclude"" once=""true"" #><#@ include file=""file.ttinclude"" once=""true"" #>";

            // Act
            var actual = sut.Process(new TextTemplate(Src));

            // Assert
            actual.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void MultipleIncludesDoNotGetFilteredWhenOnceIsSetToFalse()
        {
            // Arrange
            const string Src = @"<#@ template language=""c#"" #>
Hello <#= ""world"" #><#@ include file=""file.ttinclude"" #><#@ include file=""file.ttinclude"" #>";
            var sut = _provider.GetRequiredService<ITextTemplateProcessor>();

            // Act
            var actual = sut.Process(new TextTemplate(Src));

            // Assert
            actual.ToString().Should().Be("Hello world!!");
        }

        [Fact]
        public void NotEndedAttributeGetsIgnored()
        {
            // Arrange
            const string Src = @"<#@ template language=""vb #>
Hello <#= ""world"" #><# Write(""!""); #>";
            var sut = _provider.GetRequiredService<ITextTemplateProcessor>();

            // Act
            var actual = sut.Process(new TextTemplate(Src));

            // Assert
            actual.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void OnlySmallerThanSign_Does_Not_Lead_To_CompilerError()
        {
            // Arrange
            const string Src = "< Hello world";
            var sut = _provider.GetRequiredService<ITextTemplateProcessor>();

            // Act
            var actual = sut.Process(new TextTemplate(Src));

            // Assert
            actual.Output.Should().Be("< Hello world");
        }

        [Fact]
        public void OnlyGreaterThanSign_Does_Not_Lead_To_CompileError()
        {
            // Arrange
            const string Src = "> Hello world";
            var sut = _provider.GetRequiredService<ITextTemplateProcessor>();

            // Act
            var actual = sut.Process(new TextTemplate(Src));

            // Assert
            actual.Output.Should().Be("> Hello world");
        }

        [Fact]
        public void EmptyExpression_Leads_To_CompilerError()
        {
            // Arrange
            const string Src = "<#= #>";
            var sut = _provider.GetRequiredService<ITextTemplateProcessor>();

            // Act
            var actual = sut.Process(new TextTemplate(Src));

            // Assert
            actual.Output.Should().Be("ErrorGeneratingOutput");
            actual.CompilerErrors.Where(e => !e.IsWarning).Should().HaveCount(1);
            actual.CompilerErrors.First(e => !e.IsWarning).ErrorText.Should().Be("There is no argument given that corresponds to the required formal parameter 'objectToConvert' of 'GeneratedClassBase.ToStringInstanceHelper.ToStringWithCulture(object)'");
        }

        [Fact]
        public void CanExtractEnumPropertyWithTypeAssemblyQualifiedName()
        {
            // Arrange
            var type = typeof(UriFormat).AssemblyQualifiedName;
            var src = @"<#@ template language=""c#"" usetemplateruntimebaseclass=""true"" #>
<#@ property DefaultValue=""System.UriFormat.UriEscaped"" Name=""UriFormat"" type=""" + type + @""" #>
<#@ property Name=""Test"" type=""System.String"" #>";
            var sut = _provider.GetRequiredService<ITextTemplateProcessor>();

            // Act
            var actual = sut.ExtractParameters(new TextTemplate(src));

            // Assert
            actual.Parameters.Should().HaveCount(2);
        }

        [Fact]
        public void CanExtractEnumPropertyWithTypeFullName()
        {
            // Arrange
            const string Src = @"<#@ template language=""c#"" usetemplateruntimebaseclass=""true"" #>
<#@ property DefaultValue=""System.UriFormat.UriEscaped"" Name=""UriFormat"" type=""System.UriFormat"" #>
<#@ property Name=""Test"" type=""System.String"" #>";
            var sut = _provider.GetRequiredService<ITextTemplateProcessor>();

            // Act
            var actual = sut.ExtractParameters(new TextTemplate(Src));

            // Assert
            actual.Parameters.Should().HaveCount(2);
        }

        public interface IMyInterface
        {
            void DoSomething();
        }

        [ExcludeFromCodeCoverage]
        public class Impl1 : IMyInterface
        {
            public void DoSomething()
            {
                // Method left empty intentionally.
            }
        }

        [ExcludeFromCodeCoverage]
        public class Impl2 : IMyInterface
        {
            public void DoSomething()
            {
                // Method left empty intentionally.
            }
        }

        public void Dispose()
            => _provider.Dispose();

        [DirectivePrefix("SomeDirectiveThatGetsProcessedByUnknownTemplateTokenProcessor")]
        [ExcludeFromCodeCoverage]
        private class UnknownTemplateTokenProcessor : ITemplateSectionProcessor<TokenParserState>
        {
            public SectionProcessResult<TokenParserState> Process(SectionContext<TokenParserState> context)
            {
                return SectionProcessResult.Create(new UnknownTemplateToken { FileName = context.FileName, LineNumber = context.LineNumber, SectionContext = context });
            }

            private class UnknownTemplateToken : IRenderToken<TokenParserState>
            {
                public SectionContext<TokenParserState> SectionContext { get; set; }

                public int LineNumber { get; set; }

                public string FileName { get; set; }
            }
        }

        [ExcludeFromCodeCoverage]
        private class FileContentsProviderMock : IFileContentsProvider
        {
            public bool FileExists(string fileName)
            {
                throw new NotImplementedException();
            }

            public string GetFileContents(string fileName)
            {
                return @"<# Write(""!""); #>";
            }

            public void WriteFileContents(string path, string contents)
            {
                throw new NotImplementedException();
            }
        }
    }

    [ExcludeFromCodeCoverage]
    public class CustomBaseClass
    {
        protected void Write(object value)
        {
            GenerationEnvironment.Append(ToStringHelper.ToStringWithCulture(value));
        }

        private Runtime.T4GeneratedTemplateBase.ToStringInstanceHelper _helper;

        protected Runtime.T4GeneratedTemplateBase.ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return _helper ?? (_helper = new Runtime.T4GeneratedTemplateBase.ToStringInstanceHelper());
            }
            set
            {
                _helper = value;
            }
        }

        private StringBuilder _generationEnvironment;

        protected StringBuilder GenerationEnvironment
        {
            get
            {
                return _generationEnvironment ?? (_generationEnvironment = new StringBuilder());
            }
            set
            {
                _generationEnvironment = value;
            }
        }
    }
}
