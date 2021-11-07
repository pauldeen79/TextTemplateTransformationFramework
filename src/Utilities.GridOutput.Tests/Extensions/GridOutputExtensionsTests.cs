using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using Utilities.GridOutput.Extensions;
using Utilities.GridOutput.Tests.TestHelpers;
using Xunit;

namespace Utilities.GridOutput.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class GridOutputExtensionsTests
    {
        [Fact]
        public void To_T_Creates_Identical_Sequence_From_String_PublicCtor_AutoMapping()
        {
            // Arrange
            var input = new[]
            {
                new MyClass { StringProperty = "Hello world!", IntProperty = 1 },
                new MyClass { StringProperty = null, IntProperty = 1, NullableDateTimeProperty = DateTime.Today },
                new MyClass { EnumProperty = MyEnumeration.C }
            }.ToArray();
            var gridOutput = GridOutput.Create(input, CultureInfo.InvariantCulture);

            // Act
            var actual = gridOutput.To<MyClass>()?.ToArray();

            // Assert
            AssertTypedInstance(input, actual);
        }

        [Fact]
        public void To_T_Creates_Identical_Sequence_From_String_CustomMapping()
        {
            // Arrange
            var input = new[]
            {
                new MyClass { StringProperty = "Hello world!", IntProperty = 1 },
                new MyClass { StringProperty = null, IntProperty = 1, NullableDateTimeProperty = DateTime.Today },
                new MyClass { EnumProperty = MyEnumeration.C }
            }.ToArray();
            var gridOutput = GridOutput.Create(input, CultureInfo.InvariantCulture);

            // Act
            var actual = gridOutput.To(row => ConvertToMyClass(row, gridOutput))?.ToArray();

            // Assert
            AssertTypedInstance(input, actual);
        }

        private MyClass ConvertToMyClass(GridOutputRow row, GridOutput parentInstance)
        {
            var result = new MyClass();
            var items = row.ToDictionary(parentInstance);

            result.DateTimeProperty = items["DateTimeProperty"].ConvertTo<DateTime>();
            result.IntProperty = items["IntProperty"].ConvertTo<int>();
            result.NullableDateTimeProperty = items["NullableDateTimeProperty"].ConvertTo<DateTime?>();
            result.StringProperty = items["StringProperty"].ConvertTo<string>();
            result.EnumProperty = items["EnumProperty"].ConvertTo<MyEnumeration>();
            result.NullableEnumProperty = items["NullableEnumProperty"].ConvertTo<MyEnumeration?>();

            return result;
        }

        private static void AssertTypedInstance(MyClass[] input, MyClass[] actual)
        {
            actual.Should().NotBeNull().And.HaveCount(input.Length);
            foreach (var item in actual.Select((value, index) => new { value, index }))
            {
                item.value.DateTimeProperty.Should().Be(input[item.index].DateTimeProperty);
                item.value.IntProperty.Should().Be(input[item.index].IntProperty);
                item.value.NullableDateTimeProperty.Should().Be(input[item.index].NullableDateTimeProperty);
                item.value.StringProperty.Should().Be(input[item.index].StringProperty);
                item.value.EnumProperty.Should().Be(input[item.index].EnumProperty);
                item.value.NullableEnumProperty.Should().Be(input[item.index].NullableEnumProperty);
            }
        }
    }
}
