using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Mime;
using FluentAssertions;
using Moq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.ChildTemplateInitializeCodeTokens;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.Default.TemplateTokens.ChildTemplateInitializeCodeTokens
{
    [ExcludeFromCodeCoverage]
    public class ChildTemplateInitializeViewModelTokenTests
    {
        [Fact]
        public void Ctor_Throws_On_Null_ViewModel()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var tokenParserCallbackMock = new Mock<ITokenParserCallback<ChildTemplateInitializeViewModelTokenTests>>();
            var context = SectionContext.FromSection
            (
                new Section("test.template", 1, "contents"),
                Mode.CodeCompositionRootFeature,
                Enumerable.Empty<ITemplateToken<ChildTemplateInitializeViewModelTokenTests>>(),
                tokenParserCallbackMock.Object,
                this,
                loggerMock.Object,
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
            var loggerMock = new Mock<ILogger>();
            var tokenParserCallbackMock = new Mock<ITokenParserCallback<ChildTemplateInitializeViewModelTokenTests>>();
            var context = SectionContext.FromSection
            (
                new Section("test.template", 1, "contents"),
                Mode.CodeCompositionRootFeature,
                Enumerable.Empty<ITemplateToken<ChildTemplateInitializeViewModelTokenTests>>(),
                tokenParserCallbackMock.Object,
                this,
                loggerMock.Object,
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
        public void Can_Construct()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var tokenParserCallbackMock = new Mock<ITokenParserCallback<ChildTemplateInitializeViewModelTokenTests>>();
            var context = SectionContext.FromSection
            (
                new Section("test.template", 1, "contents"),
                Mode.CodeCompositionRootFeature,
                Enumerable.Empty<ITemplateToken<ChildTemplateInitializeViewModelTokenTests>>(),
                tokenParserCallbackMock.Object,
                this,
                loggerMock.Object,
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
