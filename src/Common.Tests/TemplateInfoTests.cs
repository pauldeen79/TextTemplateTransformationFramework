using System;
using FluentAssertions;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests
{
    public class TemplateInfoTests
    {
        [Fact]
        public void Ctor_Throws_On_Null_ShortName()
        {
            this.Invoking(_ => new TemplateInfo(null, "", "", "", default, Array.Empty<TemplateParameter>()))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_Throws_On_Null_FileName()
        {
            this.Invoking(_ => new TemplateInfo("", null, "", "", default, Array.Empty<TemplateParameter>()))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_Throws_On_Null_AssemblyName()
        {
            this.Invoking(_ => new TemplateInfo("", "", null, "", default, Array.Empty<TemplateParameter>()))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_Throws_On_Null_ClassName()
        {
            this.Invoking(_ => new TemplateInfo("", "", "", null, default, Array.Empty<TemplateParameter>()))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_Throws_On_Null_Parameters()
        {
            this.Invoking(_ => new TemplateInfo("", "", "", "", default, null))
                .Should().Throw<ArgumentNullException>();
        }
    }
}
