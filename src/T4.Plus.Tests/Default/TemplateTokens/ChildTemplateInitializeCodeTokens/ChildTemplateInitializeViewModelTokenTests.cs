﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.ChildTemplateInitializeCodeTokens;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.Default.TemplateTokens.ChildTemplateInitializeCodeTokens
{
    [ExcludeFromCodeCoverage]
    public class ChildTemplateInitializeViewModelTokenTests : TestBase
    {
        [Fact]
        public void Ctor_Throws_On_Null_ViewModel()
        {
            // Arrange
            var loggerMock = Fixture.Freeze<ILogger>();
            var tokenParserCallbackMock = Fixture.Freeze<ITokenParserCallback<ChildTemplateInitializeViewModelTokenTests>>();
            var context = SectionContext.FromSection
            (
                new Section("test.template", 1, "contents"),
                Mode.CodeCompositionRootFeature,
                Enumerable.Empty<ITemplateToken<ChildTemplateInitializeViewModelTokenTests>>(),
                tokenParserCallbackMock,
                this,
                loggerMock,
                Array.Empty<TemplateParameter>()
            );
            var model = new ValueSpecifier("model", true);
            var silentlyContinueOnError = true;
            var customResolverDelegate = new ValueSpecifier("customResolverDelegate", true);
            var resolverDelegateModel = new ValueSpecifier("resolverDelegate", true);

            // Act & Assert
            this.Invoking(_ => new ChildTemplateInitializeViewModelToken<ChildTemplateInitializeViewModelTokenTests>(
                context,
                null,
                model,
                silentlyContinueOnError,
                customResolverDelegate,
                resolverDelegateModel))
                .Should().Throw<ArgumentNullException>().WithParameterName("viewModel");
        }

        [Fact]
        public void Ctor_Throws_On_Null_CustomResolverDelegate()
        {
            // Arrange
            var loggerMock = Fixture.Freeze<ILogger>();
            var tokenParserCallbackMock = Fixture.Freeze<ITokenParserCallback<ChildTemplateInitializeViewModelTokenTests>>();
            var context = SectionContext.FromSection
            (
                new Section("test.template", 1, "contents"),
                Mode.CodeCompositionRootFeature,
                Enumerable.Empty<ITemplateToken<ChildTemplateInitializeViewModelTokenTests>>(),
                tokenParserCallbackMock,
                this,
                loggerMock,
                Array.Empty<TemplateParameter>()
            );
            var viewModel = new ValueSpecifier("viewModel", true);
            var model = new ValueSpecifier("model", true);
            var silentlyContinueOnError = true;
            var resolverDelegateModel = new ValueSpecifier("resolverDelegate", true);

            // Act & Assert
            this.Invoking(_ => new ChildTemplateInitializeViewModelToken<ChildTemplateInitializeViewModelTokenTests>(
                context,
                viewModel,
                model,
                silentlyContinueOnError,
                null,
                resolverDelegateModel))
                .Should().Throw<ArgumentNullException>().WithParameterName("customResolverDelegate");
        }

        [Fact]
        public void Ctor_Throws_On_Null_Model()
        {
            // Arrange
            var loggerMock = Fixture.Freeze<ILogger>();
            var tokenParserCallbackMock = Fixture.Freeze<ITokenParserCallback<ChildTemplateInitializeViewModelTokenTests>>();
            var context = SectionContext.FromSection
            (
                new Section("test.template", 1, "contents"),
                Mode.CodeCompositionRootFeature,
                Enumerable.Empty<ITemplateToken<ChildTemplateInitializeViewModelTokenTests>>(),
                tokenParserCallbackMock,
                this,
                loggerMock,
                Array.Empty<TemplateParameter>()
            );
            var viewModel = new ValueSpecifier("viewModel", true);
            var silentlyContinueOnError = true;
            var customResolverDelegate = new ValueSpecifier("customResolverDelegate", true);
            var resolverDelegateModel = new ValueSpecifier("resolverDelegate", true);

            // Act & Assert
            this.Invoking(_ => new ChildTemplateInitializeViewModelToken<ChildTemplateInitializeViewModelTokenTests>(
                context,
                viewModel,
                null,
                silentlyContinueOnError,
                customResolverDelegate,
                resolverDelegateModel))
                .Should().Throw<ArgumentNullException>().WithParameterName("model");
        }

        [Fact]
        public void Ctor_Throws_On_Null_ResolverDelegateModel()
        {
            // Arrange
            var loggerMock = Fixture.Freeze<ILogger>();
            var tokenParserCallbackMock = Fixture.Freeze<ITokenParserCallback<ChildTemplateInitializeViewModelTokenTests>>();
            var context = SectionContext.FromSection
            (
                new Section("test.template", 1, "contents"),
                Mode.CodeCompositionRootFeature,
                Enumerable.Empty<ITemplateToken<ChildTemplateInitializeViewModelTokenTests>>(),
                tokenParserCallbackMock,
                this,
                loggerMock,
                Array.Empty<TemplateParameter>()
            );
            var viewModel = new ValueSpecifier("viewModel", true);
            var model = new ValueSpecifier("model", true);
            var silentlyContinueOnError = true;
            var customResolverDelegate = new ValueSpecifier("customResolverDelegate", true);

            // Act & Assert
            this.Invoking(_ => new ChildTemplateInitializeViewModelToken<ChildTemplateInitializeViewModelTokenTests>(
                context,
                viewModel,
                model,
                silentlyContinueOnError,
                customResolverDelegate,
                null))
                .Should().Throw<ArgumentNullException>().WithParameterName("resolverDelegateModel");
        }

        [Fact]
        public void Can_Construct()
        {
            // Arrange
            var loggerMock = Fixture.Freeze<ILogger>();
            var tokenParserCallbackMock = Fixture.Freeze<ITokenParserCallback<ChildTemplateInitializeViewModelTokenTests>>();
            var context = SectionContext.FromSection
            (
                new Section("test.template", 1, "contents"),
                Mode.CodeCompositionRootFeature,
                Enumerable.Empty<ITemplateToken<ChildTemplateInitializeViewModelTokenTests>>(),
                tokenParserCallbackMock,
                this,
                loggerMock,
                Array.Empty<TemplateParameter>()
            );
            var viewModel = new ValueSpecifier("viewModel", true);
            var model = new ValueSpecifier("model", true);
            var silentlyContinueOnError = true;
            var customResolverDelegate = new ValueSpecifier("customResolverDelegate", true);
            var resolverDelegateModel = new ValueSpecifier("resolverDelegate", true);

            // Act
            var actual = new ChildTemplateInitializeViewModelToken<ChildTemplateInitializeViewModelTokenTests>(context,
                                                                                                               viewModel,
                                                                                                               model,
                                                                                                               silentlyContinueOnError,
                                                                                                               customResolverDelegate,
                                                                                                               resolverDelegateModel);

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
    }
}
