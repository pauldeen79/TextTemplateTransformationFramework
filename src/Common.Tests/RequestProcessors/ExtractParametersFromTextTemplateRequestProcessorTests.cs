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
using TextTemplateTransformationFramework.Common.RequestProcessors;
using TextTemplateTransformationFramework.Common.Requests;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.RequestProcessors
{
    public class ExtractParametersFromTextTemplateRequestProcessorTests : TestBase
    {
        private readonly ITemplateCodeCompiler<ExtractParametersFromTextTemplateRequestProcessorTests> _templateCodeCompilerMock;
        private readonly ITemplateOutputCreator<ExtractParametersFromTextTemplateRequestProcessorTests> _templateOutputCreatorMock;
        private readonly ITextTemplateParameterExtractor<ExtractParametersFromTextTemplateRequestProcessorTests> _templateParameterExtractorMock;
        private readonly ILogger _loggerMock;

        public ExtractParametersFromTextTemplateRequestProcessorTests()
        {
            _templateCodeCompilerMock = Fixture.Freeze<ITemplateCodeCompiler<ExtractParametersFromTextTemplateRequestProcessorTests>>();
            _templateOutputCreatorMock = Fixture.Freeze<ITemplateOutputCreator<ExtractParametersFromTextTemplateRequestProcessorTests>>();
            _templateParameterExtractorMock = Fixture.Freeze<ITextTemplateParameterExtractor<ExtractParametersFromTextTemplateRequestProcessorTests>>();
            _loggerMock = Fixture.Freeze<ILogger>();

            _templateCodeCompilerMock.Compile(Arg.Any<ITextTemplateProcessorContext<ExtractParametersFromTextTemplateRequestProcessorTests>>(), Arg.Any<TemplateCodeOutput<ExtractParametersFromTextTemplateRequestProcessorTests>>())
                                     .Returns(x =>
                                     {
                                         var context = x.ArgAt<ITextTemplateProcessorContext<ExtractParametersFromTextTemplateRequestProcessorTests>>(0);
                                         var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(context.AssemblyTemplate.AssemblyName));
                                         return TemplateCompilerOutput.Create(assembly, Array.Find(assembly.GetExportedTypes(), x => x.FullName == context.AssemblyTemplate.ClassName), Enumerable.Empty<CompilerError>(), string.Empty, string.Empty, Enumerable.Empty<ITemplateToken<ExtractParametersFromTextTemplateRequestProcessorTests>>());
                                     });

            _templateOutputCreatorMock.Create(Arg.Any<ITextTemplateProcessorContext<ExtractParametersFromTextTemplateRequestProcessorTests>>())
                                      .Returns(x => new TemplateCodeOutput<ExtractParametersFromTextTemplateRequestProcessorTests>(Enumerable.Empty<ITemplateToken<ExtractParametersFromTextTemplateRequestProcessorTests>>(), new CodeGeneratorResult(string.Empty, "C#", Enumerable.Empty<CompilerError>()), string.Empty, Enumerable.Empty<string>(), Enumerable.Empty<string>(), string.Empty, string.Empty));

            _templateParameterExtractorMock.Extract(Arg.Any<ITextTemplateProcessorContext<ExtractParametersFromTextTemplateRequestProcessorTests>>(), Arg.Any<TemplateCompilerOutput<ExtractParametersFromTextTemplateRequestProcessorTests>>())
                                           .Returns(new[] { new TemplateParameter() });
        }

        [Fact]
        public void Ctor_Throws_On_Null_Arguments()
        {
            ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(typeof(ExtractParametersFromTextTemplateRequestProcessor<ExtractParametersFromTextTemplateRequestProcessorTests>));
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
            var actual = sut.Process(new ExtractParametersFromTextTemplateRequest<ExtractParametersFromTextTemplateRequestProcessorTests>(new TextTemplateProcessorContext<ExtractParametersFromTextTemplateRequestProcessorTests>(assemblyTemplate, Array.Empty<TemplateParameter>(), _loggerMock, null)));

            // Assert
            actual.Should().NotBeNull();
            actual.Parameters.Should().ContainSingle();
        }

        private ExtractParametersFromTextTemplateRequestProcessor<ExtractParametersFromTextTemplateRequestProcessorTests> CreateSut()
            => new(_templateCodeCompilerMock, _templateParameterExtractorMock, _templateOutputCreatorMock);
    }

    [ExcludeFromCodeCoverage]
    public class MyAssemblyTemplate
    {
    }
}
