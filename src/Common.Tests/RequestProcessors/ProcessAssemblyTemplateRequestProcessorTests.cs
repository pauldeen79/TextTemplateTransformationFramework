using System;
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
    public class ProcessAssemblyTemplateRequestProcessorTests
    {
        private readonly Mock<ITemplateCodeCompiler<ProcessAssemblyTemplateRequestProcessorTests>> _templateCodeCompilerMock = new();
        private readonly Mock<ITemplateOutputCreator<ProcessAssemblyTemplateRequestProcessorTests>> _templateOutputCreatorMock = new();
        private readonly Mock<ITemplateProcessor<ProcessAssemblyTemplateRequestProcessorTests>> _templateProcessorMock = new();
        private readonly Mock<ILogger> _loggerMock = new();

        public ProcessAssemblyTemplateRequestProcessorTests()
        {
            _templateCodeCompilerMock.Setup(x => x.Compile(It.IsAny<ITextTemplateProcessorContext<ProcessAssemblyTemplateRequestProcessorTests>>(), It.IsAny<TemplateCodeOutput<ProcessAssemblyTemplateRequestProcessorTests>>()))
                                     .Returns<ITextTemplateProcessorContext<ProcessAssemblyTemplateRequestProcessorTests>, TemplateCodeOutput<ProcessAssemblyTemplateRequestProcessorTests>>((context, codeOutput) =>
                                     {
                                         var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(context.AssemblyTemplate.AssemblyName));
                                         return TemplateCompilerOutput.Create(assembly, assembly.GetExportedTypes().FirstOrDefault(x => x.FullName == context.AssemblyTemplate.ClassName), Enumerable.Empty<CompilerError>(), string.Empty, string.Empty, Enumerable.Empty<ITemplateToken<ProcessAssemblyTemplateRequestProcessorTests>>());
                                     });
            _templateOutputCreatorMock.Setup(x => x.Create(It.IsAny<ITextTemplateProcessorContext<ProcessAssemblyTemplateRequestProcessorTests>>()))
                                      .Returns<ITextTemplateProcessorContext<ProcessAssemblyTemplateRequestProcessorTests>>(context => new TemplateCodeOutput<ProcessAssemblyTemplateRequestProcessorTests>(Enumerable.Empty<ITemplateToken<ProcessAssemblyTemplateRequestProcessorTests>>(), new CodeGeneratorResult(string.Empty, "C#", Enumerable.Empty<CompilerError>()), string.Empty, Enumerable.Empty<string>(), Enumerable.Empty<string>(), string.Empty, string.Empty));
            _templateProcessorMock.Setup(x => x.Process(It.IsAny<ITextTemplateProcessorContext<ProcessAssemblyTemplateRequestProcessorTests>>(), It.IsAny<TemplateCompilerOutput<ProcessAssemblyTemplateRequestProcessorTests>>()))
                                  .Returns<ITextTemplateProcessorContext<ProcessAssemblyTemplateRequestProcessorTests>, TemplateCompilerOutput<ProcessAssemblyTemplateRequestProcessorTests>>((context, templateCompilerOutput) => ProcessResult.Create(Enumerable.Empty<CompilerError>(),  Activator.CreateInstance(templateCompilerOutput.Assembly.GetExportedTypes().First(x => x.FullName == context.AssemblyTemplate.ClassName)).ToString()));
        }

        [Fact]
        public void Ctor_Throws_On_Null_Arguments()
        {
            TestHelpers.ConstructorMustThrowArgumentNullException(typeof(ProcessAssemblyTemplateRequestProcessor<ProcessAssemblyTemplateRequestProcessorTests>));
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
            var assemblyTemplate = new AssemblyTemplate(GetType().Assembly.FullName, typeof(MyAssemblyTemplate).FullName, string.Empty);
            var actual = sut.Process(new ProcessAssemblyTemplateRequest<ProcessAssemblyTemplateRequestProcessorTests>(Array.Empty<TemplateParameter>(), new TextTemplateProcessorContext<ProcessAssemblyTemplateRequestProcessorTests>(assemblyTemplate, Array.Empty<TemplateParameter>(), _loggerMock.Object, null)));

            // Assert
            actual.Should().NotBeNull();
            actual.CompilerErrors.Should().BeEmpty();
            actual.Exception.Should().BeNull();
            actual.Output.Should().NotBeEmpty();
        }

        private ProcessAssemblyTemplateRequestProcessor<ProcessAssemblyTemplateRequestProcessorTests> CreateSut()
            => new(_templateCodeCompilerMock.Object, _templateOutputCreatorMock.Object, _templateProcessorMock.Object);
    }
}
