using System;
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

        [Theory,
            InlineData("true", true),
            InlineData("TRUE", true),
            InlineData("1", true),
            InlineData("y", true),
            InlineData("t", true),
            InlineData("yes", true),
            InlineData("false", false),
            InlineData("FALSE", false),
            InlineData("", false),
            InlineData(null, false)]
        public void IsTrue_Returns_Correct_Result(string input, bool expectedResult)
        {
            // Act
            var result = input.IsTrue();

            // Assert
            result.Should().Be(expectedResult);
        }

        [Theory,
            InlineData("true", false),
            InlineData("TRUE", false),
            InlineData("false", true),
            InlineData("FALSE", true),
            InlineData("0", true),
            InlineData("n", true),
            InlineData("f", true),
            InlineData("no", true),
            InlineData("", false),
            InlineData(null, false)]
        public void IsFalse_Returns_Correct_Result(string input, bool expectedResult)
        {
            // Act
            var result = input.IsFalse();

            // Assert
            result.Should().Be(expectedResult);
        }

        [Theory,
            InlineData("some magic string", new[] { "", "blah blah", "some magic string" }, true),
            InlineData("some wrong string", new[] { "", "blah blah", "some magic string" }, false)]
        public void In_Returns_Correct_Result(string input, string[] values, bool expectedResult)
        {
            // Act
            var result = input.In(StringComparison.OrdinalIgnoreCase, values);

            // Assert
            result.Should().Be(expectedResult);
        }
    }
}
