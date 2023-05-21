﻿using System;
using FluentAssertions;
using Moq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Requests;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.Requests
{
    public class ProcessAssemblyTemplateRequestTests
    {
        [Fact]
        public void Ctor_Throws_On_Null_AssemblyTemplate()
        {
            this.Invoking(_ => new ProcessAssemblyTemplateRequest<ProcessAssemblyTemplateRequestTests>(null, Array.Empty<TemplateParameter>(), new Mock<ITextTemplateProcessorContext<ProcessAssemblyTemplateRequestTests>>().Object))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_Does_Not_Throw_On_Null_Parameters()
        {
            this.Invoking(_ => new ProcessAssemblyTemplateRequest<ProcessAssemblyTemplateRequestTests>(new AssemblyTemplate("", "", ""), null, new Mock<ITextTemplateProcessorContext<ProcessAssemblyTemplateRequestTests>>().Object))
                .Should().NotThrow<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_Throws_On_Null_Context()
        {
            this.Invoking(_ => new ProcessAssemblyTemplateRequest<ProcessAssemblyTemplateRequestTests>(new AssemblyTemplate("", "", ""), Array.Empty<TemplateParameter>(), null))
                .Should().Throw<ArgumentNullException>();
        }
    }
}