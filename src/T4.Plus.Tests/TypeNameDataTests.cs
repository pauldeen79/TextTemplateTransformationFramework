using System.Linq;
using FluentAssertions;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Plus.Tests
{
    public class TypeNameDataTests
    {
        [Fact]
        public void Should_Construct_With_Default_Values()
        {
            this.Invoking(_ => new TypeNameData()).Should().NotThrow();
        }

        [Fact]
        public void Should_Construct_With_Non_Default_Values()
        {
            this.Invoking(_ => new TypeNameData("", "", "")).Should().NotThrow();
        }

        [Fact]
        public void Should_Be_Able_To_Read_All_Properties()
        {
            // Arrange
            var sut = new TypeNameData();

            // Act & Assert
            sut.GetType().GetProperties().ToList().ForEach(x => x.Invoking(x => x.GetValue(sut)).Should().NotThrow());
        }
    }
}
