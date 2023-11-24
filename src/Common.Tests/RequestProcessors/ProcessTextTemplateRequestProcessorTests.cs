using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using FluentAssertions;
using Moq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.RequestProcessors;
using TextTemplateTransformationFramework.Common.Requests;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.RequestProcessors
{
    public class ProcessTextTemplateRequestProcessorTests : TestBase
    {
        private readonly Mock<ITemplateCodeCompiler<ProcessTextTemplateRequestProcessorTests>> _templateCodeCompilerMock = new();
        private readonly Mock<ITemplateOutputCreator<ProcessTextTemplateRequestProcessorTests>> _templateOutputCreatorMock = new();
        private readonly Mock<ITemplateProcessor<ProcessTextTemplateRequestProcessorTests>> _templateProcessorMock = new();
        private readonly Mock<ILogger> _loggerMock = new();

        public ProcessTextTemplateRequestProcessorTests()
        {
            _templateCodeCompilerMock.Setup(x => x.Compile(It.IsAny<ITextTemplateProcessorContext<ProcessTextTemplateRequestProcessorTests>>(), It.IsAny<TemplateCodeOutput<ProcessTextTemplateRequestProcessorTests>>()))
                                     .Returns<ITextTemplateProcessorContext<ProcessTextTemplateRequestProcessorTests>, TemplateCodeOutput<ProcessTextTemplateRequestProcessorTests>>((context, codeOutput) =>
                                     {
                                         var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(context.AssemblyTemplate.AssemblyName));
                                         return TemplateCompilerOutput.Create(assembly, Array.Find(assembly.GetExportedTypes(), x => x.FullName == context.AssemblyTemplate.ClassName), Enumerable.Empty<CompilerError>(), string.Empty, string.Empty, Enumerable.Empty<ITemplateToken<ProcessTextTemplateRequestProcessorTests>>());
                                     });
            _templateOutputCreatorMock.Setup(x => x.Create(It.IsAny<ITextTemplateProcessorContext<ProcessTextTemplateRequestProcessorTests>>()))
                                      .Returns<ITextTemplateProcessorContext<ProcessTextTemplateRequestProcessorTests>>(context => new TemplateCodeOutput<ProcessTextTemplateRequestProcessorTests>(Enumerable.Empty<ITemplateToken<ProcessTextTemplateRequestProcessorTests>>(), new CodeGeneratorResult(string.Empty, "C#", Enumerable.Empty<CompilerError>()), string.Empty, Enumerable.Empty<string>(), Enumerable.Empty<string>(), string.Empty, string.Empty));
            _templateProcessorMock.Setup(x => x.Process(It.IsAny<ITextTemplateProcessorContext<ProcessTextTemplateRequestProcessorTests>>(), It.IsAny<TemplateCompilerOutput<ProcessTextTemplateRequestProcessorTests>>()))
                                  .Returns<ITextTemplateProcessorContext<ProcessTextTemplateRequestProcessorTests>, TemplateCompilerOutput<ProcessTextTemplateRequestProcessorTests>>((context, templateCompilerOutput) => ProcessResult.Create(Enumerable.Empty<CompilerError>(),  Activator.CreateInstance(templateCompilerOutput.Assembly.GetExportedTypes().First(x => x.FullName == context.AssemblyTemplate.ClassName)).ToString()));
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
            var actual = sut.Process(new ProcessTextTemplateRequest<ProcessTextTemplateRequestProcessorTests>(Array.Empty<TemplateParameter>(), new TextTemplateProcessorContext<ProcessTextTemplateRequestProcessorTests>(TextTemplate, Array.Empty<TemplateParameter>(), _loggerMock.Object, null)));

            // Assert
            actual.Should().NotBeNull();
            actual.CompilerErrors.Should().BeEmpty();
            actual.Exception.Should().BeNull();
            actual.Output.Should().NotBeEmpty();
        }

        private ProcessTextTemplateRequestProcessor<ProcessTextTemplateRequestProcessorTests> CreateSut()
            => new(_templateOutputCreatorMock.Object, _templateCodeCompilerMock.Object, _templateProcessorMock.Object);
    }
}
