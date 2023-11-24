using System;
using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Tests
{
    [ExcludeFromCodeCoverage]
    public class TokenStateProcessorTests : TestBase
    {
        [Fact]
        public void Ctor_Throws_On_Null_Arguments()
        {
            ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(typeof(TokenStateProcessor));
        }

        [Fact]
        public void Process_Throws_On_Null_State()
        {
            // Arrange
            var sut = Fixture.Create<TokenStateProcessor>();

            // Act
            sut.Invoking(x => x.Process(null, null, null, null))
               .Should().Throw<ArgumentNullException>().WithParameterName("state");
        }
    }
}
