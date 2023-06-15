using System.Linq;
using FluentAssertions;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.Default.TemplateTokens
{
    public class ParameterTokenTests
    {
        [Fact]
        public void Should_Construct_With_Default_Values()
        {
            this.Invoking(_ => new ParameterToken<ParameterTokenTests>(SectionContext.FromCurrentMode(1, this), "", "")).Should().NotThrow();
        }

        [Fact]
        public void Should_Construct_With_Non_Default_Values()
        {
            this.Invoking(_ => new ParameterToken<ParameterTokenTests>(SectionContext.FromCurrentMode(1, this), "", "", defaultValue: new("", true), componentModelData: new())).Should().NotThrow();
        }

        [Fact]
        public void Should_Be_Able_To_Read_All_Properties()
        {
            // Arrange
            var sut = new ParameterToken<ParameterTokenTests>(SectionContext.FromCurrentMode(1, this), "", "");

            // Act & Assert
            sut.GetType().GetProperties().ToList().ForEach(x => x.Invoking(x => x.GetValue(sut)).Should().NotThrow());
        }
    }
}
