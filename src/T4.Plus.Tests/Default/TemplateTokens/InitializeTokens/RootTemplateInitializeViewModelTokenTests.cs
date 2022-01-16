using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Moq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.Default.TemplateTokens.InitializeTokens
{
    [ExcludeFromCodeCoverage]
    public class RootTemplateInitializeViewModelTokenTests
    {
        [Fact]
        public void Can_Construct()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var tokenParserCallbackMock = new Mock<ITokenParserCallback<RootTemplateInitializeViewModelTokenTests>>();
            var context = SectionContext.FromSection
            (
                new Section("test.template", 1, "contents"),
                Mode.CodeCompositionRootFeature,
                Enumerable.Empty<ITemplateToken<RootTemplateInitializeViewModelTokenTests>>(),
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
            var actual = new RootTemplateInitializeViewModelToken<RootTemplateInitializeViewModelTokenTests>(context,
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
