﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using CrossCutting.Common.Testing;
using FluentAssertions;
using Moq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.RequestProcessors;
using TextTemplateTransformationFramework.Common.Requests;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.RequestProcessors
{
    public class ExtractParametersFromAssemblyTemplateRequestProcessorTests
    {
        private readonly Mock<ITemplateCodeCompiler<ExtractParametersFromAssemblyTemplateRequestProcessorTests>> _templateCodeCompilerMock = new();
        private readonly Mock<ITemplateOutputCreator<ExtractParametersFromAssemblyTemplateRequestProcessorTests>> _templateOutputCreatorMock = new();
        private readonly Mock<ITextTemplateParameterExtractor<ExtractParametersFromAssemblyTemplateRequestProcessorTests>> _templateParameterExtractorMock = new();
        private readonly Mock<ILogger> _loggerMock = new();

        public ExtractParametersFromAssemblyTemplateRequestProcessorTests()
        {
            _templateCodeCompilerMock.Setup(x => x.Compile(It.IsAny<ITextTemplateProcessorContext<ExtractParametersFromAssemblyTemplateRequestProcessorTests>>(), It.IsAny<TemplateCodeOutput<ExtractParametersFromAssemblyTemplateRequestProcessorTests>>()))
                                     .Returns<ITextTemplateProcessorContext<ExtractParametersFromAssemblyTemplateRequestProcessorTests>, TemplateCodeOutput<ExtractParametersFromAssemblyTemplateRequestProcessorTests>>((context, codeOutput) =>
                                     {
                                         var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(context.AssemblyTemplate.AssemblyName));
                                         return TemplateCompilerOutput.Create(assembly, assembly.GetExportedTypes().FirstOrDefault(x => x.FullName == context.AssemblyTemplate.ClassName), Enumerable.Empty<CompilerError>(), string.Empty, string.Empty, Enumerable.Empty<ITemplateToken<ExtractParametersFromAssemblyTemplateRequestProcessorTests>>());
                                     });
            _templateOutputCreatorMock.Setup(x => x.Create(It.IsAny<ITextTemplateProcessorContext<ExtractParametersFromAssemblyTemplateRequestProcessorTests>>()))
                                      .Returns<ITextTemplateProcessorContext<ExtractParametersFromAssemblyTemplateRequestProcessorTests>>(context => new TemplateCodeOutput<ExtractParametersFromAssemblyTemplateRequestProcessorTests>(Enumerable.Empty<ITemplateToken<ExtractParametersFromAssemblyTemplateRequestProcessorTests>>(), new CodeGeneratorResult(string.Empty, "C#", Enumerable.Empty<CompilerError>()), string.Empty, Enumerable.Empty<string>(), Enumerable.Empty<string>(), string.Empty, string.Empty));
            _templateParameterExtractorMock.Setup(x => x.Extract(It.IsAny<ITextTemplateProcessorContext<ExtractParametersFromAssemblyTemplateRequestProcessorTests>>(), It.IsAny<TemplateCompilerOutput<ExtractParametersFromAssemblyTemplateRequestProcessorTests>>()))
                                           .Returns<ITextTemplateProcessorContext<ExtractParametersFromAssemblyTemplateRequestProcessorTests>, TemplateCompilerOutput<ExtractParametersFromAssemblyTemplateRequestProcessorTests>>((context, templateCompilerOutput) => new[] { new TemplateParameter() });
        }

        [Fact]
        public void Ctor_Throws_On_Null_Arguments()
        {
            TestHelpers.ConstructorMustThrowArgumentNullException(typeof(ExtractParametersFromAssemblyTemplateRequestProcessor<ExtractParametersFromAssemblyTemplateRequestProcessorTests>));
        }

        [Fact]
        public void Process_Throws_On_Null_Argument()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Process(null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Process_Returns_Correct_Template_Parameters()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var assemblyTemplate = new AssemblyTemplate(GetType().Assembly.FullName, typeof(MyAssemblyTemplate).FullName, string.Empty);
            var actual = sut.Process(new ExtractParametersFromAssemblyTemplateRequest<ExtractParametersFromAssemblyTemplateRequestProcessorTests>(new TextTemplateProcessorContext<ExtractParametersFromAssemblyTemplateRequestProcessorTests>(assemblyTemplate, Array.Empty<TemplateParameter>(), _loggerMock.Object, null)));

            // Assert
            actual.Should().NotBeNull();
            actual.Parameters.Should().ContainSingle();
        }

        private ExtractParametersFromAssemblyTemplateRequestProcessor<ExtractParametersFromAssemblyTemplateRequestProcessorTests> CreateSut()
            => new(_templateCodeCompilerMock.Object, _templateOutputCreatorMock.Object, _templateParameterExtractorMock.Object);
    }

    [ExcludeFromCodeCoverage]
    public class MyAssemblyTemplate
    {
    }
}
