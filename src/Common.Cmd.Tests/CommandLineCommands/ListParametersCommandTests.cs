using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
    public class ListParametersCommandTests
    {
        private readonly Mock<ITextTemplateProcessor> _processorMock;
        private readonly Mock<IFileContentsProvider> _fileContentsProviderMock;
        private readonly Mock<IAssemblyService> _assemblyServiceMock;

        private ListParametersCommand CreateSut() => new ListParametersCommand(_processorMock.Object, _fileContentsProviderMock.Object, _assemblyServiceMock.Object);

        public ListParametersCommandTests()
        {
            _processorMock = new Mock<ITextTemplateProcessor>();
            _fileContentsProviderMock = new Mock<IFileContentsProvider>();
            _assemblyServiceMock = new Mock<IAssemblyService>();
        }

        [Fact]
        public void Ctor_Throws_On_Null_Argument()
        {
            TestHelpers.ConstructorMustThrowArgumentNullException(typeof(ListParametersCommand));
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
            actual.Should().Be("Error: Either Filename or AssemblyName is required." + Environment.NewLine);
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
            _processorMock.Setup(x => x.ExtractParameters(It.IsAny<TextTemplate>()))
                          .Returns(ExtractParametersResult.Create(Enumerable.Empty<TemplateParameter>(), new[] { new CompilerError(1, "CS1001", "Kaboom", "existing.template", false, 1) }, "code", string.Empty));
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
            _processorMock.Setup(x => x.ExtractParameters(It.IsAny<TextTemplate>()))
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
            _fileContentsProviderMock.Setup(x => x.FileExists("existing.template")).Returns(true);
            _fileContentsProviderMock.Setup(x => x.GetFileContents("existing.template")).Returns("<#@ template language=\"c#\" #>");
            _processorMock.Setup(x => x.ExtractParameters(It.IsAny<TextTemplate>()))
                          .Returns(ExtractParametersResult.Create(new[] { new TemplateParameter { Name = "MyParameter", Type = typeof(string) } }, Array.Empty<CompilerError>(), "code", string.Empty));
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f existing.template");

            // Assert
            actual.Should().Be(@"MyParameter (System.String)
");
        }

        [Fact]
        public void Execute_Without_Errors_And_Exception_Produces_Correct_List_Of_Parameters_For_AssemblyTemplate()
        {
            _processorMock.Setup(x => x.ExtractParameters(It.IsAny<AssemblyTemplate>()))
                          .Returns(ExtractParametersResult.Create(new[] { new TemplateParameter { Name = "MyParameter", Type = typeof(string) } }, Array.Empty<CompilerError>(), "code", string.Empty));
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-a my.dll", "-n myclassname");

            // Assert
            actual.Should().Be(@"MyParameter (System.String)
");
        }
    }
}
