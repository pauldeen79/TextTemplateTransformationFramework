using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Moq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.RequestProcessors;
using TextTemplateTransformationFramework.Common.Requests;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.RequestProcessors
{
    [ExcludeFromCodeCoverage]
    public class PreProcessTextTemplateRequestProcessorTests
    {
        private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());
        private PreProcessTextTemplateRequestProcessor<PreProcessTextTemplateRequestProcessorTests> CreateSut() => _fixture.Create<PreProcessTextTemplateRequestProcessor<PreProcessTextTemplateRequestProcessorTests>>();

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
            var contextMock = _fixture.Freeze<Mock<ITextTemplateProcessorContext<PreProcessTextTemplateRequestProcessorTests>>>();
            var templateOutputCreatorMock = _fixture.Freeze<Mock<ITemplateOutputCreator<PreProcessTextTemplateRequestProcessorTests>>>();
            templateOutputCreatorMock.Setup(x => x.Create(contextMock.Object)).Returns(new TemplateCodeOutput<PreProcessTextTemplateRequestProcessorTests>
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
            var request = new PreProcessTextTemplateRequest<PreProcessTextTemplateRequestProcessorTests>(new TextTemplate("<# template language=\"c#\"", "template.tt"), Array.Empty<TemplateParameter>(), contextMock.Object);

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
            var contextMock = _fixture.Freeze<Mock<ITextTemplateProcessorContext<PreProcessTextTemplateRequestProcessorTests>>>();
            var templateOutputCreatorMock = _fixture.Freeze<Mock<ITemplateOutputCreator<PreProcessTextTemplateRequestProcessorTests>>>();
            templateOutputCreatorMock.Setup(x => x.Create(contextMock.Object)).Throws<ApplicationException>();
            var sut = CreateSut();
            var request = new PreProcessTextTemplateRequest<PreProcessTextTemplateRequestProcessorTests>(new TextTemplate("<# template language=\"c#\"", "template.tt"), Array.Empty<TemplateParameter>(), contextMock.Object);

            // Act
            var actual = sut.Process(request);

            // Assert
            actual.Should().NotBeNull();
            actual.Exception.Should().StartWith("System.ApplicationException: Error in the application.");
        }
    }
}
