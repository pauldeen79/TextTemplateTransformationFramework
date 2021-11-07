using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests
{
    [ExcludeFromCodeCoverage]
    public class CompilerErrorTests
    {
        [Fact]
        public void CanSerializeCompilerError()
        {
            // Arrange
            var sut = new CompilerError
            (
                2,
                "ERR1",
                "Something went wrong",
                "file.cs",
                false,
                1
            );

            // Act
            var actual = sut.ToString();

            // Assert
            actual.Should().Be("file.cs(1,2): error ERR1: Something went wrong");
        }
    }
}
