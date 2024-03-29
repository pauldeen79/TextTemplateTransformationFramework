﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
using NSubstitute;
using TextCopy;
using TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands;
using TextTemplateTransformationFramework.Common.Cmd.Contracts;
using TextTemplateTransformationFramework.Common.Cmd.Tests.TestFixtures;
using TextTemplateTransformationFramework.Common.Contracts;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Cmd.Tests.CommandLineCommands
{
    [ExcludeFromCodeCoverage]
    public class RunTemplateCommandTests : TestBase
    {
        private readonly ITextTemplateProcessor _processorMock;
        private readonly IFileContentsProvider _fileContentsProviderMock;
        private readonly ITemplateInfoRepository _templateInfoRepositoryMock;
        private readonly IUserInput _userInputMock;
        private readonly IClipboard _clipboardMock;

        private RunTemplateCommand CreateSut() => Fixture.Create<RunTemplateCommand>();

        public RunTemplateCommandTests()
        {
            _processorMock = Fixture.Freeze<ITextTemplateProcessor>();
            _fileContentsProviderMock = Fixture.Freeze<IFileContentsProvider>();
            _templateInfoRepositoryMock = Fixture.Freeze<ITemplateInfoRepository>();
            _userInputMock = Fixture.Freeze<IUserInput>();
            _clipboardMock = Fixture.Freeze<IClipboard>();
        }

        [Fact]
        public void Ctor_Throws_On_Null_Argument()
        {
            ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(typeof(RunTemplateCommand));
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
        public void Execute_Without_Filename_Leads_To_Error()
        {
            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut);

            // Assert
            actual.Should().Be("Error: Either Filename, AssemblyName or ShortName is required." + Environment.NewLine);
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
            actual.Should().Be("Error: You can either use Filename, AssemblyName or ShortName, not a combination of these." + Environment.NewLine);
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
            _fileContentsProviderMock.FileExists("existing.template").Returns(true);
            _fileContentsProviderMock.GetFileContents("existing.template").Returns("<#@ template language=\"c#\" #>");
            _processorMock.Process(Arg.Any<TextTemplate>(), Arg.Any<TemplateParameter[]>())
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
            _fileContentsProviderMock.FileExists("existing.template").Returns(true);
            _fileContentsProviderMock.GetFileContents("existing.template").Returns("<#@ template language=\"c#\" #>");
            _processorMock.ExtractParameters(Arg.Any<TextTemplate>())
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
            _fileContentsProviderMock.FileExists("existing.template").Returns(true);
            _fileContentsProviderMock.GetFileContents("existing.template").Returns("<#@ template language=\"c#\" #>");
            _processorMock.Process(Arg.Any<TextTemplate>(), Arg.Any<TemplateParameter[]>())
                          .Returns(ProcessResult.Create(Array.Empty<CompilerError>(), "code", exception: new InvalidOperationException("kaboom")));

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template");

            // Assert
            actual.Should().Be(@"Exception occured while processing the template:
System.InvalidOperationException: kaboom
");
        }

        [Fact]
        public void Execute_Without_Errors_And_Exception_Produces_Correct_Template_Output_For_TextTemplate()
        {
            // Arrange
            _fileContentsProviderMock.FileExists("existing.template").Returns(true);
            _fileContentsProviderMock.GetFileContents("existing.template").Returns("<#@ template language=\"c#\" #>");
            _processorMock.Process(Arg.Any<TextTemplate>(), Arg.Any<TemplateParameter[]>())
                          .Returns(ProcessResult.Create(Array.Empty<CompilerError>(), "template output"));

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template");

            // Assert
            actual.Should().Be(@"Template output:
template output
");
        }

        [Fact]
        public void Execute_Without_Errors_And_Exception_Produces_Correct_Template_Output_For_AssemblyTemplate()
        {
            // Arrange
            _processorMock.Process(Arg.Any<AssemblyTemplate>(), Arg.Any<TemplateParameter[]>())
                          .Returns(ProcessResult.Create(Array.Empty<CompilerError>(), "template output"));

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-a my.dll", "-n myclass");

            // Assert
            actual.Should().Be(@"Template output:
template output
");
        }

        [Fact]
        public void Execute_Without_Errors_And_Exception_Produces_Correct_Template_Output_For_GlobalTemplate_Text()
        {
            // Arrange
            _fileContentsProviderMock.FileExists("existing.template").Returns(true);
            _fileContentsProviderMock.GetFileContents("existing.template").Returns("<#@ template language=\"c#\" #>");
            _templateInfoRepositoryMock.FindByShortName(Arg.Any<string>()).Returns(new TemplateInfo("myshortname", "existing.template", "", "", TemplateType.TextTemplate, Array.Empty<TemplateParameter>()));
            _processorMock.Process(Arg.Any<TextTemplate>(), Arg.Any<TemplateParameter[]>())
                          .Returns(ProcessResult.Create(Array.Empty<CompilerError>(), "template output"));

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-s myshortname");

            // Assert
            actual.Should().Be(@"Template output:
template output
");
        }

        [Fact]
        public void Execute_Without_Errors_And_Exception_Produces_Correct_Template_Output_For_GlobalTemplate_Assembly()
        {
            // Arrange
            _templateInfoRepositoryMock.FindByShortName(Arg.Any<string>()).Returns(new TemplateInfo("myshortname", "", "my.dll", "myclass", TemplateType.AssemblyTemplate, Array.Empty<TemplateParameter>()));
            _processorMock.Process(Arg.Any<AssemblyTemplate>(), Arg.Any<TemplateParameter[]>())
                          .Returns(ProcessResult.Create(Array.Empty<CompilerError>(), "template output"));

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-s myshortname");

            // Assert
            actual.Should().Be(@"Template output:
template output
");
        }

        [Fact]
        public void Execute_With_Non_Existing_Short_Name_Gives_Exception()
        {
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-s myshortname");

            // Assert
            actual.Should().Be(@"Exception occured while processing the template:
System.InvalidOperationException: Could not find template with short name myshortname
");
        }

        [Fact]
        public void Execute_With_Output_Option_Saves_Output_To_File()
        {
            // Arrange
            _fileContentsProviderMock.FileExists("existing.template").Returns(true);
            _fileContentsProviderMock.GetFileContents("existing.template").Returns("<#@ template language=\"c#\" #>");
            _processorMock.Process(Arg.Any<TextTemplate>(), Arg.Any<TemplateParameter[]>())
                          .Returns(ProcessResult.Create(Array.Empty<CompilerError>(), "template output"));

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template", "-o output.txt");

            // Assert
            actual.Should().Be(@"Written template output to file: output.txt
");
            _fileContentsProviderMock.Received(1).WriteFileContents("output.txt", "template output");
        }

        [Fact]
        public void Execute_With_Clipboard_Option_Saves_Output_To_Clipboard()
        {
            // Arrange
            _fileContentsProviderMock.FileExists("existing.template").Returns(true);
            _fileContentsProviderMock.GetFileContents("existing.template").Returns("<#@ template language=\"c#\" #>");
            _processorMock.Process(Arg.Any<TextTemplate>(), Arg.Any<TemplateParameter[]>())
                          .Returns(ProcessResult.Create(Array.Empty<CompilerError>(), "template output"));

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template", "-c");

            // Assert
            actual.Should().Be(@"Copied template output to clipboard
");
            _clipboardMock.Received(1).SetText("template output");
        }

        [Fact]
        public void Execute_With_DiagnosticsOutput_Option_Saves_DiagnosticsOutput_To_File()
        {
            // Arrange
            _fileContentsProviderMock.FileExists("existing.template").Returns(true);
            _fileContentsProviderMock.GetFileContents("existing.template").Returns("<#@ template language=\"c#\" #>");
            _processorMock.Process(Arg.Any<TextTemplate>(), Arg.Any<TemplateParameter[]>())
                          .Returns(ProcessResult.Create(Array.Empty<CompilerError>(), "template output", "source", "diagnostics output"));

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template", "-diag diagnosticsoutput.txt");

            // Assert
            actual.Should().Be(@"Written diagnostic dump to file: diagnosticsoutput.txt
Template output:
template output
");
            _fileContentsProviderMock.Received(1).WriteFileContents("diagnosticsoutput.txt", "diagnostics output");
        }

        [Fact]
        public void Execute_With_Parameters_Processes_Parameters_Correctly()
        {
            // Arrange
            _fileContentsProviderMock.FileExists("existing.template").Returns(true);
            _fileContentsProviderMock.GetFileContents("existing.template").Returns("<#@ template language=\"c#\" #>");
            _processorMock.Process(Arg.Any<TextTemplate>(), Arg.Any<TemplateParameter[]>())
                          .Returns(ProcessResult.Create(Array.Empty<CompilerError>(), "template output"));

            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template", "param1:value1", "param2:value2");

            // Assert
            _processorMock.Received(1).Process(Arg.Any<TextTemplate>(), Arg.Is<TemplateParameter[]>(x => x.Length == 2));
        }

        [Fact]
        public void Execute_Global_Template_With_Parameters_Merges_Parameters_Correctly()
        {
            // Arrange
            _fileContentsProviderMock.FileExists("existing.template").Returns(true);
            _fileContentsProviderMock.GetFileContents("existing.template").Returns("<#@ template language=\"c#\" #>");
            _templateInfoRepositoryMock.FindByShortName(Arg.Any<string>()).Returns(new TemplateInfo("myshortname", "existing.template", "", "", TemplateType.TextTemplate, [new TemplateParameter { Name = "param1", Value = "defaultValue" }]));
            _processorMock.Process(Arg.Any<TextTemplate>(), Arg.Any<TemplateParameter[]>())
                          .Returns(ProcessResult.Create(Array.Empty<CompilerError>(), "template output"));

            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-s myshortname", "param1:value1", "param2:value2");

            // Assert
            _processorMock.Received(1).Process(Arg.Any<TextTemplate>(), Arg.Is<TemplateParameter[]>(x => x.Length == 2 && x[0].Name == "param1" && x[0].Value.ToString() == "value1" && x[x.Length - 1].Name == "param2" && x[x.Length - 1].Value.ToString() == "value2"));
        }

        [Fact]
        public void Execute_Interactive_Gets_Parameters_Correctly()
        {
            // Arrange
            _fileContentsProviderMock.FileExists("existing.template").Returns(true);
            _fileContentsProviderMock.GetFileContents("existing.template").Returns("<#@ template language=\"c#\" #>");
            _processorMock.ExtractParameters(Arg.Any<TextTemplate>())
                          .Returns(ExtractParametersResult.Create(Enumerable.Range(1, 2).Select(x => new TemplateParameter { Name = $"param{x}", Type = typeof(string) }).ToArray(), Array.Empty<CompilerError>(), "{}", string.Empty));
            _processorMock.Process(Arg.Any<TextTemplate>(), Arg.Any<TemplateParameter[]>())
                          .Returns(ProcessResult.Create(Array.Empty<CompilerError>(), "template output"));

            // Act
            _ = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template", "--interactive");

            // Assert
            _userInputMock.Received(2).GetValue(Arg.Any<TemplateParameter>());
            _processorMock.Received(1).Process(Arg.Any<TextTemplate>(), Arg.Is<TemplateParameter[]>(x => x.Length == 2));
        }
    }
}
