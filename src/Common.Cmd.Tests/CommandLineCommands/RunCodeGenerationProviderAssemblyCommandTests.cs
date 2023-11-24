using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
using Moq;
using TextCopy;
using TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands;
using TextTemplateTransformationFramework.Common.Cmd.Tests.TestFixtures;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Runtime;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Cmd.Tests.CommandLineCommands
{
    [ExcludeFromCodeCoverage]
    public class RunCodeGenerationProviderAssemblyCommandTests : TestBase
    {
        private readonly Mock<IClipboard> _clipboardMock;
        private readonly Mock<IAssemblyService> _assemblyServiceMock;

        private RunCodeGenerationProviderAssemblyCommand CreateSut() => new RunCodeGenerationProviderAssemblyCommand(_clipboardMock.Object, _assemblyServiceMock.Object);

        public RunCodeGenerationProviderAssemblyCommandTests()
        {
            _clipboardMock = new Mock<IClipboard>();
            _assemblyServiceMock = new Mock<IAssemblyService>();
            _assemblyServiceMock.Setup(x => x.LoadAssembly(It.IsAny<string>(), It.IsAny<AssemblyLoadContext>()))
                                .Returns<string, AssemblyLoadContext>((name, ctx) => name.EndsWith(".dll") ? ctx.LoadFromAssemblyPath(FullyQualify(name)) : ctx.LoadFromAssemblyName(new AssemblyName(name)));
        }

        private static string FullyQualify(string name)
            => Path.IsPathFullyQualified(name)
                ? name
                : Path.Combine(Directory.GetCurrentDirectory(), name);

        [Fact]
        public void Ctor_Throws_On_Null_Argument()
        {
            ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(typeof(RunCodeGenerationProviderAssemblyCommand));
        }

        [Fact]
        public void Initialize_Adds_Command_To_Application()
        {
            // Arrange
            var app = new CommandLineApplication();
            var sut = CreateSut();

            // Act
            sut.Initialize(app);

            // Assert
            app.Commands.Should().HaveCount(1);
        }

        [Fact]
        public void Initialize_Throws_On_Null_Argument()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Initialize(null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Execute_Without_Assembly_Leads_To_Error()
        {
            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut);

            // Assert
            actual.Should().Be("Error: Assembly name is required." + Environment.NewLine);
        }

        [Fact]
        public void Execute_With_Path_Option_Saves_Output_From_TemplateFileManager()
        {
            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, $"-a {GetType().Assembly.FullName}", $"-p {Directory.GetCurrentDirectory()}");

            // Assert
            actual.Should().Be($@"Written code generation output to path: {Directory.GetCurrentDirectory()}
");
        }

#if Windows
        [Fact]
        public void Execute_With_Path_Option_Saves_Output_From_TemplateFileManager_Partial_AssemblyFileName()
        {
            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, $"-a TextTemplateTransformationFramework.Common.Cmd.Tests.dll", $"-p {Directory.GetCurrentDirectory()}");

            // Assert
            actual.Should().Be($@"Written code generation output to path: {Directory.GetCurrentDirectory()}
");
        }

        [Fact]
        public void Execute_With_Path_Option_Saves_Output_From_TemplateFileManager_Partial_AssemblyFileName_And_HintPath()
        {
        // Act
        var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, $"-a TextTemplateTransformationFramework.Common.Cmd.Tests.dll", $"-p {Directory.GetCurrentDirectory()}", $"-u {Directory.GetCurrentDirectory()}");

        // Assert
        actual.Should().Be($@"Written code generation output to path: {Directory.GetCurrentDirectory()}
");
    }
#endif

        [Fact]
        public void Execute_With_Path_Option_Saves_Output_From_TemplateFileManager_Fully_Qualified_AssemblyFileName()
        {
            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, $"-a {GetType().Assembly.Location}", $"-p {Directory.GetCurrentDirectory()}");

            // Assert
            actual.Should().Be($@"Written code generation output to path: {Directory.GetCurrentDirectory()}
");
        }

        [Fact]
        public void Execute_With_Clipboard_Option_Saves_Output_To_Clipboard()
        {
            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, $"-a {GetType().Assembly.FullName}", "-c");

            // Assert
            actual.Should().Be(@"Copied code generation output to clipboard
");
            var content = new MultipleContentBuilder().ToString();
            _clipboardMock.Verify(x => x.SetText(content), Times.Once);
        }

        [Fact]
        public void Execute_Without_Path_And_Clipboard_Options_Writes_Output_To_Host()
        {
            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, $"-a {GetType().Assembly.FullName}");

            // Assert
            actual.Should().Be(@$"Code generation output:
{new MultipleContentBuilder()}
");
        }

        [Fact]
        public void Execute_With_Filter_Filters_CodeGenerationProviders_Correctly()
        {
            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, $"-a {GetType().Assembly.FullName}", "--filter SomeName");

            // Assert
            actual.Should().Be(@$"Code generation output:
{new MultipleContentBuilder()}
");
        }
    }
}
