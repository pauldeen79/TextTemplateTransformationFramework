using System;
using AutoFixture;
using FluentAssertions;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Requests;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.Requests
{
    public class ProcessTextTemplateRequestTests : TestBase
    {
        [Fact]
        public void Ctor_Does_Not_Throw_On_Null_Parameters()
        {
            this.Invoking(_ => new ProcessTextTemplateRequest<ProcessTextTemplateRequestTests>(null, Fixture.Freeze<ITextTemplateProcessorContext<ProcessTextTemplateRequestTests>>()))
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
