using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using TextTemplateTransformationFramework.Common.Extensions;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class ObjectExtensionsTests
    {
        [Fact]
        public void ConvertValue_Throws_On_Null_Type()
        {
            this.Invoking(x => x.ConvertValue(null)).Should().Throw<ArgumentNullException>().WithParameterName("type");
        }
    }
}
