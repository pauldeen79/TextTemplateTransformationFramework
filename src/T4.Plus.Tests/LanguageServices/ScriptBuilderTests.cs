using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.SectionProcessors.Sections;
using TextTemplateTransformationFramework.T4.Plus.Core.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Models;
using TextTemplateTransformationFramework.T4.Plus.Tests.TestFixtures;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.LanguageServices
{
    [ExcludeFromCodeCoverage]
    public sealed class ScriptBuilderTests : IDisposable
    {
        private readonly ServiceProvider _provider;

        public ScriptBuilderTests()
        {
            var serviceCollection = new ServiceCollection()
                .AddTextTemplateTransformationT4PlusNetCore()
                .AddSingleton<IFileContentsProvider, FileContentsProviderMock>();
            _provider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public void CanBuildScriptFromSingleObject()
        {
            // Arrange
            var input = new AssemblyDirectiveModel<TokenParserState>
            {
                Name = "My assembly",
                PreLoad = true,
                HintPath = @"c:\somewhere",
            };
            var sut = _provider.GetRequiredService<IScriptBuilder<TokenParserState>>();

            // Act
            var actual = sut.Build(new FakeAssemblyDirective<TokenParserState>(), input);

            // Assert
            actual.Should().Be(@"<#@ Assembly Name=""My assembly"" HintPath=""c:\somewhere"" PreLoad=""true"" #>");
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
            var input1 = new AssemblyDirectiveModel<TokenParserState>
            {
                Name = "My assembly 1",
                PreLoad = true,
                HintPath = @"c:\somewhere",
            };
            var input2 = new AssemblyDirectiveModel<TokenParserState>
            {
                Name = "My assembly 2",
                PreLoad = false,
                HintPath = @"c:\somewhere",
            };
            var sut = _provider.GetRequiredService<IScriptBuilder<TokenParserState>>();

            // Act
            var actual = sut.Build(new FakeAssemblyDirective<TokenParserState>(), input1, input2);

            // Assert
            actual.Should().Be(@"<#@ Assembly Name=""My assembly 1"" HintPath=""c:\somewhere"" PreLoad=""true"" #>
<#@ Assembly Name=""My assembly 2"" HintPath=""c:\somewhere"" #>");
        }

        [Fact]
        public void CanDiscoverDirectivesExcludingT4PlusDirectives()
        {
            // Arrange
            var sut = _provider.GetRequiredService<IScriptBuilder<TokenParserState>>();

            // Act
            var actual = sut.GetKnownDirectives().ToArray();

            // Assert
            actual.Should().HaveCount(50);
        }

        public void Dispose()
            => _provider.Dispose();
    }
}
