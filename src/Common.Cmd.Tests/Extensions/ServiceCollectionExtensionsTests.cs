using System;
using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using ScriptCompiler;
using TextTemplateTransformationFramework.Common.Cmd.Extensions;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Cmd.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class ServiceCollectionExtensionsTests : TestBase
    {
        [Fact]
        public void All_Dependencies_Can_Be_Resolved()
        {
            // Act
            var action = new Action(() => _ = new ServiceCollection()
                .AddTransient(_ => Fixture.Freeze<IScriptCompiler>())
                .AddTransient(_ => Fixture.Freeze<ITokenProcessor<ServiceCollectionExtensionsTests>>())
                .AddTransient(_ => Fixture.Freeze<IScriptBuilder<ServiceCollectionExtensionsTests>>())
                .AddTransient(_ => Fixture.Freeze<IFileContentsProvider>())
                .AddTransient(_ => Fixture.Freeze<ITemplateOutputCreator<ServiceCollectionExtensionsTests>>())
                .AddTransient(_ => Fixture.Freeze<ITextTemplateTokenParser<ServiceCollectionExtensionsTests>>())
                .AddTransient(_ => Fixture.Freeze<IAssemblyService>())
                .AddTextTemplateTransformation<ServiceCollectionExtensionsTests>()
                .AddTextTemplateTransformationCommands<ServiceCollectionExtensionsTests>()
                .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true }));

            // Assert
            action.Should().NotThrow();
        }
    }
}
