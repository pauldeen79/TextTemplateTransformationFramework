using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Moq;
using TextTemplateTransformationFramework.Common.Contracts;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests
{
    [ExcludeFromCodeCoverage]
    public class SectionContextTests
    {
        [Fact]
        public void FromSection_Throws_On_Null_Section_Argument()
        {
            // Act & Assert
            this.Invoking(_ => SectionContext.FromSection(null, 1, Enumerable.Empty<ITemplateToken<SectionContextTests>>(), new Mock<ITokenParserCallback<SectionContextTests>>().Object, this, new Mock<ILogger>().Object, Array.Empty<TemplateParameter>()))
                .Should().Throw<ArgumentNullException>().WithParameterName("section");
        }

        [Fact]
        public void FromSection_Throws_On_Null_TokenParserCallback_Argument()
        {
            // Act & Assert
            this.Invoking(_ => SectionContext.FromSection(new Section("my.template", 1, "contents"), 1, Enumerable.Empty<ITemplateToken<SectionContextTests>>(), null, this, new Mock<ILogger>().Object, Array.Empty<TemplateParameter>()))
                .Should().Throw<ArgumentNullException>().WithParameterName("tokenParserCallback");
        }

        [Fact]
        public void FromToken_Throws_On_Null_Token()
        {
            // Act & Assert
            this.Invoking(_ => SectionContext.FromToken(null, this))
                .Should().Throw<ArgumentNullException>().WithParameterName("token");
        }
    }
}
