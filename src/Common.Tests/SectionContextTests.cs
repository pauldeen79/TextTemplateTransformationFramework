using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using TextTemplateTransformationFramework.Common.Contracts;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests
{
    [ExcludeFromCodeCoverage]
    public class SectionContextTests : TestBase
    {
        [Fact]
        public void FromSection_Throws_On_Null_Section_Argument()
        {
            // Act & Assert
            this.Invoking(_ => SectionContext.FromSection(null, 1, Enumerable.Empty<ITemplateToken<SectionContextTests>>(), Fixture.Freeze<ITokenParserCallback<SectionContextTests>>(), this, Fixture.Freeze<ILogger>(), Array.Empty<TemplateParameter>()))
                .Should().Throw<ArgumentNullException>().WithParameterName("section");
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
