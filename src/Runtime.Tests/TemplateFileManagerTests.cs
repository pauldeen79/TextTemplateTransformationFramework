using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FluentAssertions;
using Xunit;

namespace TextTemplateTransformationFramework.Runtime.Tests
{
    [ExcludeFromCodeCoverage]
    public class TemplateFileManagerTests
    {
        [Fact]
        public void Ctor_Throws_On_Null_GetStringBuilderDelegate()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateFileManager(_ => { }, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("getStringBuilderDelegate");
        }

        [Fact]
        public void Ctor_Throw_On_Null_SetStringBuilderDelegate()
        {
            // Act & Assert
            this.Invoking(_ => new TemplateFileManager(null, () => new StringBuilder()))
                .Should().Throw<ArgumentNullException>().WithParameterName("setStringBuilderDelegate");
        }

        [Fact]
        public void Process_Adds_All_Contents_To_Original_StringBuilder_When_SilentOutput_Is_False_And_Split_Is_False()
        {
            // Arrange
            var sb = new StringBuilder();
            var sut = new TemplateFileManager(_ => { }, () => sb);
            sut.StartNewFile().Append("Hello world");
            sut.StartNewFile().Append("Hello second file");

            // Act
            sut.Process(false, false);

            // Assert
            sb.ToString().Should().BeEquivalentTo("Hello worldHello second file");
        }
    }
}
