using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using TextTemplateTransformationFramework.Common.Default;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.Default
{
    [ExcludeFromCodeCoverage]
    public class TemplateInitializeParameterSetterTests
    {
        [Fact]
        public void Set_Throws_On_Null_Context()
        {
            // Arrange
            var sut = new TemplateInitializeParameterSetter<TemplateInitializeParameterSetterTests>();
            var action = new Action(() => sut.Set(null));

            // Act & Assert
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
