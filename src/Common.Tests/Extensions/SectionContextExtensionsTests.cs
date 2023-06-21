using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.BaseClassFooterTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.ClassFooterTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.InitializeTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.NamespaceFooterTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.RenderTokens;
using TextTemplateTransformationFramework.Common.Extensions;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class SectionContextExtensionsTests
    {
        [Theory,
            InlineData(Mode.CodeInitialize, typeof(InitializeErrorToken<SectionContextExtensionsTests>)),
            InlineData(Mode.CodeRender, typeof(RenderErrorToken<SectionContextExtensionsTests>)),
            InlineData(Mode.CodeClassFeature, typeof(ClassFooterErrorToken<SectionContextExtensionsTests>)),
            InlineData(Mode.CodeBaseClassFeature, typeof(BaseClassFooterErrorToken<SectionContextExtensionsTests>)),
            InlineData(Mode.CodeNamespaceFeature, typeof(NamespaceFooterErrorToken<SectionContextExtensionsTests>)),
            InlineData(int.MaxValue, typeof(RenderErrorToken<SectionContextExtensionsTests>))]
        public void CreateErrorToken_Returns_Correct_ErrorToken_For_Specified_ModePosition(int currentMode, Type expectedType)
        {
            // Arrange
            var context = SectionContext.FromCurrentMode(currentMode, this);

            // Act
            var result = context.CreateErrorToken("Kaboom");

            // Assert
            result.Should().BeOfType(expectedType);
            result.Message.Should().BeOneOf("Kaboom", "Unsupported mode: 2147483647");
        }

        [Theory,
            InlineData(Mode.CodeInitialize, typeof(InitializeWarningToken<SectionContextExtensionsTests>)),
            InlineData(Mode.CodeRender, typeof(RenderWarningToken<SectionContextExtensionsTests>)),
            InlineData(Mode.CodeClassFeature, typeof(ClassFooterWarningToken<SectionContextExtensionsTests>)),
            InlineData(Mode.CodeBaseClassFeature, typeof(BaseClassFooterWarningToken<SectionContextExtensionsTests>)),
            InlineData(Mode.CodeNamespaceFeature, typeof(NamespaceFooterWarningToken<SectionContextExtensionsTests>)),
            InlineData(int.MaxValue, typeof(RenderWarningToken<SectionContextExtensionsTests>))]
        public void CreateWarningToken_Returns_Correct_WarningToken_For_Specified_ModePosition(int currentMode, Type expectedType)
        {
            // Arrange
            var context = SectionContext.FromCurrentMode(currentMode, this);

            // Act
            var result = context.CreateWarningToken("Kaboom");

            // Assert
            result.Should().BeOfType(expectedType);
            result.Message.Should().BeOneOf("Kaboom", "Unsupported mode: 2147483647");
        }

        [Theory,
            InlineData(Mode.CodeInitialize, true, typeof(InitializeTextToken<SectionContextExtensionsTests>)),
            InlineData(Mode.CodeRender, true, typeof(RenderTextToken<SectionContextExtensionsTests>)),
            InlineData(Mode.CodeClassFeature, true, typeof(ClassFooterTextToken<SectionContextExtensionsTests>)),
            InlineData(Mode.CodeBaseClassFeature, true, typeof(BaseClassFooterTextToken<SectionContextExtensionsTests>)),
            InlineData(Mode.CodeNamespaceFeature, true, typeof(NamespaceFooterTextToken<SectionContextExtensionsTests>)),
            InlineData(int.MaxValue, true, typeof(RenderErrorToken<SectionContextExtensionsTests>)),
            InlineData(int.MaxValue, false, null),
            InlineData(Mode.TextRangeEnd, false, null)]
        public void CreateTextToken_Returns_Correct_TextToken_For_Specified_ModePosition(int currentMode, bool force, Type expectedType)
        {
            // Arrange
            var context = SectionContext.FromCurrentMode(currentMode, this);

            // Act
            var result = context.CreateTextToken("My text", force);

            // Assert
            if (expectedType == null)
            {
                result.Should().BeNull();
            }
            else
            {
                result.Should().BeOfType(expectedType);
                if (result is ITextToken<SectionContextExtensionsTests> textToken)
                {
                    textToken.Contents.Should().Be("My text");
                }
            }
        }

        [Theory,
            InlineData(Mode.ExpressionInitialize, typeof(InitializeExpressionToken<SectionContextExtensionsTests>)),
            InlineData(Mode.ExpressionRender, typeof(RenderExpressionToken<SectionContextExtensionsTests>)),
            InlineData(Mode.ExpressionClassFeature, typeof(ClassFooterExpressionToken<SectionContextExtensionsTests>)),
            InlineData(Mode.ExpressionBaseClassFeature, typeof(BaseClassFooterExpressionToken<SectionContextExtensionsTests>)),
            InlineData(Mode.ExpressionNamespaceFeature, typeof(NamespaceFooterExpressionToken<SectionContextExtensionsTests>)),
            InlineData(Mode.ExpressionRangeEnd, null),
            InlineData(Mode.CodeRender, null),
            InlineData(int.MaxValue, null)]
        public void CreateExpressionToken_Returns_Correct_ExpressionToken_For_Specified_ModePosition(int currentMode, Type expectedType)
        {
            // Arrange
            var context = SectionContext.FromCurrentMode(currentMode, this);

            // Act
            var result = context.CreateExpressionToken("My expression");

            // Assert
            if (expectedType == null)
            {
                result.Should().BeNull();
            }
            else
            {
                result.Should().BeOfType(expectedType);
                result.Expression.Should().Be("My expression");
            }
        }
    }
}
