using System;
using FluentAssertions;
using Moq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Requests;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.Requests
{
    public class ProcessTextTemplateRequestTests
    {
        [Fact]
        public void Ctor_Does_Not_Throw_On_Null_Parameters()
        {
            this.Invoking(_ => new ProcessTextTemplateRequest<ProcessTextTemplateRequestTests>(null, new Mock<ITextTemplateProcessorContext<ProcessTextTemplateRequestTests>>().Object))
                .Should().NotThrow<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_Throws_On_Null_Context()
        {
            this.Invoking(_ => new ProcessTextTemplateRequest<ProcessTextTemplateRequestTests>(Array.Empty<TemplateParameter>(), null))
                .Should().Throw<ArgumentNullException>();
        }
    }
}
