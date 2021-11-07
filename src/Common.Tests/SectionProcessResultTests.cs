using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.InitializeTokens;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests
{
    [ExcludeFromCodeCoverage]
    public class SectionProcessResultTests
    {
        [Fact]
        public void CreateWithMapper_Works_With_Single_TemplateToken()
        {
            // Arrange & Act
            var actual = SectionProcessResult.Create(SectionContext<SectionProcessResultTests>.Empty, new object(), (ctx, m) => true, () => new NamespaceImportToken<SectionProcessResultTests>(SectionContext<SectionProcessResultTests>.Empty, "test"));

            // Assert
            actual.Should().NotBeNull();
        }

        [Fact]
        public void CreateWithMapper_Works_With_IEnumerable_Of_TemplateToken()
        {
            // Arrange & Act
            var actual = SectionProcessResult.Create(SectionContext<SectionProcessResultTests>.Empty, new object(), (ctx, m) => true, () => new ITemplateToken<SectionProcessResultTests>[] { new NamespaceImportToken<SectionProcessResultTests>(SectionContext<SectionProcessResultTests>.Empty, "test") });

            // Assert
            actual.Should().NotBeNull();
        }

        [Fact]
        public void CreateWithMapper_Throws_On_Unknown_Type()
        {
            // Arrange
            var sut = SectionContext<SectionProcessResultTests>.Empty;

            // Act & Assert
            sut.Invoking(x => SectionProcessResult.Create(x, new object(), (ctx, m) => true, () => new object()))
               .Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void CreateWithMapper_NotValid_Uses_ExistingResult_When_Available()
        {
            // Arrange
            var existingResult = SectionProcessResult.Create(new InitializeErrorToken<SectionProcessResultTests>(SectionContext<SectionProcessResultTests>.Empty, "Existing"));

            // Act
            var actual = SectionProcessResult.Create(SectionContext<SectionProcessResultTests>.Empty, new object(), (ctx, m) => false, () => new object(), existingResult: existingResult);

            // Assert
            actual.Should().BeSameAs(existingResult);
        }

        [Fact]
        public void CreateWithMapper_NotValid_Returns_PassThrough_When_ExistingResult_Is_Null_And_PassThrough_Is_True()
        {
            // Act
            var actual = SectionProcessResult.Create(SectionContext<SectionProcessResultTests>.Empty, new object(), (ctx, m) => false, () => new object(), null, true);

            // Assert
            actual.PassThrough.Should().BeTrue();
        }

        [Fact]
        public void CreateWithMapper_NotValid_Returns_NonPassThrough_When_ExistingResult_Is_Null_And_PassThrough_Is_False()
        {
            // Act
            var actual = SectionProcessResult.Create(SectionContext<SectionProcessResultTests>.Empty, new object(), (ctx, m) => false, () => new object(), null, false);

            // Assert
            actual.PassThrough.Should().BeFalse();
        }
    }
}
