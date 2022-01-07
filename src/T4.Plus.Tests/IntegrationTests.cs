using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.Runtime;
using TextTemplateTransformationFramework.T4.Plus.Core.Extensions;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Plus.Tests
{
    [ExcludeFromCodeCoverage]
    public sealed class IntegrationTests : IDisposable
    {
        private readonly ServiceProvider _provider;

        public IntegrationTests()
        {
            var serviceCollection = new ServiceCollection()
                .AddTextTemplateTransformationT4PlusNetCore();
            _provider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public void CanRenderTemplateUsingT4PlusTemplateProcessor()
        {
            // Arrange
            var sut = _provider.GetRequiredService<ITextTemplateProcessor>();
            const string Src = @"<#@ template language=""c#"" #>
Hello <#= ""world"" #><# Write(""!""); #>";

            // Act
            var actual = sut.Process(Src);

            // Assert
            actual.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void CanRenderTemplateUsingT4PlusTemplateProcessorWithRuntimeBaseClass()
        {
            // Arrange
            var sut = _provider.GetRequiredService<ITextTemplateProcessor>();
            const string Src = @"<#@ template language=""c#"" #><#@ useTemplateRuntimeBaseClass #>
<#@ assembly name=""TextTemplateTransformationFramework.T4.Plus.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"" #>
Hello <#= ""world"" #><# Write(""!""); #>";

            // Act
            var actual = sut.Process(Src);

            // Assert
            actual.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void CanRunOldChildTemplateClassUsingRuntime()
        {
            // Arrange
            var template = new OldTemplate.GeneratedTemplate();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(template);

            // Assert
            actual.Should().Be(@"Hello world!");
        }

        [Fact]
        public void CanRunNewChildTemplateClassUsingRuntime()
        {
            // Arrange
            var template = new NewTemplate.GeneratedTemplate();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(template);

            // Assert
            actual.Should().Be(@"Hello world!");
        }

        public void Dispose()
            => _provider.Dispose();
    }
}
