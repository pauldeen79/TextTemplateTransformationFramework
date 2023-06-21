using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Moq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.RenderTokens;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.RenderTokens;
using TextTemplateTransformationFramework.T4.Plus.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Models;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class SectionContextExtensionsTests
    {
        [Fact]
        public void CreateRenderChildTemplateToken_Returns_ErrorToken_When_Mode_Is_Not_Render()
        {
            // Arrange
            var sut = CreateSut(1000 + ModePosition.Initialize); //Note that it's important that we increase the mode by 1000. Else GetCurrentPosition() returns Render anyway :(

            // Act
            var actual = sut.CreateRenderChildTemplateToken(new RenderChildTemplateDirectiveModel());

            // Assert
            actual.Should().BeAssignableTo<IRenderErrorToken<SectionContextExtensionsTests>>();
        }

        [Fact]
        public void CreateRenderChildTemplateToken_Returns_RenderChildTemplateToken_When_Mode_Is_Render()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var actual = sut.CreateRenderChildTemplateToken(new RenderChildTemplateDirectiveModel());

            // Assert
            actual.Should().BeAssignableTo<IRenderChildTemplateToken<SectionContextExtensionsTests>>();
        }

        [Fact]
        public void CreateRenderChildTemplateToken_Throws_On_Null_Context()
        {
            // Arrange
            SectionContext<SectionContextExtensionsTests> nullSut = null;

            // Act & Assert
            this.Invoking(_ => nullSut.CreateRenderChildTemplateToken(new RenderChildTemplateDirectiveModel()))
                   .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void CreateRenderChildTemplateToken_Throws_On_Null_Model()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.CreateRenderChildTemplateToken(null))
               .Should().Throw<ArgumentNullException>().WithParameterName("model");
        }

        [Fact]
        public void GetChildTemplateTokens_Throws_On_Null_Context()
        {
            // Arrange
            SectionContext<SectionContextExtensionsTests> nullSut = null;

            // Act & Assert
            this.Invoking(_ => nullSut.GetChildTemplateTokens(null, string.Empty))
                .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void GetChildTemplateTokens_Throws_On_Null_FileContentsProvider()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.GetChildTemplateTokens(null, string.Empty))
                .Should().Throw<ArgumentNullException>().WithParameterName("fileContentsProvider");
        }

        [Fact]
        public void GetRootClassName_Throws_On_Null_Context()
        {
            // Arrange
            SectionContext<SectionContextExtensionsTests> nullSut = null;

            // Act & Assert
            this.Invoking(_ => nullSut.GetRootClassName())
                .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void GetClassName_Throws_On_Null_Context()
        {
            // Arrange
            SectionContext<SectionContextExtensionsTests> nullSut = null;

            // Act & Assert
            this.Invoking(_ => nullSut.GetClassName())
                .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        private SectionContext<SectionContextExtensionsTests> CreateSut(int modePosition = ModePosition.Render)
        {
            var tokenParserCallbackMock = new Mock<ITokenParserCallback<SectionContextExtensionsTests>>();
            var loggerMock = new Mock<ILogger>();
            return SectionContext.FromSection(new Section("test.template", 1, "<# Hello world! #>"),
                                              modePosition,
                                              Enumerable.Empty<ITemplateToken<SectionContextExtensionsTests>>(),
                                              tokenParserCallbackMock.Object,
                                              this,
                                              loggerMock.Object,
                                              Array.Empty<TemplateParameter>());
        }
    }
}
