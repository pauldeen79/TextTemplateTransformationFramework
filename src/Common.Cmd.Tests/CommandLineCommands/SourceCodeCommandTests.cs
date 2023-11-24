using System;
using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
using NSubstitute;
using TextCopy;
using TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands;
using TextTemplateTransformationFramework.Common.Cmd.Tests.TestFixtures;
using TextTemplateTransformationFramework.Common.Contracts;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Cmd.Tests.CommandLineCommands
{
    [ExcludeFromCodeCoverage]
    public class SourceCodeCommandTests : TestBase
    {
        private readonly ITextTemplateProcessor _processorMock;
        private readonly IFileContentsProvider _fileContentsProviderMock;
        private readonly IClipboard _clipboardMock;

        private SourceCodeCommand CreateSut() => Fixture.Create<SourceCodeCommand>();

        public SourceCodeCommandTests()
        {
            _processorMock = Fixture.Freeze<ITextTemplateProcessor>();
            _fileContentsProviderMock = Fixture.Freeze<IFileContentsProvider>();
            _clipboardMock = Fixture.Freeze<IClipboard>();
        }

        [Fact]
        public void Ctor_Throws_On_Null_Argument()
        {
            ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(typeof(SourceCodeCommand));
        }

        [Fact]
        public void Initialize_Adds_Command_To_Application()
        {
            // Arrange
            var app = new CommandLineApplication();
            var textTemplateProcessorMock = Fixture.Freeze<ITextTemplateProcessor>();
            var fileContentsProviderMock = Fixture.Freeze<IFileContentsProvider>();
            var sut = new SourceCodeCommand(textTemplateProcessorMock, fileContentsProviderMock, _clipboardMock);

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
        public void Execute_With_Exception_Leads_To_Error()
        {
            _fileContentsProviderMock.FileExists("existing.template").Returns(true);
            _fileContentsProviderMock.GetFileContents("existing.template").Returns("<#@ template language=\"c#\" #>");
            _processorMock.PreProcess(Arg.Any<TextTemplate>(), Arg.Any<TemplateParameter[]>())
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
            _fileContentsProviderMock.FileExists("existing.template").Returns(true);
            _fileContentsProviderMock.GetFileContents("existing.template").Returns("<#@ template language=\"c#\" #>");
            _processorMock.PreProcess(Arg.Any<TextTemplate>(), Arg.Any<TemplateParameter[]>())
                          .Returns(ProcessResult.Create(Array.Empty<CompilerError>(), string.Empty, "CodeGoesHere();"));
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template");

            // Assert
            actual.Should().Be(@"Source code output:
CodeGoesHere();
");
        }

        [Fact]
        public void Execute_With_Output_Option_Saves_Output_To_File()
        {
            _fileContentsProviderMock.FileExists("existing.template").Returns(true);
            _fileContentsProviderMock.GetFileContents("existing.template").Returns("<#@ template language=\"c#\" #>");
            _processorMock.PreProcess(Arg.Any<TextTemplate>(), Arg.Any<TemplateParameter[]>())
                          .Returns(ProcessResult.Create(Array.Empty<CompilerError>(), string.Empty, "CodeGoesHere();"));
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template", "-o output.cs");

            // Assert
            actual.Should().Be(@"Written source code output to file: output.cs
");
            _fileContentsProviderMock.Received(1).WriteFileContents("output.cs", "CodeGoesHere();");
        }

        [Fact]
        public void Execute_With_Clipboard_Option_Saves_Output_To_Clipboard()
        {
            _fileContentsProviderMock.FileExists("existing.template").Returns(true);
            _fileContentsProviderMock.GetFileContents("existing.template").Returns("<#@ template language=\"c#\" #>");
            _processorMock.PreProcess(Arg.Any<TextTemplate>(), Arg.Any<TemplateParameter[]>())
                          .Returns(ProcessResult.Create(Array.Empty<CompilerError>(), string.Empty, "CodeGoesHere();"));
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template", "-c");

            // Assert
            actual.Should().Be(@"Copied source code to clipboard
");
            _clipboardMock.Received(1).SetText("CodeGoesHere();");
        }
    }
}
