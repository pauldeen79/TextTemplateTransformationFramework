using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using TextTemplateTransformationFramework.Common.Extensions;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class DictionaryOfStringObjectExtensionsTests
    {
        [Fact]
        public void GetValue_Without_DefaultValue_Returns_Null_When_Key_Is_Not_Found()
        {
            // Arrange
            var sut = new Dictionary<string, object>();
            sut.Add("Key", "Value");

            // Act
            var actual = sut.GetValue<string>("WrongKey");

            // Assert
            actual.Should().BeNull();
        }

        [Fact]
        public void GetValue_Without_DefaultValue_Returns_Value_When_Key_Is_Found()
        {
            // Arrange
            var sut = new Dictionary<string, object>();
            sut.Add("Key", "Value");

            // Act
            var actual = sut.GetValue<string>("Key");

            // Assert
            actual.Should().Be("Value");
        }

        [Fact]
        public void GetValue_With_DefaultValue_Returns_DefaultValue_When_Key_Is_Not_Found()
        {
            // Arrange
            var sut = new Dictionary<string, object>();
            sut.Add("Key", "Value");

            // Act
            var actual = sut.GetValue<string>("WrongKey", "DefaultValue");

            // Assert
            actual.Should().Be("DefaultValue");
        }

        [Fact]
        public void GetValue_With_DefaultValue_Returns_Value_When_Key_Is_Found()
        {
            // Arrange
            var sut = new Dictionary<string, object>();
            sut.Add("Key", "Value");

            // Act
            var actual = sut.GetValue<string>("Key", "DefaultValue");

            // Assert
            actual.Should().Be("Value");
        }
    }
}
