using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using CrossCutting.Common.Testing;
using FluentAssertions;
using Moq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.Default
{
    public class TemplateCodeCompilerTests
    {
        private readonly Mock<ICodeCompiler<TemplateCodeCompilerTests>> _codeCompilerMock = new();
        private readonly Mock<ITemplateFactory<TemplateCodeCompilerTests>> _templateFactoryMock = new();
        private readonly Mock<IAssemblyService> _assemblyServiceMock = new();

        public TemplateCodeCompilerTests()
        {
            _assemblyServiceMock.Setup(x => x.LoadAssembly(It.IsAny<string>(), It.IsAny<AssemblyLoadContext>()))
                                .Returns<string, AssemblyLoadContext>((assemblyName, context) => context.LoadFromAssemblyName(new AssemblyName(assemblyName)));
        }

        [Fact]
        public void Ctor_Throws_On_Null_Arguments()
        {
            TestHelpers.ConstructorMustThrowArgumentNullException(typeof(TemplateCodeCompiler<TemplateCodeCompilerTests>));
        }

        [Fact]
        public void Compile_Throws_On_Null_Context()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Compile(null, new TemplateCodeOutput<TemplateCodeCompilerTests>(Enumerable.Empty<TemplateToken<TemplateCodeCompilerTests>>(), string.Empty, new TemplateCodeOutput<TemplateCodeCompilerTests>(Enumerable.Empty<ITemplateToken< TemplateCodeCompilerTests>>(), new CodeGeneratorResult(string.Empty, "C#", Enumerable.Empty<CompilerError>()), string.Empty, Enumerable.Empty<string>(), Enumerable.Empty<string>(), string.Empty, string.Empty))))
               .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Compile_Throws_On_Null_CodeOutput()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Compile(new TextTemplateProcessorContext<TemplateCodeCompilerTests>(new AssemblyTemplate(GetType().Assembly.FullName, typeof(MyAssemblyTemplate).FullName, AssemblyLoadContext.Default), Array.Empty<TemplateParameter>(), new Mock<ILogger>().Object, SectionContext<TemplateCodeCompilerTests>.Empty), null))
               .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Compile_Processes_AssemblyTemplate_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var actual = sut.Compile(new TextTemplateProcessorContext<TemplateCodeCompilerTests>(new AssemblyTemplate(GetType().Assembly.FullName, typeof(MyAssemblyTemplate).FullName, AssemblyLoadContext.Default), Array.Empty<TemplateParameter>(), new Mock<ILogger>().Object, SectionContext<TemplateCodeCompilerTests>.Empty), new TemplateCodeOutput<TemplateCodeCompilerTests>(Enumerable.Empty<TemplateToken<TemplateCodeCompilerTests>>(), string.Empty, new TemplateCodeOutput<TemplateCodeCompilerTests>(Enumerable.Empty<ITemplateToken<TemplateCodeCompilerTests>>(), new CodeGeneratorResult(string.Empty, "C#", Enumerable.Empty<CompilerError>()), string.Empty, Enumerable.Empty<string>(), Enumerable.Empty<string>(), string.Empty, string.Empty)));

            // Assert
            actual.Should().NotBeNull();
            actual.Assembly.Should().NotBeNull();
            actual.Template.Should().BeOfType<MyAssemblyTemplate>();
        }

        private TemplateCodeCompiler<TemplateCodeCompilerTests> CreateSut() => new(
            _codeCompilerMock.Object,
            _templateFactoryMock.Object,
            _assemblyServiceMock.Object);
    }
}
