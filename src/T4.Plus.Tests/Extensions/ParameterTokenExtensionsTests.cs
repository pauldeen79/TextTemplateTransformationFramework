using FluentAssertions;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Extensions;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.Extensions
{
    public class ParameterTokenExtensionsTests
    {
        [Fact]
        public void WithPropertySetter_Returns_Instance_With_PropertySetter_Set_To_True()
        {
            // Arrange
            var sut = new ParameterToken<ParameterTokenExtensionsTests>(SectionContext.FromCurrentMode(0, this), "x", "y", addPropertySetter: false);

            // Act
            var actual = sut.WithPropertySetter();

            // Assert
            actual.Should().BeOfType<ParameterToken<ParameterTokenExtensionsTests>>();
            ((ParameterToken<ParameterTokenExtensionsTests>)actual).AddPropertySetter.Should().BeTrue();
        }
    }
}
