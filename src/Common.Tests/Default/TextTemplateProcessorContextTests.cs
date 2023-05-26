using System;
using FluentAssertions;
using Moq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.Default
{
    public class TextTemplateProcessorContextTests
    {
        [Fact]
        public void Ctor_Throws_On_Null_TextTemplate()
        {
            // Act & Assert
            this.Invoking(_ => new TextTemplateProcessorContext<TextTemplateProcessorContextTests>((TextTemplate)null, null, new Mock<ILogger>().Object, SectionContext<TextTemplateProcessorContextTests>.Empty))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_Throws_On_Null_AssemblyTemplate()
        {
            // Act & Assert
            this.Invoking(_ => new TextTemplateProcessorContext<TextTemplateProcessorContextTests>((AssemblyTemplate)null, null, new Mock<ILogger>().Object, SectionContext<TextTemplateProcessorContextTests>.Empty))
                .Should().Throw<ArgumentNullException>();
        }
    }
}
