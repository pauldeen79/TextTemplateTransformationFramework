using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.Default.TemplateTokens.InitializeTokens
{
    [ExcludeFromCodeCoverage]
    public class RootTemplateInitializeViewModelTokenTests : TestBase
    {
        [Fact]
        public void Can_Construct()
        {
            // Arrange
            var loggerMock = Fixture.Freeze<ILogger>();
            var tokenParserCallbackMock = Fixture.Freeze<ITokenParserCallback<RootTemplateInitializeViewModelTokenTests>>();
            var context = SectionContext.FromSection
            (
                new Section("test.template", 1, "contents"),
                Mode.CodeCompositionRootFeature,
                Enumerable.Empty<ITemplateToken<RootTemplateInitializeViewModelTokenTests>>(),
                tokenParserCallbackMock,
                this,
                loggerMock,
                Array.Empty<TemplateParameter>()
            );
            var viewModel = new ValueSpecifier("viewModel", true);
            var model = new ValueSpecifier("model", true);
            var silentlyContinueOnError = true;
            var customResolverDelegate = new ValueSpecifier("customResolverDelegate", true);
            var customResolverDelegateModel = new ValueSpecifier("resolverDelegate", true);

            // Act
            var actual = new RootTemplateInitializeViewModelToken<RootTemplateInitializeViewModelTokenTests>(context,
                                                                                                             viewModel,
                                                                                                             model,
                                                                                                             silentlyContinueOnError,
                                                                                                             customResolverDelegate,
                                                                                                             customResolverDelegateModel);

            // Assert
            actual.Should().NotBeNull();
            actual.CustomResolverDelegateExpression.Should().Be("customResolverDelegate");
            actual.CustomResolverDelegateExpressionIsLiteral.Should().BeTrue();
            actual.FileName.Should().Be("test.template");
            actual.LineNumber.Should().Be(1);
            actual.Model.Should().Be("model");
            actual.ModelIsLiteral.Should().BeTrue();
            actual.ResolverDelegateModel.Should().Be("resolverDelegate");
            actual.ResolverDelegateModelIsLiteral.Should().BeTrue();
            actual.SectionContext.Should().BeSameAs(context);
            actual.SilentlyContinueOnError.Should().BeTrue();
            actual.ViewModelName.Should().Be("viewModel");
            actual.ViewModelNameIsLiteral.Should().BeTrue();
        }

        [Fact]
        public void Ctor_Throws_On_Null_ViewModel()
        {
            // Arrange
            var loggerMock = Fixture.Freeze<ILogger>();
            var tokenParserCallbackMock = Fixture.Freeze<ITokenParserCallback<RootTemplateInitializeViewModelTokenTests>>();
            var context = SectionContext.FromSection
            (
                new Section("test.template", 1, "contents"),
                Mode.CodeCompositionRootFeature,
                Enumerable.Empty<ITemplateToken<RootTemplateInitializeViewModelTokenTests>>(),
                tokenParserCallbackMock,
                this,
                loggerMock,
                Array.Empty<TemplateParameter>()
            );
            var model = new ValueSpecifier("model", true);
            var silentlyContinueOnError = true;
            var customResolverDelegate = new ValueSpecifier("customResolverDelegate", true);
            var customResolverDelegateModel = new ValueSpecifier("resolverDelegate", true);

            // Act & Assert
            this.Invoking(_ => new RootTemplateInitializeViewModelToken<RootTemplateInitializeViewModelTokenTests>(
                context,
                null,
                model,
                silentlyContinueOnError,
                customResolverDelegate,
                customResolverDelegateModel)).Should().Throw<ArgumentNullException>().WithParameterName("viewModel");
        }

        [Fact]
        public void Ctor_Throws_On_Null_Model()
        {
            // Arrange
            var loggerMock = Fixture.Freeze<ILogger>();
            var tokenParserCallbackMock = Fixture.Freeze<ITokenParserCallback<RootTemplateInitializeViewModelTokenTests>>();
            var context = SectionContext.FromSection
            (
                new Section("test.template", 1, "contents"),
                Mode.CodeCompositionRootFeature,
                Enumerable.Empty<ITemplateToken<RootTemplateInitializeViewModelTokenTests>>(),
                tokenParserCallbackMock,
                this,
                loggerMock,
                Array.Empty<TemplateParameter>()
            );
            var viewModel = new ValueSpecifier("viewModel", true);
            var silentlyContinueOnError = true;
            var customResolverDelegate = new ValueSpecifier("customResolverDelegate", true);
            var customResolverDelegateModel = new ValueSpecifier("resolverDelegate", true);

            // Act & Assert
            this.Invoking(_ => new RootTemplateInitializeViewModelToken<RootTemplateInitializeViewModelTokenTests>(
                context,
                viewModel,
                null,
                silentlyContinueOnError,
                customResolverDelegate,
                customResolverDelegateModel)).Should().Throw<ArgumentNullException>().WithParameterName("model");
        }

        [Fact]
        public void Ctor_Throws_On_Null_CustomResolverDelegate()
        {
            // Arrange
            var loggerMock = Fixture.Freeze<ILogger>();
            var tokenParserCallbackMock = Fixture.Freeze<ITokenParserCallback<RootTemplateInitializeViewModelTokenTests>>();
            var context = SectionContext.FromSection
            (
                new Section("test.template", 1, "contents"),
                Mode.CodeCompositionRootFeature,
                Enumerable.Empty<ITemplateToken<RootTemplateInitializeViewModelTokenTests>>(),
                tokenParserCallbackMock,
                this,
                loggerMock,
                Array.Empty<TemplateParameter>()
            );
            var viewModel = new ValueSpecifier("viewModel", true);
            var model = new ValueSpecifier("model", true);
            var silentlyContinueOnError = true;
            var customResolverDelegateModel = new ValueSpecifier("resolverDelegate", true);

            // Act & Assert
            this.Invoking(_ => new RootTemplateInitializeViewModelToken<RootTemplateInitializeViewModelTokenTests>(
                context,
                viewModel,
                model,
                silentlyContinueOnError,
                null,
                customResolverDelegateModel)).Should().Throw<ArgumentNullException>().WithParameterName("customResolverDelegate");
        }

        [Fact]
        public void Ctor_Throws_On_Null_CstomResolverDelegateModel()
        {
            // Arrange
            var loggerMock = Fixture.Freeze<ILogger>();
            var tokenParserCallbackMock = Fixture.Freeze<ITokenParserCallback<RootTemplateInitializeViewModelTokenTests>>();
            var context = SectionContext.FromSection
            (
                new Section("test.template", 1, "contents"),
                Mode.CodeCompositionRootFeature,
                Enumerable.Empty<ITemplateToken<RootTemplateInitializeViewModelTokenTests>>(),
                tokenParserCallbackMock,
                this,
                loggerMock,
                Array.Empty<TemplateParameter>()
            );
            var viewModel = new ValueSpecifier("viewModel", true);
            var model = new ValueSpecifier("model", true);
            var silentlyContinueOnError = true;
            var customResolverDelegate = new ValueSpecifier("customResolverDelegateModel", true);

            // Act & Assert
            this.Invoking(_ => new RootTemplateInitializeViewModelToken<RootTemplateInitializeViewModelTokenTests>(
                context,
                viewModel,
                model,
                silentlyContinueOnError,
                customResolverDelegate,
                null)).Should().Throw<ArgumentNullException>().WithParameterName("customResolverDelegateModel");
        }
    }
}
