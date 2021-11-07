using System;
using System.Diagnostics.CodeAnalysis;

namespace Utilities.GridOutput.Tests.TestHelpers
{
    [ExcludeFromCodeCoverage]
    public class MyClass
    {
        public int IntProperty { get; set; }
        public string StringProperty { get; set; }
        public DateTime DateTimeProperty { get; set; }
        public DateTime? NullableDateTimeProperty { get; set; }
        public int? NullableIntProperty { get; set; }
        public MyEnumeration EnumProperty { get; set; }
        public MyEnumeration? NullableEnumProperty { get; set; }
    }
}
