using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.RenderTokens;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.Default.TemplateTokens.RenderTokens
{
    [ExcludeFromCodeCoverage]
    public class RenderGeneratorAssemblyTokenTests
    {
        [Fact]
        public void Ctor_Throws_On_Null_AssemblyName()
        {
            this.Invoking(_ => new RenderGeneratorAssemblyToken<RenderGeneratorAssemblyTokenTests>(
                SectionContext.FromCurrentMode(1, this),
                null,
                new ValueSpecifier("", true),
                new ValueSpecifier("", true),
                true,
                true)).Should().Throw<ArgumentNullException>().WithParameterName("assemblyName");
        }

        [Fact]
        public void Ctor_Throws_On_Null_BasePath()
        {
            this.Invoking(_ => new RenderGeneratorAssemblyToken<RenderGeneratorAssemblyTokenTests>(
                SectionContext.FromCurrentMode(1, this),
                new ValueSpecifier("", true),
                null,
                new ValueSpecifier("", true),
                true,
                true)).Should().Throw<ArgumentNullException>().WithParameterName("basePath");
        }

        [Fact]
        public void Ctor_Throws_On_Null_CurrentDirectory()
        {
            this.Invoking(_ => new RenderGeneratorAssemblyToken<RenderGeneratorAssemblyTokenTests>(
                SectionContext.FromCurrentMode(1, this),
                new ValueSpecifier("", true),
                new ValueSpecifier("", true),
                null,
                true,
                true)).Should().Throw<ArgumentNullException>().WithParameterName("currentDirectory");
        }
    }
}
