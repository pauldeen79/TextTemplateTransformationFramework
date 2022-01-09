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
        public void CreateRenderChildTemplateToken_Returns_Error_Token_When_Mode_Is_Not_Render()
        {
            // Arrange
            var tokenParserCallbackMock = new Mock<ITokenParserCallback<SectionContextExtensionsTests>>();
            var loggerMock = new Mock<ILogger>();
            var sut = SectionContext.FromSection("<# Hello world! #>",
                                                 1000 + ModePosition.Initialize, //Note that it's important that we increase the mode by 1000. Else GetCurrentPosition() returns Render anyway :(
                                                 1,
                                                 "test.template",
                                                 Enumerable.Empty<ITemplateToken<SectionContextExtensionsTests>>(),
                                                 tokenParserCallbackMock.Object,
                                                 this,
                                                 loggerMock.Object);

            // Act
            var actual = sut.CreateRenderChildTemplateToken(new RenderChildTemplateDirectiveModel());

            // Assert
            actual.Should().BeAssignableTo<IRenderErrorToken<SectionContextExtensionsTests>>();
        }

        [Fact]
        public void CreateRenderChildTemplateToken_Returns_Error_Token_When_Mode_Is_Render()
        {
            // Arrange
            var tokenParserCallbackMock = new Mock<ITokenParserCallback<SectionContextExtensionsTests>>();
            var loggerMock = new Mock<ILogger>();
            var sut = SectionContext.FromSection("<# Hello world! #>",
                                                 ModePosition.Render,
                                                 1,
                                                 "test.template",
                                                 Enumerable.Empty<ITemplateToken<SectionContextExtensionsTests>>(),
                                                 tokenParserCallbackMock.Object,
                                                 this,
                                                 loggerMock.Object);

            // Act
            var actual = sut.CreateRenderChildTemplateToken(new RenderChildTemplateDirectiveModel());

            // Assert
            actual.Should().BeAssignableTo<IRenderChildTemplateToken<SectionContextExtensionsTests>>();
        }
    }
}
