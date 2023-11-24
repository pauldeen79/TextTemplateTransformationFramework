using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Loader;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.RenderTokens;
using TextTemplateTransformationFramework.Common.SectionProcessors.Sections;
using TextTemplateTransformationFramework.T4.Core.Extensions;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Tests
{
    [ExcludeFromCodeCoverage]
    public sealed class TokenParserTests : TestBase, IDisposable
    {
        private readonly ServiceProvider _provider;

        public TokenParserTests()
        {
            var serviceCollection = new ServiceCollection()
                .AddTextTemplateTransformationT4NetCore()
                .AddSingleton<ITemplateSectionProcessor<TokenParserState>, DataTableSectionProcessor>();
            _provider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public void Ctor_Throws_On_Null_Arguments()
        {
            ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(typeof(TokenParser));
        }

        [Fact]
        public void Can_Escape_Placeholders_With_Backslash()
        {
            // Arrange
            const string Src = @"<#@ template language=""c#"" #>\<# literal \#>";
            var sut = _provider.GetRequiredService<ITextTemplateTokenParser<TokenParserState>>();

            // Act
            var actual = sut.Parse
            (
                new TextTemplateProcessorContext<TokenParserState>
                (
                    new TextTemplate(Src),
                    Array.Empty<TemplateParameter>(),
                    _provider.GetRequiredService<ILoggerFactory>().Create(),
                    null
                )
            );

            // Assert
            actual.Should().HaveCount(2);
            actual.Cast<ISourceSectionToken<TokenParserState>>().ElementAt(1).SectionProcessorType.Should().Be(typeof(TextSection<TokenParserState>));
            actual.Cast<ISourceSectionToken<TokenParserState>>().ElementAt(1).TemplateTokens.First().Should().BeOfType<RenderTextToken<TokenParserState>>();
        }

        [Fact]
        public void Can_Use_DataTable_In_Directive()
        {
            // Arrange
            const string Src = @"<#@ template language=""c#""#>
<#@ datatable value=""
| Name        | Type           |
| Property1   | System.String  |
| Property2   | System.Boolean |"" #>";
            var sut = _provider.GetRequiredService<ITextTemplateTokenParser<TokenParserState>>();

            // Act
            var actual = sut.Parse
            (
                new TextTemplateProcessorContext<TokenParserState>
                (
                    new TextTemplate(Src),
                    Array.Empty<TemplateParameter>(),
                    _provider.GetRequiredService<ILoggerFactory>().Create(),
                    null
                )
            );

            // Assert
            actual.Should().HaveCount(2);
            actual.Cast<ISourceSectionToken<TokenParserState>>().ElementAt(1).TemplateTokens.First().Should().BeOfType<RenderTextToken<TokenParserState>>();
            var renderTextToken = actual.Cast<ISourceSectionToken<TokenParserState>>().ElementAt(1).TemplateTokens.First() as RenderTextToken<TokenParserState>;
            renderTextToken.Contents.Should().Be(@"
| Name        | Type           |
| Property1   | System.String  |
| Property2   | System.Boolean |");
        }

        [Fact]
        public void Parse_Throws_On_Null_Argument()
        {
            // Arrange
            var sut = _provider.GetRequiredService<ITextTemplateTokenParser<TokenParserState>>();

            // Act & Assert
            sut.Invoking(x => x.Parse(null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Parse_Returns_No_Tokens_When_AssemblyTemplate_Is_Present_In_Context()
        {
            // Arrange
            var sut = _provider.GetRequiredService<ITextTemplateTokenParser<TokenParserState>>();
            var context = new TextTemplateProcessorContext<TokenParserState>(new AssemblyTemplate(GetType().Assembly.FullName, typeof(MyAssemblyTemplate).FullName, AssemblyLoadContext.Default), Array.Empty<TemplateParameter>(), _provider.GetRequiredService<ILoggerFactory>().Create(), SectionContext<TokenParserState>.Empty);

            // Act
            var actual = sut.Parse(context);

            // Assert
            actual.Should().BeEmpty();
        }

        public void Dispose() => _provider.Dispose();

        [DirectivePrefix("datatable")]
        private sealed class DataTableSectionProcessor : ITemplateSectionProcessor<TokenParserState>
        {
            public SectionProcessResult<TokenParserState> Process(SectionContext<TokenParserState> context)
            {
                var table = context.TokenParserCallback.GetSectionArguments(context, "value");
                return SectionProcessResult.Create(new RenderTextToken<TokenParserState>(context, table.FirstOrDefault()));
            }
        }
    }

    [ExcludeFromCodeCoverage]
    public class MyAssemblyTemplate
    {
    }
}
