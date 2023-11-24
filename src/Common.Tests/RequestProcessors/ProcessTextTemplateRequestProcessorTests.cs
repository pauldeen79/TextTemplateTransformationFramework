using System;
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
    public class ProcessTextTemplateRequestProcessorTests : TestBase
    {
        private readonly ITemplateCodeCompiler<ProcessTextTemplateRequestProcessorTests> _templateCodeCompilerMock;
        private readonly ITemplateOutputCreator<ProcessTextTemplateRequestProcessorTests> _templateOutputCreatorMock;
        private readonly ITemplateProcessor<ProcessTextTemplateRequestProcessorTests> _templateProcessorMock;
        private readonly ILogger _loggerMock;

        public ProcessTextTemplateRequestProcessorTests()
        {
            _templateCodeCompilerMock = Fixture.Freeze<ITemplateCodeCompiler<ProcessTextTemplateRequestProcessorTests>>();
            _templateOutputCreatorMock = Fixture.Freeze<ITemplateOutputCreator<ProcessTextTemplateRequestProcessorTests>>();
            _templateProcessorMock = Fixture.Freeze<ITemplateProcessor<ProcessTextTemplateRequestProcessorTests>>();
            _loggerMock = Fixture.Freeze<ILogger>();

            _templateCodeCompilerMock.Compile(Arg.Any<ITextTemplateProcessorContext<ProcessTextTemplateRequestProcessorTests>>(), Arg.Any<TemplateCodeOutput<ProcessTextTemplateRequestProcessorTests>>())
                                     .Returns(x =>
                                     {
                                         var context = x.ArgAt<ITextTemplateProcessorContext<ProcessTextTemplateRequestProcessorTests>>(0);
                                         var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(context.AssemblyTemplate.AssemblyName));
                                         return TemplateCompilerOutput.Create(assembly, Array.Find(assembly.GetExportedTypes(), x => x.FullName == context.AssemblyTemplate.ClassName), Enumerable.Empty<CompilerError>(), string.Empty, string.Empty, Enumerable.Empty<ITemplateToken<ProcessTextTemplateRequestProcessorTests>>());
                                     });

            _templateOutputCreatorMock.Create(Arg.Any<ITextTemplateProcessorContext<ProcessTextTemplateRequestProcessorTests>>())
                                      .Returns(new TemplateCodeOutput<ProcessTextTemplateRequestProcessorTests>(Enumerable.Empty<ITemplateToken<ProcessTextTemplateRequestProcessorTests>>(), new CodeGeneratorResult(string.Empty, "C#", Enumerable.Empty<CompilerError>()), string.Empty, Enumerable.Empty<string>(), Enumerable.Empty<string>(), string.Empty, string.Empty));

            _templateProcessorMock.Process(Arg.Any<ITextTemplateProcessorContext<ProcessTextTemplateRequestProcessorTests>>(), Arg.Any<TemplateCompilerOutput<ProcessTextTemplateRequestProcessorTests>>())
                                  .Returns(x => ProcessResult.Create(Enumerable.Empty<CompilerError>(), Activator.CreateInstance(x.ArgAt<TemplateCompilerOutput<ProcessTextTemplateRequestProcessorTests>>(1).Assembly.GetExportedTypes().First(y => y.FullName == x.ArgAt<ITextTemplateProcessorContext<ProcessTextTemplateRequestProcessorTests>>(0).AssemblyTemplate.ClassName)).ToString()));
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
        public void Process_Returns_Correct_Template_Output()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var TextTemplate = new AssemblyTemplate(GetType().Assembly.FullName, typeof(MyAssemblyTemplate).FullName, AssemblyLoadContext.Default);
            var actual = sut.Process(new ProcessTextTemplateRequest<ProcessTextTemplateRequestProcessorTests>(Array.Empty<TemplateParameter>(), new TextTemplateProcessorContext<ProcessTextTemplateRequestProcessorTests>(TextTemplate, Array.Empty<TemplateParameter>(), _loggerMock, null)));

            // Assert
            actual.Should().NotBeNull();
            actual.CompilerErrors.Should().BeEmpty();
            actual.Exception.Should().BeNull();
            actual.Output.Should().NotBeEmpty();
        }

        private ProcessTextTemplateRequestProcessor<ProcessTextTemplateRequestProcessorTests> CreateSut()
            => new(_templateOutputCreatorMock, _templateCodeCompilerMock, _templateProcessorMock);
    }
}
