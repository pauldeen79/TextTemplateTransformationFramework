using System;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Common.Testing;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
using Moq;
using TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands;
using TextTemplateTransformationFramework.Common.Cmd.Tests.TestFixtures;
using TextTemplateTransformationFramework.Common.Contracts;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Cmd.Tests.CommandLineCommands
{
    [ExcludeFromCodeCoverage]
    public class RunTemplateCommandTests
    {
        private readonly Mock<ITextTemplateProcessor> _processorMock;
        private readonly Mock<IFileContentsProvider> _fileContentsProviderMock;

        private RunTemplateCommand CreateSut() => new RunTemplateCommand(_processorMock.Object, _fileContentsProviderMock.Object);

        public RunTemplateCommandTests()
        {
            _processorMock = new Mock<ITextTemplateProcessor>();
            _fileContentsProviderMock = new Mock<IFileContentsProvider>();
        }

        [Fact]
        public void Ctor_Throws_On_Null_Argument()
        {
            TestHelpers.ConstructorMustThrowArgumentNullException(typeof(RunTemplateCommand));
        }

        [Fact]
        public void Initialize_Adds_Command_To_Application()
        {
            // Arrange
            var app = new CommandLineApplication();
            var textTemplateProcessorMock = new Mock<ITextTemplateProcessor>();
            var fileContentsProviderMock = new Mock<IFileContentsProvider>();
            var sut = new RunTemplateCommand(textTemplateProcessorMock.Object, fileContentsProviderMock.Object);

            // Act
            sut.Initialize(app);

            // Assert
            app.Commands.Should().HaveCount(1);
        }

        [Fact]
        public void Execute_Without_Filename_Leads_To_Error()
        {
            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut);

            // Assert
            actual.Should().Be("Error: Filename is required." + Environment.NewLine);
        }

        [Fact]
        public void Execute_With_Non_Existing_Filename_Leads_To_Error()
        {
            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f nonexisting.template");

            // Assert
            actual.Should().Be("Error: File [nonexisting.template] does not exist." + Environment.NewLine);
        }

        [Fact]
        public void Execute_With_Compiler_Errors_Leads_To_Error()
        {
            _fileContentsProviderMock.Setup(x => x.FileExists("existing.template")).Returns(true);
            _fileContentsProviderMock.Setup(x => x.GetFileContents("existing.template")).Returns("<#@ template language=\"c#\" #>");
            _processorMock.Setup(x => x.Process(It.IsAny<TextTemplate>(), It.IsAny<TemplateParameter[]>()))
                          .Returns(ProcessResult.Create(new[] { new CompilerError(1, "CS1001", "Kaboom", "existing.template", false, 1) }, "code", string.Empty));
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template");

            // Assert
            actual.Should().Be(@"Compiler errors:
existing.template(1,1): error CS1001: Kaboom
");
        }

        [Fact]
        public void Execute_With_Exception_Leads_To_Error()
        {
            _fileContentsProviderMock.Setup(x => x.FileExists("existing.template")).Returns(true);
            _fileContentsProviderMock.Setup(x => x.GetFileContents("existing.template")).Returns("<#@ template language=\"c#\" #>");
            _processorMock.Setup(x => x.Process(It.IsAny<TextTemplate>(), It.IsAny<TemplateParameter[]>()))
                          .Returns(ProcessResult.Create(Array.Empty<CompilerError>(), "code", exception: new InvalidOperationException("kaboom")));
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template");

            // Assert
            actual.Should().Be(@"Exception occured:
System.InvalidOperationException: kaboom
");
        }

        [Fact]
        public void Execute_Without_Errors_And_Exception_Produces_Correct_Template_Output()
        {
            _fileContentsProviderMock.Setup(x => x.FileExists("existing.template")).Returns(true);
            _fileContentsProviderMock.Setup(x => x.GetFileContents("existing.template")).Returns("<#@ template language=\"c#\" #>");
            _processorMock.Setup(x => x.Process(It.IsAny<TextTemplate>(), It.IsAny<TemplateParameter[]>()))
                          .Returns(ProcessResult.Create(Array.Empty<CompilerError>(), "template output"));
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template");

            // Assert
            actual.Should().Be(@"Template output:
template output
");
        }

        [Fact]
        public void Execute_With_Output_Option_Saves_Output_To_File()
        {
            _fileContentsProviderMock.Setup(x => x.FileExists("existing.template")).Returns(true);
            _fileContentsProviderMock.Setup(x => x.GetFileContents("existing.template")).Returns("<#@ template language=\"c#\" #>");
            _processorMock.Setup(x => x.Process(It.IsAny<TextTemplate>(), It.IsAny<TemplateParameter[]>()))
                          .Returns(ProcessResult.Create(Array.Empty<CompilerError>(), "template output"));
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template", "-o output.txt");

            // Assert
            actual.Should().Be(@"Written template output to file: output.txt
");
            _fileContentsProviderMock.Verify(x => x.WriteFileContents("output.txt", "template output"), Times.Once);
        }
    }
}
