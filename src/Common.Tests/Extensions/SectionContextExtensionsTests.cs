using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
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
            InlineData(Mode.CodeNamespaceFeature, typeof(NamespaceFooterErrorToken<SectionContextExtensionsTests>))]
        public void CreateErrorToken_Returns_Correct_ErrorToken_For_Specified_ModePosition(int currentMode, Type expectedType)
        {
            // Arrange
            var context = SectionContext.FromCurrentMode(currentMode, this);

            // Act
            var result = context.CreateErrorToken("Kaboom");

            // Assert
            result.Should().BeOfType(expectedType);
        }
    }
}
