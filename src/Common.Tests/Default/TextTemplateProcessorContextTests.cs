using System;
using AutoFixture;
using FluentAssertions;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.Default
{
    public class TextTemplateProcessorContextTests : TestBase
    {
        [Fact]
        public void Ctor_Throws_On_Null_TextTemplate()
        {
            // Act & Assert
            this.Invoking(_ => new TextTemplateProcessorContext<TextTemplateProcessorContextTests>((TextTemplate)null, null, Fixture.Freeze<ILogger>(), SectionContext<TextTemplateProcessorContextTests>.Empty))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_Throws_On_Null_AssemblyTemplate()
        {
            // Act & Assert
            this.Invoking(_ => new TextTemplateProcessorContext<TextTemplateProcessorContextTests>((AssemblyTemplate)null, null, Fixture.Freeze<ILogger>(), SectionContext<TextTemplateProcessorContextTests>.Empty))
                .Should().Throw<ArgumentNullException>();
        }
    }
}
