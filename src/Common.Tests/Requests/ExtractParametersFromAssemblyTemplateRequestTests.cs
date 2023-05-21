using System;
using FluentAssertions;
using Moq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Requests;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.Requests
{
    public class ExtractParametersFromAssemblyTemplateRequestTests
    {
        [Fact]
        public void Ctor_Throws_On_Null_AssemblyTemplate()
        {
            this.Invoking(_ => new ExtractParametersFromAssemblyTemplateRequest<ExtractParametersFromAssemblyTemplateRequestTests>(null, new Mock<ITextTemplateProcessorContext<ExtractParametersFromAssemblyTemplateRequestTests>>().Object))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_Throws_On_Null_Context()
        {
            this.Invoking(_ => new ExtractParametersFromAssemblyTemplateRequest<ExtractParametersFromAssemblyTemplateRequestTests>(new AssemblyTemplate("", "", ""), null))
                .Should().Throw<ArgumentNullException>();
        }
    }
}
