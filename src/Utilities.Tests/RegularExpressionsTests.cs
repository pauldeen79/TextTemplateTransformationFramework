using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Utilities.Tests
{
    [ExcludeFromCodeCoverage]
    public class RegularExpressionsTests
    {
        private const string Value1 = "exceptionhandling:customerrorstext and so on and so on";
        private const string Value2 = "exceptionhandling:customerrorstext";
        private const string Value3 = "exceptionhandling:customerrors";
        private const string Value4 = "some prefix exceptionhandling:customerrors";
        private const string Value5 = "some prefix exceptionhandling:customerrors and so on and so on";

        [Theory, InlineData(Value1), InlineData(Value2), InlineData(Value3), InlineData(Value4), InlineData(Value5)]
        public void String_Contains_Plain(string value)
        {
            // Act
            var actual = value?.Contains("exceptionhandling:customerrors") == true;

            // Assert
            actual.Should().BeTrue();
        }

        [Theory, InlineData(Value1, false), InlineData(Value2, false), InlineData(Value3, true), InlineData(Value4, true), InlineData(Value5, false)]
        public void String_EndsWith_Plain(string value, bool expectedResult)
        {
            // Act
            var actual = value?.EndsWith("exceptionhandling:customerrors") == true;

            // Assert
            actual.Should().Be(expectedResult);
        }

        [Theory, InlineData(Value1), InlineData(Value2), InlineData(Value3), InlineData(Value4), InlineData(Value5)]
        public void String_Contains_Regex(string value)
        {
            // Act
            var actual = Regex.IsMatch(value, "exceptionhandling:customerrors", RegexOptions.IgnoreCase);

            // Assert
            actual.Should().BeTrue();
        }

        [Theory, InlineData(Value1, false), InlineData(Value2, false), InlineData(Value3, true), InlineData(Value4, true), InlineData(Value5, false)]
        public void String_EndsWith_Regex(string value, bool expectedResult)
        {
            // Act
            var actual = Regex.IsMatch(value, "exceptionhandling:customerrors$", RegexOptions.IgnoreCase);

            // Assert
            actual.Should().Be(expectedResult);
        }

        [Theory,
            InlineData("", false, ""),
            InlineData("    Assert.Equal(a, b); //comment", true, "    b.Should().Be(a); //comment")]
        public void Replace_Assert_Equals_With_Should_Be(string input, bool expectedIsMatch, string expectedReplacement)
        {
            // Arrange
            const string RegExExpression = @"(.*)Assert\.Equal\((.*)\,(.*)\)(.*)";
            var regEx = new Regex(RegExExpression, RegexOptions.Compiled);

            // Act
            var isMatch = regEx.IsMatch(input);

            // Assert
            isMatch.Should().Be(expectedIsMatch);
            if (isMatch)
            {
                var matchesCollection = regEx.Match(input);
                // 0 is the entire group    -> "<prefix>Assert.Equal(<a>, <b>)<suffix>"
                // 1 is the prefix          -> "    "
                // 2 is the first argument  -> "a"
                // 3 is the second argument -> " b"
                // 4 is the suffix          -> ; //comment
                var replacement = $"{matchesCollection.Groups[1].Value}{matchesCollection.Groups[3].Value.TrimStart()}.Should().Be({matchesCollection.Groups[2].Value.TrimStart()}){matchesCollection.Groups[4].Value}";
                replacement.Should().Be(expectedReplacement);
            }
        }

        [Fact]
        public void Replace_Assert_IsType_With_Should_BeOfType()
        {
            // Arrange
            const string Input = "Assert.IsType<ComponentContainerTests>(resolved);";
            const string RegExExpression = @"(.*)Assert\.IsType<(.*)>\((.*)\)(.*)";
            var regEx = new Regex(RegExExpression, RegexOptions.Compiled);

            // Act
            var isMatch = regEx.IsMatch(Input);

            // Assert
            isMatch.Should().BeTrue();
            var matchesCollection = regEx.Match(Input);
            var replacement = $"{matchesCollection.Groups[1].Value}{matchesCollection.Groups[3].Value}.Should().BeOfType<{matchesCollection.Groups[2].Value}>(){matchesCollection.Groups[4].Value}";
            replacement.Should().Be("resolved.Should().BeOfType<ComponentContainerTests>();");
        }

        [Fact]
        public void Replace_Assert_NotNull_With_Should_NotBeNull()
        {
            // Arrange
            const string Input = "Assert.NotNull(x);";
            const string RegExExpression = @"(.*)Assert\.NotNull\((.*)\)(.*)";
            var regEx = new Regex(RegExExpression, RegexOptions.Compiled);

            // Act
            var isMatch = regEx.IsMatch(Input);

            // Assert
            isMatch.Should().BeTrue();
            var matchesCollection = regEx.Match(Input);
            var replacement = $"{matchesCollection.Groups[1].Value}{matchesCollection.Groups[2].Value}.Should().NotBeNull(){matchesCollection.Groups[3].Value}";
            replacement.Should().Be("x.Should().NotBeNull();");
        }

        [Fact]
        public void Replace_Assert_Null_With_Should_BeNull()
        {
            // Arrange
            const string Input = "Assert.Null(x);";
            const string RegExExpression = @"(.*)Assert\.Null\((.*)\)(.*)";
            var regEx = new Regex(RegExExpression, RegexOptions.Compiled);

            // Act
            var isMatch = regEx.IsMatch(Input);

            // Assert
            isMatch.Should().BeTrue();
            var matchesCollection = regEx.Match(Input);
            var replacement = $"{matchesCollection.Groups[1].Value}{matchesCollection.Groups[2].Value}.Should().BeNull(){matchesCollection.Groups[3].Value}";
            replacement.Should().Be("x.Should().BeNull();");
        }

        [Fact]
        public void Replace_Assert_Empty_With_Should_BeEmpty()
        {
            // Arrange
            const string Input = "Assert.Empty(x);";
            const string RegExExpression = @"(.*)Assert\.Empty\((.*)\)(.*)";
            var regEx = new Regex(RegExExpression, RegexOptions.Compiled);

            // Act
            var isMatch = regEx.IsMatch(Input);

            // Assert
            isMatch.Should().BeTrue();
            var matchesCollection = regEx.Match(Input);
            var replacement = $"{matchesCollection.Groups[1].Value}{matchesCollection.Groups[2].Value}.Should().BeEmpty(){matchesCollection.Groups[3].Value}";
            replacement.Should().Be("x.Should().BeEmpty();");
        }

        [Fact]
        public void Replace_Assert_Contains_String_With_Should_Contain()
        {
            // Arrange
            const string Input = @"Assert.Contains(""a"", b);";
            const string RegExExpression = @"(.*)Assert\.Contains\(""(.*)"",(.*)\)(.*)";
            var regEx = new Regex(RegExExpression, RegexOptions.Compiled);

            // Act
            var isMatch = regEx.IsMatch(Input);

            // Assert
            isMatch.Should().BeTrue();
            var matchesCollection = regEx.Match(Input);
            var replacement = $"{matchesCollection.Groups[1].Value}{matchesCollection.Groups[3].Value.TrimStart()}.Should().Contain(\"{matchesCollection.Groups[2].Value.TrimStart()}\"){matchesCollection.Groups[4].Value}";
            replacement.Should().Be(@"b.Should().Contain(""a"");");
        }

        [Fact]
        public void Replace_Assert_DoesNotContain_String_With_Should_NotContain()
        {
            // Arrange
            const string Input = @"Assert.DoesNotContain(""a"", b);";
            const string RegExExpression = @"(.*)Assert\.DoesNotContain\(""(.*)"",(.*)\)(.*)";
            var regEx = new Regex(RegExExpression, RegexOptions.Compiled);

            // Act
            var isMatch = regEx.IsMatch(Input);

            // Assert
            isMatch.Should().BeTrue();
            var matchesCollection = regEx.Match(Input);
            var replacement = $"{matchesCollection.Groups[1].Value}{matchesCollection.Groups[3].Value.TrimStart()}.Should().NotContain(\"{matchesCollection.Groups[2].Value.TrimStart()}\"){matchesCollection.Groups[4].Value}";
            replacement.Should().Be(@"b.Should().NotContain(""a"");");
        }

        [Fact]
        public void Replace_Assert_StartsWith_String_With_Should_StartWith()
        {
            // Arrange
            const string Input = @"Assert.StartsWith(""a"", b);";
            const string RegExExpression = @"(.*)Assert\.StartsWith\(""(.*)"",(.*)\)(.*)";
            var regEx = new Regex(RegExExpression, RegexOptions.Compiled);

            // Act
            var isMatch = regEx.IsMatch(Input);

            // Assert
            isMatch.Should().BeTrue();
            var matchesCollection = regEx.Match(Input);
            var replacement = $"{matchesCollection.Groups[1].Value}{matchesCollection.Groups[3].Value.TrimStart()}.Should().StartWith(\"{matchesCollection.Groups[2].Value.TrimStart()}\"){matchesCollection.Groups[4].Value}";
            replacement.Should().Be(@"b.Should().StartWith(""a"");");
        }
    }
}
