using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using TextTemplateTransformationFramework.Common.Cmd.Default;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Cmd.Tests.Default
{
    [ExcludeFromCodeCoverage]
    public class UserInputTests
    {
        [Fact]        
        public void UserInput_Throws_On_Null_Argument()
        {
            // Arrange
            var sut = new UserInput();

            // Act & Assert
            sut.Invoking(x => x.GetValue(null)).Should().Throw<ArgumentNullException>();
        }
    }
}
