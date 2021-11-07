using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Utilities.GridOutput.Extensions;
using Xunit;

namespace Utilities.GridOutput.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class GridOutputRowExtensionsTests
    {
        [Fact]
        public void ToKeyValuePairs_Returns_Correct_Value()
        {
            // Arrange
            var row = new GridOutputRow
            {
                Cells = new[]
                {
                    new GridOutputCell { Value = "Value1" },
                    new GridOutputCell { Value = "Value2" },
                    new GridOutputCell { Value = "Value3" }
                }
            };
            var input = new GridOutput
            {
                ColumnNames = new[] { "Field1", "Field2", "Field3" },
                Data = new[] { row }
            };

            // Act
            var actual = row.ToKeyValuePairs(input)?.ToArray();

            // Assert
            actual.Should().NotBeNull().And.HaveCount(3);
            if (actual != null)
            {
                actual[0].Key.Should().Be(input.ColumnNames.ElementAt(0));
                actual[1].Key.Should().Be(input.ColumnNames.ElementAt(1));
                actual[2].Key.Should().Be(input.ColumnNames.ElementAt(2));
                actual[0].Value.Should().Be(row.Cells.ElementAt(0).Value);
                actual[1].Value.Should().Be(row.Cells.ElementAt(1).Value);
                actual[2].Value.Should().Be(row.Cells.ElementAt(2).Value);
            }
        }
    }
}
