using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using TextTemplateTransformationFramework.Common.Data;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.Data
{
    [ExcludeFromCodeCoverage]
    public class SectionProcessResultDataTests
    {
        [Fact]
        public void Validate_Throws_On_Null_Context()
        {
            // Arrange
            var sut = CreateSut();
            sut.Context = null;
            var action = new Action(() => sut.Validate());

            // Act & Assert
            action.Should().Throw<InvalidOperationException>().WithMessage("Context is required");
        }

        [Fact]
        public void Validate_Throws_On_Null_Model()
        {
            // Arrange
            var sut = CreateSut();
            sut.Model = null;
            var action = new Action(() => sut.Validate());

            // Act & Assert
            action.Should().Throw<InvalidOperationException>().WithMessage("Model is required");
        }

        [Fact]
        public void Validate_Throws_On_Null_IsValidDelegate()
        {
            // Arrange
            var sut = CreateSut();
            sut.IsValidDelegate = null;
            var action = new Action(() => sut.Validate());

            // Act & Assert
            action.Should().Throw<InvalidOperationException>().WithMessage("IsValidDelegate is required");
        }

        [Fact]
        public void Validate_Throws_On_Null_MapDelegate()
        {
            // Arrange
            var sut = CreateSut();
            sut.MapDelegate = null;
            var action = new Action(() => sut.Validate());

            // Act & Assert
            action.Should().Throw<InvalidOperationException>().WithMessage("MapDelegate is required");
        }

        private SectionProcessResultData<SectionProcessResultDataTests> CreateSut()
            => new SectionProcessResultData<SectionProcessResultDataTests>()
                .WithContext(SectionContext<SectionProcessResultDataTests>.Empty)
                .WithDirectiveName("Test")
                .WithExistingResult(null)
                .WithIsValidDelegate((_, _) => true)
                .WithMapDelegate(() => new object())
                .WithModel(new object())
                .WithPassThrough(false)
                .WithSwitchToMode(1000)
                .WithTokensAreForRootTemplateSection(false);
    }
}
