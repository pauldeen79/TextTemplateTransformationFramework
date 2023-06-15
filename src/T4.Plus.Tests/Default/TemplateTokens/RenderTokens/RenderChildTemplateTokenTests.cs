using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.RenderTokens;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.Default.TemplateTokens.RenderTokens
{
    [ExcludeFromCodeCoverage]
    public class RenderChildTemplateTokenTests
    {
        [Fact]
        public void Ctor_Throws_On_Null_ChildTemplate()
        {
            // Act & Assert
            this.Invoking(_ => new RenderChildTemplateToken<RenderChildTemplateTokenTests>(
                SectionContext.FromCurrentMode(1, this),
                null,
                new(string.Empty, true),
                false,
                false,
                new(string.Empty, true),
                new(
                    new(string.Empty, true),
                    new(string.Empty, true),
                    new(string.Empty, true),
                    new(string.Empty, true)
                    )))
                .Should().Throw<ArgumentNullException>().WithParameterName("childTemplate");
        }

        [Fact]
        public void Ctor_Throws_On_Null_Model()
        {
            // Act & Assert
            this.Invoking(_ => new RenderChildTemplateToken<RenderChildTemplateTokenTests>(
                SectionContext.FromCurrentMode(1, this),
                new(string.Empty, true),
                null,
                false,
                false,
                new(string.Empty, true),
                new(
                    new(string.Empty, true),
                    new(string.Empty, true),
                    new(string.Empty, true),
                    new(string.Empty, true)
                    )))
                .Should().Throw<ArgumentNullException>().WithParameterName("model");
        }

        [Fact]
        public void Ctor_Throws_On_Null_SeparatorTemplate()
        {
            // Act & Assert
            this.Invoking(_ => new RenderChildTemplateToken<RenderChildTemplateTokenTests>(
                SectionContext.FromCurrentMode(1, this),
                new(string.Empty, true),
                new(string.Empty, true),
                false,
                false,
                null,
                new(
                    new(string.Empty, true),
                    new(string.Empty, true),
                    new(string.Empty, true),
                    new(string.Empty, true)
                    )))
                .Should().Throw<ArgumentNullException>().WithParameterName("separatorTemplate");
        }

        [Fact]
        public void Ctor_Throws_On_Null_CustomRenderData()
        {
            // Act & Assert
            this.Invoking(_ => new RenderChildTemplateToken<RenderChildTemplateTokenTests>(
                SectionContext.FromCurrentMode(1, this),
                new(string.Empty, true),
                new(string.Empty, true),
                false,
                false,
                new(string.Empty, true),
                null))
                .Should().Throw<ArgumentNullException>().WithParameterName("customRenderData");
        }
    }
}
