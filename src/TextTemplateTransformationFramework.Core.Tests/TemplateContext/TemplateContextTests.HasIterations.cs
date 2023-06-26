﻿namespace TextTemplateTransformationFramework.Core.Tests;

public partial class TemplateContextTests
{
    public class HasIterations : TemplateContextTests
    {
        [Theory,
            InlineData(null, null, false),
            InlineData(0, null, false),
            InlineData(null, 1, false),
            InlineData(0, 1, true)]
        public void Returns_Correct_Result(int? iterationNumber, int? iterationCount, bool expectedResult)
        {
            // Arrange
            var sut = new TemplateContext(this, iterationNumber: iterationNumber, iterationCount: iterationCount);

            // Act
            var result = sut.HasIterations;

            // Assert
            result.Should().Be(expectedResult);
        }
    }
}
