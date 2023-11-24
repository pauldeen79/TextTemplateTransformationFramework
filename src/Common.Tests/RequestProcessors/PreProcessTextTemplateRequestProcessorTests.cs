using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.RequestProcessors;
using TextTemplateTransformationFramework.Common.Requests;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.RequestProcessors
{
    [ExcludeFromCodeCoverage]
    public class PreProcessTextTemplateRequestProcessorTests : TestBase
    {
        private PreProcessTextTemplateRequestProcessor<PreProcessTextTemplateRequestProcessorTests> CreateSut() => Fixture.Create<PreProcessTextTemplateRequestProcessor<PreProcessTextTemplateRequestProcessorTests>>();

        [Fact]
        public void Ctor_Throws_On_Null_TemplateOutputCreator()
        {
            // Arrange
            var action = new Action(() => _ = new PreProcessTextTemplateRequestProcessor<PreProcessTextTemplateRequestProcessorTests>(null));

            // Act & Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("templateOutputCreator");
        }

        [Fact]
        public void Process_Throws_On_Null_Request()
        {
            // Arrange
            var sut = CreateSut();
            var action = new Action(() => sut.Process(null));

            // Act & Assert
            action.Should().Throw<ArgumentNullException>().WithParameterName("request");
        }

        [Fact]
        public void Process_Returns_Correct_Result_On_Success()
        {
            // Arrange
            var contextMock = Fixture.Freeze<ITextTemplateProcessorContext<PreProcessTextTemplateRequestProcessorTests>>();
            var templateOutputCreatorMock = Fixture.Freeze<ITemplateOutputCreator<PreProcessTextTemplateRequestProcessorTests>>();
            templateOutputCreatorMock.Create(contextMock).Returns(new TemplateCodeOutput<PreProcessTextTemplateRequestProcessorTests>
            (
                Enumerable.Empty<ITemplateToken<PreProcessTextTemplateRequestProcessorTests>>(),
                new CodeGeneratorResult("code", "c#", Enumerable.Empty<CompilerError>()),
                "cs",
                Enumerable.Empty<string>(),
                Enumerable.Empty<string>(),
                "GeneratedClass",
                "C:\\Temp"
            ));
            var sut = CreateSut();
            var request = new PreProcessTextTemplateRequest<PreProcessTextTemplateRequestProcessorTests>(Array.Empty<TemplateParameter>(), contextMock);

            // Act
            var actual = sut.Process(request);

            // Assert
            actual.Should().NotBeNull();
            actual.SourceCode.Should().Be("code");
            actual.OutputExtension.Should().Be("cs");
        }

        [Fact]
        public void Process_Returns_Result_With_Exception_On_Exception()
        {
            // Arrange
            var contextMock = Fixture.Freeze<ITextTemplateProcessorContext<PreProcessTextTemplateRequestProcessorTests>>();
            var templateOutputCreatorMock = Fixture.Freeze<ITemplateOutputCreator<PreProcessTextTemplateRequestProcessorTests>>();
            templateOutputCreatorMock.Create(contextMock).Throws<ApplicationException>();
            var sut = CreateSut();
            var request = new PreProcessTextTemplateRequest<PreProcessTextTemplateRequestProcessorTests>(Array.Empty<TemplateParameter>(), contextMock);

            // Act
            var actual = sut.Process(request);

            // Assert
            actual.Should().NotBeNull();
            actual.Exception.Should().StartWith("System.ApplicationException: Error in the application.");
        }
    }
}
