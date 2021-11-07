using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using FluentAssertions;
using Utilities.Extensions;
using Xunit;

namespace Utilities.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class ObjectExtensionsTests
    {
        [Fact]
        public void Then_Executes_Action()
        {
            var input = "Hello world";
            var executed = false;
            var actual = input.Then(_ => executed = true);

            actual.Should().Be("Hello world");
            executed.Should().BeTrue();
        }

        [Fact]
        public void With_Executes_Function()
        {
            var input = "Hello world";
            var actual = input.With(x => x.ToUpper(CultureInfo.InvariantCulture));

            actual.Should().Be("HELLO WORLD");
        }
    }
}
