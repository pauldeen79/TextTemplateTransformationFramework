using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.Default
{
    public class TemplateCodeCompilerTests : TestBase
    {
        private readonly ICodeCompiler<TemplateCodeCompilerTests> _codeCompilerMock;
        private readonly ITemplateFactory<TemplateCodeCompilerTests> _templateFactoryMock;
        private readonly IAssemblyService _assemblyServiceMock;

        public TemplateCodeCompilerTests()
        {
            _templateFactoryMock = Fixture.Freeze<ITemplateFactory<TemplateCodeCompilerTests>>();
            _codeCompilerMock = Fixture.Freeze<ICodeCompiler<TemplateCodeCompilerTests>>();
            _assemblyServiceMock = Fixture.Freeze<IAssemblyService>();
            _assemblyServiceMock.LoadAssembly(Arg.Any<string>(), Arg.Any<AssemblyLoadContext>())
                                .Returns(x => x.ArgAt<AssemblyLoadContext>(1).LoadFromAssemblyName(new AssemblyName(x.ArgAt<string>(0))));
        }

        [Fact]
        public void Ctor_Throws_On_Null_Arguments()
        {
            ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(typeof(TemplateCodeCompiler<TemplateCodeCompilerTests>));
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
            sut.Invoking(x => x.Compile(new TextTemplateProcessorContext<TemplateCodeCompilerTests>(new AssemblyTemplate(GetType().Assembly.FullName, typeof(MyAssemblyTemplate).FullName, AssemblyLoadContext.Default), Array.Empty<TemplateParameter>(), Fixture.Freeze<ILogger>(), SectionContext<TemplateCodeCompilerTests>.Empty), null))
               .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Compile_Processes_AssemblyTemplate_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var actual = sut.Compile(new TextTemplateProcessorContext<TemplateCodeCompilerTests>(new AssemblyTemplate(GetType().Assembly.FullName, typeof(MyAssemblyTemplate).FullName, AssemblyLoadContext.Default), Array.Empty<TemplateParameter>(), Fixture.Freeze<ILogger>(), SectionContext<TemplateCodeCompilerTests>.Empty), new TemplateCodeOutput<TemplateCodeCompilerTests>(Enumerable.Empty<TemplateToken<TemplateCodeCompilerTests>>(), string.Empty, new TemplateCodeOutput<TemplateCodeCompilerTests>(Enumerable.Empty<ITemplateToken<TemplateCodeCompilerTests>>(), new CodeGeneratorResult(string.Empty, "C#", Enumerable.Empty<CompilerError>()), string.Empty, Enumerable.Empty<string>(), Enumerable.Empty<string>(), string.Empty, string.Empty)));

            // Assert
            actual.Should().NotBeNull();
            actual.Assembly.Should().NotBeNull();
            actual.Template.Should().BeOfType<MyAssemblyTemplate>();
        }

        private TemplateCodeCompiler<TemplateCodeCompilerTests> CreateSut() => new(
            _codeCompilerMock,
            _templateFactoryMock,
            _assemblyServiceMock);
    }
}
