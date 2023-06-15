using System;
using FluentAssertions;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Plus.Tests
{
    public class CustomRenderDataTests
    {
        [Fact]
        public void Ctor_Should_Throw_On_Null_CustomResolverDelegate()
        {
            this.Invoking(x => new CustomRenderData(null, new("", true), new("", true), new("", true))).Should().Throw<ArgumentNullException>().WithParameterName("customResolverDelegate");
        }

        [Fact]
        public void Ctor_Should_Throw_On_Null_ResolverDelegateModel()
        {
            this.Invoking(x => new CustomRenderData(new("", true), null, new("", true), new("", true))).Should().Throw<ArgumentNullException>().WithParameterName("resolverDelegateModel");
        }

        [Fact]
        public void Ctor_Should_Throw_On_Null_CustomRenderChildTemplateDelegate()
        {
            this.Invoking(x => new CustomRenderData(new("", true), new("", true), null, new("", true))).Should().Throw<ArgumentNullException>().WithParameterName("customRenderChildTemplateDelegate");
        }

        [Fact]
        public void Ctor_Should_Throw_On_Null_CustomTemplateNameDelegate()
        {
            this.Invoking(x => new CustomRenderData(new("", true), new("", true), new("", true), null)).Should().Throw<ArgumentNullException>().WithParameterName("customTemplateNameDelegate");
        }

    }
}
