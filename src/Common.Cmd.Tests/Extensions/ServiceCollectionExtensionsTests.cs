using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ScriptCompiler;
using TextTemplateTransformationFramework.Common.Cmd.Extensions;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Cmd.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void All_Dependencies_Can_Be_Resolved()
        {
            // Act
            var action = new Action(() => _ = new ServiceCollection()
                .AddTransient(_ => new Mock<IScriptCompiler>().Object)
                .AddTransient(_ => new Mock<ITokenProcessor<ServiceCollectionExtensionsTests>>().Object)
                .AddTransient(_ => new Mock<IScriptBuilder<ServiceCollectionExtensionsTests>>().Object)
                .AddTransient(_ => new Mock<IFileContentsProvider>().Object)
                .AddTransient(_ => new Mock<ITemplateOutputCreator<ServiceCollectionExtensionsTests>>().Object)
                .AddTransient(_ => new Mock<ITextTemplateTokenParser<ServiceCollectionExtensionsTests>>().Object)
                .AddTransient(_ => new Mock<IAssemblyService>().Object)
                .AddTextTemplateTransformation<ServiceCollectionExtensionsTests>()
                .AddTextTemplateTransformationCommands<ServiceCollectionExtensionsTests>()
                .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true }));

            // Assert
            action.Should().NotThrow();
        }
    }
}
