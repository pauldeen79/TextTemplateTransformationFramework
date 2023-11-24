using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.Requests;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.Default
{
    [ExcludeFromCodeCoverage]
    public class TextTemplateProcessorTests : TestBase
    {
        private readonly ILoggerFactory _loggerFactoryMock;
        private readonly IRequestProcessor<ProcessTextTemplateRequest<TextTemplateProcessorTests>, ProcessResult> _processTextTemplateProcessorMock;
        private readonly IRequestProcessor<PreProcessTextTemplateRequest<TextTemplateProcessorTests>, ProcessResult> _preProcessTextTemplateProcessorMock;
        private readonly IRequestProcessor<ExtractParametersFromTextTemplateRequest<TextTemplateProcessorTests>, ExtractParametersResult> _extractParametersFromTextTemplateProcessorMock;

        public TextTemplateProcessorTests()
        {
            _loggerFactoryMock = Fixture.Freeze<ILoggerFactory>();
            _processTextTemplateProcessorMock = Fixture.Freeze<IRequestProcessor<ProcessTextTemplateRequest<TextTemplateProcessorTests>, ProcessResult>>();
            _preProcessTextTemplateProcessorMock = Fixture.Freeze<IRequestProcessor<PreProcessTextTemplateRequest<TextTemplateProcessorTests>, ProcessResult>>();
            _extractParametersFromTextTemplateProcessorMock = Fixture.Freeze<IRequestProcessor<ExtractParametersFromTextTemplateRequest<TextTemplateProcessorTests>, ExtractParametersResult>>();
            _loggerFactoryMock.Create().Returns(Fixture.Freeze<ILogger>());
            _processTextTemplateProcessorMock
                .Process(Arg.Any<ProcessTextTemplateRequest<TextTemplateProcessorTests>>())
                .Returns(x => ProcessResult.Create(Enumerable.Empty<CompilerError>(), GetOutput(x.ArgAt<ProcessTextTemplateRequest<TextTemplateProcessorTests>>(0).Context)));
            _extractParametersFromTextTemplateProcessorMock
                .Process(Arg.Any<ExtractParametersFromTextTemplateRequest<TextTemplateProcessorTests>>())
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
            ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(typeof(TextTemplateProcessor<TextTemplateProcessorTests>));
        }

        [Fact]
        public void Process_Processes_AssemblyTemplate_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Process(new AssemblyTemplate(GetType().Assembly.FullName, typeof(MyAssemblyTemplate).FullName, AssemblyLoadContext.Default), [new TemplateParameter { Name = nameof(MyAssemblyTemplate.MyParameter), Value = "something" }]);

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
            var result = sut.ExtractParameters(new AssemblyTemplate(GetType().Assembly.FullName, typeof(MyAssemblyTemplate).FullName, AssemblyLoadContext.Default));

            // Assert
            result.Should().NotBeNull();
            result.CompilerErrors.Should().BeEmpty();
            result.Exception.Should().BeNull();
            result.Parameters.Should().ContainSingle();
        }
        private TextTemplateProcessor<TextTemplateProcessorTests> CreateSut() => new(
            _loggerFactoryMock,
            _processTextTemplateProcessorMock,
            _preProcessTextTemplateProcessorMock,
            _extractParametersFromTextTemplateProcessorMock);
    }

    [ExcludeFromCodeCoverage]
    public class MyAssemblyTemplate
    {
        public string MyParameter { get; set; }

        public override string ToString() => $"Hello from MyAssemblyTemplate. MyParameter is [{MyParameter}]";
    }
}
