using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CrossCutting.Common.Testing;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
using Moq;
using TextCopy;
using TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
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
        private readonly Mock<IUserInput> _userInputMock;
        private readonly Mock<IClipboard> _clipboardMock;

        private RunTemplateCommand CreateSut() => new RunTemplateCommand(_processorMock.Object,
                                                                         _fileContentsProviderMock.Object,
                                                                         _userInputMock.Object,
                                                                         _clipboardMock.Object);

        public RunTemplateCommandTests()
        {
            _processorMock = new Mock<ITextTemplateProcessor>();
            _fileContentsProviderMock = new Mock<IFileContentsProvider>();
            _userInputMock = new Mock<IUserInput>();
            _clipboardMock = new Mock<IClipboard>();
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
            var sut = CreateSut();

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
            actual.Should().Be("Error: Either Filename or AssemblyName is required." + Environment.NewLine);
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
        public void Execute_With_Both_Filename_And_AssemblyName_Leads_To_Error()
        {
            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template", "-a myassembly.dll");

            // Assert
            actual.Should().Be("Error: You can either use Filename or AssemblyName, not both." + Environment.NewLine);
        }

        [Fact]
        public void Execute_With_AssemblyName_Without_ClassName_Leads_To_Error()
        {
            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-a myassembly.dll");

            // Assert
            actual.Should().Be("Error: When AssemblyName is filled, then ClassName is required." + Environment.NewLine);
        }

        [Fact]
        public void Execute_With_Compiler_Errors_Leads_To_Error()
        {
            // Arrange
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
        public void Execute_With_Exception_While_Extracting_Parameters_Leads_To_Error()
        {
            // Arrange
            _fileContentsProviderMock.Setup(x => x.FileExists("existing.template")).Returns(true);
            _fileContentsProviderMock.Setup(x => x.GetFileContents("existing.template")).Returns("<#@ template language=\"c#\" #>");
            _processorMock.Setup(x => x.ExtractParameters(It.IsAny<TextTemplate>()))
                          .Returns(ExtractParametersResult.Create(Array.Empty<TemplateParameter>(),
                                                                  Array.Empty<CompilerError>(),
                                                                  string.Empty,
                                                                  string.Empty,
                                                                  new InvalidOperationException("kaboom")));

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template", "--interactive");

            // Assert
            actual.Should().Be(@"Exception occured while extracting parameters from the template:
System.InvalidOperationException: kaboom
");
        }

        [Fact]
        public void Execute_With_Exception_While_Processing_Template_Leads_To_Error()
        {
            // Arrange
            _fileContentsProviderMock.Setup(x => x.FileExists("existing.template")).Returns(true);
            _fileContentsProviderMock.Setup(x => x.GetFileContents("existing.template")).Returns("<#@ template language=\"c#\" #>");
            _processorMock.Setup(x => x.Process(It.IsAny<TextTemplate>(), It.IsAny<TemplateParameter[]>()))
                          .Returns(ProcessResult.Create(Array.Empty<CompilerError>(), "code", exception: new InvalidOperationException("kaboom")));

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template");

            // Assert
            actual.Should().Be(@"Exception occured while processing the template:
System.InvalidOperationException: kaboom
");
        }

        [Fact]
        public void Execute_Without_Errors_And_Exception_Produces_Correct_Template_Output()
        {
            // Arrange
            _fileContentsProviderMock.Setup(x => x.FileExists("existing.template")).Returns(true);
            _fileContentsProviderMock.Setup(x => x.GetFileContents("existing.template")).Returns("<#@ template language=\"c#\" #>");
            _processorMock.Setup(x => x.Process(It.IsAny<TextTemplate>(), It.IsAny<TemplateParameter[]>()))
                          .Returns(ProcessResult.Create(Array.Empty<CompilerError>(), "template output"));

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template");

            // Assert
            actual.Should().Be(@"Template output:
template output
");
        }

        [Fact]
        public void Execute_With_Output_Option_Saves_Output_To_File()
        {
            // Arrange
            _fileContentsProviderMock.Setup(x => x.FileExists("existing.template")).Returns(true);
            _fileContentsProviderMock.Setup(x => x.GetFileContents("existing.template")).Returns("<#@ template language=\"c#\" #>");
            _processorMock.Setup(x => x.Process(It.IsAny<TextTemplate>(), It.IsAny<TemplateParameter[]>()))
                          .Returns(ProcessResult.Create(Array.Empty<CompilerError>(), "template output"));

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template", "-o output.txt");

            // Assert
            actual.Should().Be(@"Written template output to file: output.txt
");
            _fileContentsProviderMock.Verify(x => x.WriteFileContents("output.txt", "template output"), Times.Once);
        }

        [Fact]
        public void Execute_With_Clipboard_Option_Saves_Output_To_Clipboard()
        {
            // Arrange
            _fileContentsProviderMock.Setup(x => x.FileExists("existing.template")).Returns(true);
            _fileContentsProviderMock.Setup(x => x.GetFileContents("existing.template")).Returns("<#@ template language=\"c#\" #>");
            _processorMock.Setup(x => x.Process(It.IsAny<TextTemplate>(), It.IsAny<TemplateParameter[]>()))
                          .Returns(ProcessResult.Create(Array.Empty<CompilerError>(), "template output"));

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template", "-c");

            // Assert
            actual.Should().Be(@"Copied template output to clipboard
");
            _clipboardMock.Verify(x => x.SetText("template output"), Times.Once);
        }

        [Fact]
        public void Execute_With_DiagnosticsOutput_Option_Saves_DiagnosticsOutput_To_File()
        {
            // Arrange
            _fileContentsProviderMock.Setup(x => x.FileExists("existing.template")).Returns(true);
            _fileContentsProviderMock.Setup(x => x.GetFileContents("existing.template")).Returns("<#@ template language=\"c#\" #>");
            _processorMock.Setup(x => x.Process(It.IsAny<TextTemplate>(), It.IsAny<TemplateParameter[]>()))
                          .Returns(ProcessResult.Create(Array.Empty<CompilerError>(), "template output", "source", "diagnostics output"));

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template", "-diag diagnosticsoutput.txt");

            // Assert
            actual.Should().Be(@"Written diagnostic dump to file: diagnosticsoutput.txt
Template output:
template output
");
            _fileContentsProviderMock.Verify(x => x.WriteFileContents("diagnosticsoutput.txt", "diagnostics output"), Times.Once);
        }

        [Fact]
        public void Execute_With_Parameters_Processes_Parameters_Correctly()
        {
            // Arrange
            _fileContentsProviderMock.Setup(x => x.FileExists("existing.template")).Returns(true);
            _fileContentsProviderMock.Setup(x => x.GetFileContents("existing.template")).Returns("<#@ template language=\"c#\" #>");
            _processorMock.Setup(x => x.Process(It.IsAny<TextTemplate>(), It.IsAny<TemplateParameter[]>()))
                          .Returns(ProcessResult.Create(Array.Empty<CompilerError>(), "template output"));

            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template", "param1:value1", "param2:value2");

            // Assert
            _processorMock.Verify(x => x.Process(It.IsAny<TextTemplate>(), It.Is<TemplateParameter[]>(x => x.Length == 2)));
        }

        [Fact]
        public void Execute_Interactive_Gets_Parameters_Correctly()
        {
            // Arrange
            _fileContentsProviderMock.Setup(x => x.FileExists("existing.template")).Returns(true);
            _fileContentsProviderMock.Setup(x => x.GetFileContents("existing.template")).Returns("<#@ template language=\"c#\" #>");
            _processorMock.Setup(x => x.ExtractParameters(It.IsAny<TextTemplate>()))
                          .Returns(ExtractParametersResult.Create(Enumerable.Range(1, 2).Select(x => new TemplateParameter { Name = $"param{x}", Type = typeof(string) }).ToArray(), Array.Empty<CompilerError>(), "{}", string.Empty));
            _processorMock.Setup(x => x.Process(It.IsAny<TextTemplate>(), It.IsAny<TemplateParameter[]>()))
                          .Returns(ProcessResult.Create(Array.Empty<CompilerError>(), "template output"));

            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template", "--interactive");

            // Assert
            _userInputMock.Verify(x => x.GetValue(It.IsAny<TemplateParameter>()), Times.Exactly(2));
            _processorMock.Verify(x => x.Process(It.IsAny<TextTemplate>(), It.Is<TemplateParameter[]>(x => x.Length == 2)));
        }
    }
}
