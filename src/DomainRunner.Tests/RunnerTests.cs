using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Xunit;

namespace DomainRunner.Tests
{
    [ExcludeFromCodeCoverage]
    public class RunnerTests
    {
        [Fact]
        public void Can_Run_Piece_Of_Code_In_Separate_AppDomain()
        {
            // Arrange
            var dlg = new Func<string>(() => "1234");
            
            // Act
            var result = Domain.Runner.RunInAppDomain(dlg);

            // Assert
            result.Should().Be("1234");
        }

        [Fact]
        public void Can_Run_Piece_Of_Code_With_Argument_In_Separate_AppDomain()
        {
            // Arrange
            var dlg = new Func<string, string>(input => input.ToUpperInvariant());

            // Act
            var result = Domain.Runner.RunInAppDomain(dlg, "hello world");

            // Assert
            result.Should().Be("HELLO WORLD");
        }

        [Fact]
        public void Delegate_Null_Without_AppDomain_Argument_Throws()
        {
            this.Invoking(_ => Domain.Runner.RunInAppDomain(null))
                .Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("delg");
        }

        [Fact]
        public void AppDomain_Null_Throws()
        {
            this.Invoking(_ => Domain.Runner.RunInAppDomain(null, null, Array.Empty<object>()))
                .Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("targetDomain");
        }
    }
}
