using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using CrossCutting.Common.Testing;
using FluentAssertions;
using Moq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.Requests;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.Default
{
    [ExcludeFromCodeCoverage]
    public class TextTemplateProcessorTests
    {
        private readonly Mock<ILoggerFactory> _loggerFactoryMock = new();
        private readonly Mock<IRequestProcessor<ProcessTextTemplateRequest<TextTemplateProcessorTests>, ProcessResult>> _processTextTemplateProcessorMock = new();
        private readonly Mock<IRequestProcessor<PreProcessTextTemplateRequest<TextTemplateProcessorTests>, ProcessResult>> _preProcessTextTemplateProcessorMock = new();
        private readonly Mock<IRequestProcessor<ExtractParametersFromTextTemplateRequest<TextTemplateProcessorTests>, ExtractParametersResult>> _extractParametersFromTextTemplateProcessorMock = new();

        public TextTemplateProcessorTests()
        {
            _loggerFactoryMock.Setup(x => x.Create()).Returns(new Mock<ILogger>().Object);
            _processTextTemplateProcessorMock
                .Setup(x => x.Process(It.IsAny<ProcessTextTemplateRequest<TextTemplateProcessorTests>>()))
                .Returns<ProcessTextTemplateRequest<TextTemplateProcessorTests>>(request => ProcessResult.Create(Enumerable.Empty<CompilerError>(), GetOutput(request.Context)));
            _extractParametersFromTextTemplateProcessorMock
                .Setup(x => x.Process(It.IsAny<ExtractParametersFromTextTemplateRequest<TextTemplateProcessorTests>>()))
                .Returns(ExtractParametersResult.Create(new[] { new TemplateParameter { Name = nameof(MyAssemblyTemplate.MyParameter), Type = typeof(string) } }, Array.Empty<CompilerError>(), string.Empty, string.Empty));
        }

        private string GetOutput(ITextTemplateProcessorContext<TextTemplateProcessorTests> context)
        {
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(context.AssemblyTemplate.AssemblyName));
            var templateType = assembly.GetExportedTypes().First(x => x.FullName == context.AssemblyTemplate.ClassName);
            var template = (MyAssemblyTemplate)Activator.CreateInstance(templateType);
            template.MyParameter = context.Parameters.First(x => x.Name == nameof(MyAssemblyTemplate.MyParameter)).Value?.ToString();
            return template.ToString() ?? "Error: Could not instanciate template";
        }

        [Fact]
        public void Ctor_Throws_On_Null_Arguments()
        {
            TestHelpers.ConstructorMustThrowArgumentNullException(typeof(TextTemplateProcessor<TextTemplateProcessorTests>));
        }

        [Fact]
        public void Process_Processes_AssemblyTemplate_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Process(new AssemblyTemplate(GetType().Assembly.FullName, typeof(MyAssemblyTemplate).FullName), new[] { new TemplateParameter { Name = nameof(MyAssemblyTemplate.MyParameter), Value = "something" } });

            // Assert
            result.Should().NotBeNull();
            result.CompilerErrors.Should().BeEmpty();
            result.Exception.Should().BeNull();
            result.Output.Should().Be("Hello from MyAssemblyTemplate. MyParameter is [something]");
        }

        [Fact]
        public void ExtractParameters_Processes_AssemblyTemplate_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.ExtractParameters(new AssemblyTemplate(GetType().Assembly.FullName, typeof(MyAssemblyTemplate).FullName));

            // Assert
            result.Should().NotBeNull();
            result.CompilerErrors.Should().BeEmpty();
            result.Exception.Should().BeNull();
            result.Parameters.Should().ContainSingle();
        }
        private TextTemplateProcessor<TextTemplateProcessorTests> CreateSut() => new(
            _loggerFactoryMock.Object,
            _processTextTemplateProcessorMock.Object,
            _preProcessTextTemplateProcessorMock.Object,
            _extractParametersFromTextTemplateProcessorMock.Object);
    }

    [ExcludeFromCodeCoverage]
    public class MyAssemblyTemplate
    {
        public string MyParameter { get; set; }

        public override string ToString() => $"Hello from MyAssemblyTemplate. MyParameter is [{MyParameter}]";
    }
}
