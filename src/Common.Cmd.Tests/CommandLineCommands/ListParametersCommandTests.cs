using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
using NSubstitute;
using TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands;
using TextTemplateTransformationFramework.Common.Cmd.Tests.TestFixtures;
using TextTemplateTransformationFramework.Common.Contracts;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Cmd.Tests.CommandLineCommands
{
    [ExcludeFromCodeCoverage]
    public class ListParametersCommandTests : TestBase
    {
        private readonly ITextTemplateProcessor _processorMock;
        private readonly IFileContentsProvider _fileContentsProviderMock;
        private readonly ITemplateInfoRepository _templateInfoRepositoryMock;

        private ListParametersCommand CreateSut() => Fixture.Create<ListParametersCommand>();

        public ListParametersCommandTests()
        {
            _processorMock = Fixture.Freeze<ITextTemplateProcessor>();
            _fileContentsProviderMock = Fixture.Freeze<IFileContentsProvider>();
            _templateInfoRepositoryMock = Fixture.Freeze<ITemplateInfoRepository>();
        }

        [Fact]
        public void Ctor_Throws_On_Null_Argument()
        {
            ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(typeof(ListParametersCommand));
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
            _fileContentsProviderMock.FileExists("existing.template").Returns(true);
            _fileContentsProviderMock.GetFileContents("existing.template").Returns("<#@ template language=\"c#\" #>");
            _processorMock.ExtractParameters(Arg.Any<TextTemplate>())
                          .Returns(ExtractParametersResult.Create(Enumerable.Empty<TemplateParameter>(), [new CompilerError(1, "CS1001", "Kaboom", "existing.template", false, 1)], "code", string.Empty));
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template");

            // Assert
            actual.Should().Be(@"Compiler errors:
existing.template(1,1): error CS1001: Kaboom
");
        }

        [Fact]
        public void Execute_With_Exception_Leads_To_Error()
        {
            _fileContentsProviderMock.FileExists("existing.template").Returns(true);
            _fileContentsProviderMock.GetFileContents("existing.template").Returns("<#@ template language=\"c#\" #>");
            _processorMock.ExtractParameters(Arg.Any<TextTemplate>())
                          .Returns(ExtractParametersResult.Create(Enumerable.Empty<TemplateParameter>(), Array.Empty<CompilerError>(), "code", string.Empty, new InvalidOperationException("kaboom")));
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template");

            // Assert
            actual.Should().Be(@"Exception occured:
System.InvalidOperationException: kaboom
");
        }

        [Fact]
        public void Execute_Without_Errors_And_Exception_Produces_Correct_List_Of_Parameters_For_TextTemplate()
        {
            _fileContentsProviderMock.FileExists("existing.template").Returns(true);
            _fileContentsProviderMock.GetFileContents("existing.template").Returns("<#@ template language=\"c#\" #>");
            _processorMock.ExtractParameters(Arg.Any<TextTemplate>())
                          .Returns(ExtractParametersResult.Create(new[] { new TemplateParameter { Name = "MyParameter", Type = typeof(string) } }, Array.Empty<CompilerError>(), "code", string.Empty));
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template");

            // Assert
            actual.Should().Be(@"MyParameter (System.String)
");
        }

        [Fact]
        public void Execute_Without_Errors_And_Exception_Produces_Correct_List_Of_Parameters_For_AssemblyTemplate()
        {
            _processorMock.ExtractParameters(Arg.Any<AssemblyTemplate>())
                          .Returns(ExtractParametersResult.Create(new[] { new TemplateParameter { Name = "MyParameter", Type = typeof(string) } }, Array.Empty<CompilerError>(), "code", string.Empty));
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-a my.dll", "-n myclassname");

            // Assert
            actual.Should().Be(@"MyParameter (System.String)
");
        }

        [Fact]
        public void Execute_Without_Errors_And_Exception_Produces_Correct_List_Of_Parameters_For_TextTemplate_Using_ShortName()
        {
            _fileContentsProviderMock.FileExists("existing.template").Returns(true);
            _fileContentsProviderMock.GetFileContents("existing.template").Returns("<#@ template language=\"c#\" #>");
            _templateInfoRepositoryMock.FindByShortName(Arg.Any<string>()).Returns(new TemplateInfo("myshortname", "existing.template", "", "", TemplateType.TextTemplate, Array.Empty<TemplateParameter>()));
            _processorMock.ExtractParameters(Arg.Any<TextTemplate>())
                          .Returns(ExtractParametersResult.Create(new[] { new TemplateParameter { Name = "MyParameter", Type = typeof(string) } }, Array.Empty<CompilerError>(), "code", string.Empty));
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-s myshortname");

            // Assert
            actual.Should().Be(@"MyParameter (System.String)
");
        }

        [Fact]
        public void Execute_Without_Errors_And_Exception_Produces_Correct_List_Of_Parameters_For_AssemblyTemplate_Using_ShortName()
        {
            _processorMock.ExtractParameters(Arg.Any<AssemblyTemplate>())
                          .Returns(ExtractParametersResult.Create(new[] { new TemplateParameter { Name = "MyParameter", Type = typeof(string) } }, Array.Empty<CompilerError>(), "code", string.Empty));
            _templateInfoRepositoryMock.FindByShortName(Arg.Any<string>()).Returns(new TemplateInfo("myshortname", "", "my.dll", "myclassname", TemplateType.AssemblyTemplate, Array.Empty<TemplateParameter>()));
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-s myshortname");

            // Assert
            actual.Should().Be(@"MyParameter (System.String)
");
        }

        [Fact]
        public void Execute_With_Non_Existing_Short_Name_Gives_Exception()
        {
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-s myshortname");

            // Assert
            actual.Should().Be(@"Exception occured:
System.InvalidOperationException: Could not find template with short name myshortname
");
        }
    }
}
