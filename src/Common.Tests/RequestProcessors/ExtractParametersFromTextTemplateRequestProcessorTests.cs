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
using TextTemplateTransformationFramework.Common.RequestProcessors;
using TextTemplateTransformationFramework.Common.Requests;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.RequestProcessors
{
    public class ExtractParametersFromTextTemplateRequestProcessorTests
    {
        private readonly Mock<ITemplateCodeCompiler<ExtractParametersFromTextTemplateRequestProcessorTests>> _templateCodeCompilerMock = new();
        private readonly Mock<ITemplateOutputCreator<ExtractParametersFromTextTemplateRequestProcessorTests>> _templateOutputCreatorMock = new();
        private readonly Mock<ITextTemplateParameterExtractor<ExtractParametersFromTextTemplateRequestProcessorTests>> _templateParameterExtractorMock = new();
        private readonly Mock<ILogger> _loggerMock = new();

        public ExtractParametersFromTextTemplateRequestProcessorTests()
        {
            _templateCodeCompilerMock.Setup(x => x.Compile(It.IsAny<ITextTemplateProcessorContext<ExtractParametersFromTextTemplateRequestProcessorTests>>(), It.IsAny<TemplateCodeOutput<ExtractParametersFromTextTemplateRequestProcessorTests>>()))
                                     .Returns<ITextTemplateProcessorContext<ExtractParametersFromTextTemplateRequestProcessorTests>, TemplateCodeOutput<ExtractParametersFromTextTemplateRequestProcessorTests>>((context, codeOutput) =>
                                     {
                                         var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(context.AssemblyTemplate.AssemblyName));
                                         return TemplateCompilerOutput.Create(assembly, assembly.GetExportedTypes().FirstOrDefault(x => x.FullName == context.AssemblyTemplate.ClassName), Enumerable.Empty<CompilerError>(), string.Empty, string.Empty, Enumerable.Empty<ITemplateToken<ExtractParametersFromTextTemplateRequestProcessorTests>>());
                                     });
            _templateOutputCreatorMock.Setup(x => x.Create(It.IsAny<ITextTemplateProcessorContext<ExtractParametersFromTextTemplateRequestProcessorTests>>()))
                                      .Returns<ITextTemplateProcessorContext<ExtractParametersFromTextTemplateRequestProcessorTests>>(context => new TemplateCodeOutput<ExtractParametersFromTextTemplateRequestProcessorTests>(Enumerable.Empty<ITemplateToken<ExtractParametersFromTextTemplateRequestProcessorTests>>(), new CodeGeneratorResult(string.Empty, "C#", Enumerable.Empty<CompilerError>()), string.Empty, Enumerable.Empty<string>(), Enumerable.Empty<string>(), string.Empty, string.Empty));
            _templateParameterExtractorMock.Setup(x => x.Extract(It.IsAny<ITextTemplateProcessorContext<ExtractParametersFromTextTemplateRequestProcessorTests>>(), It.IsAny<TemplateCompilerOutput<ExtractParametersFromTextTemplateRequestProcessorTests>>()))
                                           .Returns<ITextTemplateProcessorContext<ExtractParametersFromTextTemplateRequestProcessorTests>, TemplateCompilerOutput<ExtractParametersFromTextTemplateRequestProcessorTests>>((context, templateCompilerOutput) => new[] { new TemplateParameter() });
        }

        [Fact]
        public void Ctor_Throws_On_Null_Arguments()
        {
            TestHelpers.ConstructorMustThrowArgumentNullException(typeof(ExtractParametersFromTextTemplateRequestProcessor<ExtractParametersFromTextTemplateRequestProcessorTests>));
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
            var assemblyTemplate = new AssemblyTemplate(GetType().Assembly.FullName, typeof(MyAssemblyTemplate).FullName, AssemblyLoadContext.Default);
            var actual = sut.Process(new ExtractParametersFromTextTemplateRequest<ExtractParametersFromTextTemplateRequestProcessorTests>(new TextTemplateProcessorContext<ExtractParametersFromTextTemplateRequestProcessorTests>(assemblyTemplate, Array.Empty<TemplateParameter>(), _loggerMock.Object, null)));

            // Assert
            actual.Should().NotBeNull();
            actual.Parameters.Should().ContainSingle();
        }

        private ExtractParametersFromTextTemplateRequestProcessor<ExtractParametersFromTextTemplateRequestProcessorTests> CreateSut()
            => new(_templateCodeCompilerMock.Object, _templateParameterExtractorMock.Object, _templateOutputCreatorMock.Object);
    }

    [ExcludeFromCodeCoverage]
    public class MyAssemblyTemplate
    {
    }
}
