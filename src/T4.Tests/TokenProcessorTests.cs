using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Core.Extensions;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Tests
{
    [ExcludeFromCodeCoverage]
    public sealed class TokenProcessorTests : IDisposable
    {
        private readonly ServiceProvider _provider;

        public TokenProcessorTests()
        {
            var serviceCollection = new ServiceCollection().AddTextTemplateTransformationT4NetCore();
            _provider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public void Parse_Throws_On_Null_Context()
        {
            // Arrange
            var sut = _provider.GetRequiredService<ITokenProcessor<TokenParserState>>();

            // Act & Assert
            sut.Invoking(x => x.Process(null, Enumerable.Empty<ITemplateToken<TokenParserState>>())).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Parse_Throws_On_Null_Tokens()
        {
            // Arrange
            var sut = _provider.GetRequiredService<ITokenProcessor<TokenParserState>>();
            var context = new TextTemplateProcessorContext<TokenParserState>(new AssemblyTemplate(GetType().Assembly.FullName, typeof(MyAssemblyTemplate).FullName), Array.Empty<TemplateParameter>(), _provider.GetRequiredService<ILoggerFactory>().Create(), SectionContext<TokenParserState>.Empty);

            // Act & Assert
            sut.Invoking(x => x.Process(context, null)).Should().Throw<ArgumentNullException>();
        }
        [Fact]
        public void Parse_Returns_Assembly_From_AssemblyTemplate_When_Present_In_Context()
        {
            // Arrange
            var sut = _provider.GetRequiredService<ITokenProcessor<TokenParserState>>();
            var context = new TextTemplateProcessorContext<TokenParserState>(new AssemblyTemplate(GetType().Assembly.FullName, typeof(MyAssemblyTemplate).FullName), Array.Empty<TemplateParameter>(), _provider.GetRequiredService<ILoggerFactory>().Create(), SectionContext<TokenParserState>.Empty);

            // Act
            var actual = sut.Process(context, Enumerable.Empty<ITemplateToken<TokenParserState>>());

            // Assert
            actual.Should().NotBeNull();
            actual.ClassName.Should().Be(typeof(MyAssemblyTemplate).FullName);
        }

        public void Dispose() => _provider.Dispose();
    }
}
