using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Models;
using TextTemplateTransformationFramework.Common.SectionProcessors.Sections;
using TextTemplateTransformationFramework.T4.Core.Extensions;
using TextTemplateTransformationFramework.T4.Tests.TestFixtures;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Tests.LanguageServices
{
    [ExcludeFromCodeCoverage]
    public sealed class ScriptBuilderTests : IDisposable
    {
        private readonly ServiceProvider _provider;

        public ScriptBuilderTests()
        {
            // Note we're using T4 instead of T4Plus, to get a better predictable amount of directives ;-)
            var serviceCollection = new ServiceCollection()
                .AddTextTemplateTransformationT4NetCore()
                .AddSingleton<IFileContentsProvider, FileContentsProviderMock>();
            _provider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public void CanBuildScriptFromSingleObject()
        {
            // Arrange
            var input = new AssemblyDirectiveModel
            {
                Name = "My assembly"
            };
            var sut = _provider.GetRequiredService<IScriptBuilder<TokenParserState>>();

            // Act
            var actual = sut.Build(new FakeAssemblyDirective<TokenParserState>(), input);

            // Assert
            actual.Should().Be(@"<#@ Assembly Name=""My assembly"" #>");
        }

        [Fact]
        public void CanBuildScriptForSection()
        {
            // Arrange
            var sut = _provider.GetRequiredService<IScriptBuilder<TokenParserState>>();

            // Act
            var actual = sut.Build(new ClassFeatureSection<TokenParserState>(), new TextTemplateTransformationFramework.Common.Models.SectionModel { Code = "Kaboom();" });

            // Assert
            actual.Should().Be("<#+ Kaboom(); #>");
        }

        [Fact]
        public void CanBuildScriptFromMultipleObjects()
        {
            // Arrange
            var input1 = new AssemblyDirectiveModel
            {
                Name = "My assembly 1"
            };
            var input2 = new AssemblyDirectiveModel
            {
                Name = "My assembly 2"
            };
            var sut = _provider.GetRequiredService<IScriptBuilder<TokenParserState>>();

            // Act
            var actual = sut.Build(new FakeAssemblyDirective<TokenParserState>(), input1, input2);

            // Assert
            actual.Should().Be(@"<#@ Assembly Name=""My assembly 1"" #>
<#@ Assembly Name=""My assembly 2"" #>");
        }

        [Fact]
        public void CanDiscoverDirectives()
        {
            // Arrange
            var sut = _provider.GetRequiredService<IScriptBuilder<TokenParserState>>();

            // Act
            var actual = sut.GetKnownDirectives().ToArray();

            // Assert
            actual.Should().HaveCount(9);
        }

        public void Dispose()
            => _provider.Dispose();
    }
}
