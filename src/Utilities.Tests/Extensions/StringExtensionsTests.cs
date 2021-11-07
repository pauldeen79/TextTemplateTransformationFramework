using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Utilities.Extensions;
using Xunit;

namespace Utilities.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class StringExtensionsTests
    {
        [Theory,
            InlineData("foo", "foo"),
            InlineData("some text", "some_text"),
            InlineData("1test", "_1test")]
        public void Sanitize_Returns_Correct_Result(string input, string expectedOutput)
        {
            // Act
            var result = input.Sanitize();

            // Assert
            result.Should().Be(expectedOutput);
        }

        [Theory,
            InlineData("System.String", "System.String"),
            InlineData("TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens.ClearErrorsToken`1[[TextTemplateTransformationFramework.T4.TokenParserState, TextTemplateTransformationFramework.T4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]", "TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens.ClearErrorsToken"),
            InlineData("TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens.ClearErrorsToken`1", "TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens.ClearErrorsToken")]
        public void WithoutGenerics_Returns_Correct_Value(string input, string expectedOutput)
        {
            // Act
            var actual = input.WithoutGenerics();

            // Assert
            actual.Should().Be(expectedOutput);
        }
    }
}
