using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Utilities.GridOutput.Tests.TestHelpers;
using Xunit;

namespace Utilities.GridOutput.Tests
{
    [ExcludeFromCodeCoverage]
    public class GridOutputTests
    {
        [Fact]
        public void Create_Creates_String_From_Sequence()
        {
            // Arrange
            var input = new[]
            {
                new MyClass { StringProperty = "Hello world!", IntProperty = 1 }
            };

            // Act
            var actual = GridOutput.Create(input, CultureInfo.InvariantCulture);

            // Assert
            actual.Should().NotBeNull();
            actual.ToString().Should().Be(@"IntProperty	StringProperty	DateTimeProperty	NullableDateTimeProperty	NullableIntProperty	EnumProperty	NullableEnumProperty
1	Hello world!	01/01/0001 00:00:00	[[NULL]]	[[NULL]]	A	[[NULL]]
");
        }

        [Fact]
        public void FromString_Creates_GridOutput_From_String()
        {
            // Arrange
            const string Input = @"IntProperty	StringProperty	DateTimeProperty	NullableDateTimeProperty	NullableIntProperty	EnumProperty	NullableEnumProperty
1	Hello world!	01/01/0001 00:00:00	[[NULL]]	[[NULL]]	A	[[NULL]]
";

            // Act
            var actual = GridOutput.FromString(Input);

            // Assert
            actual.Should().NotBeNull();
            actual.ColumnNames.Count().Should().Be(typeof(MyClass).GetProperties().Length);
            actual.Data.Count().Should().Be(1);
            actual.Data.ElementAt(0).Cells.Count().Should().Be(7);
            using (new AssertionScope())
            {
                actual.Data.ElementAt(0).Cells.ElementAt(0).Value.Should().Be("1");
                actual.Data.ElementAt(0).Cells.ElementAt(1).Value.Should().Be("Hello world!");
                actual.Data.ElementAt(0).Cells.ElementAt(2).Value.Should().Be("01/01/0001 00:00:00");
                actual.Data.ElementAt(0).Cells.ElementAt(3).Value.Should().BeNull();
                actual.Data.ElementAt(0).Cells.ElementAt(4).Value.Should().BeNull();
                actual.Data.ElementAt(0).Cells.ElementAt(5).Value.Should().Be("A");
                actual.Data.ElementAt(0).Cells.ElementAt(6).Value.Should().BeNull();
            }
        }
    }
}
