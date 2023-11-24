using System;
using System.Diagnostics.CodeAnalysis;
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
    public class AddTemplateCommandTests : TestBase
    {
        private readonly IFileContentsProvider _fileContentsProviderMock;
        private readonly ITemplateInfoRepository _templateInfoRepositoryMock;

        public AddTemplateCommandTests()
        {
            _fileContentsProviderMock = Fixture.Freeze<IFileContentsProvider>();
            _templateInfoRepositoryMock = Fixture.Freeze<ITemplateInfoRepository>();
        }

        [Fact]
        public void Ctor_Throws_On_Null_Argument()
        {
            ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(typeof(AddTemplateCommand));
        }

        [Fact]
        public void Initialize_Adds_VersionCommand_To_Application()
        {
            // Arrange
            var app = new CommandLineApplication();
            var sut = CreateSut();

            // Act
            sut.Initialize(app);

            // Assert
            app.Commands.Should().ContainSingle();
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
        public void Execute_Without_ShortName_Leads_To_Error()
        {
            // Arrange
            _fileContentsProviderMock.FileExists(Arg.Any<string>()).Returns(true);

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f my.template");

            // Assert
            actual.Should().Be("Error: Shortname is required." + Environment.NewLine);
        }

        [Fact]
        public void Execute_Without_FileName_And_AssemblyName_Leads_To_Error()
        {
            // Arrange
            _fileContentsProviderMock.FileExists(Arg.Any<string>()).Returns(true);

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-s MyShortName");

            // Assert
            actual.Should().Be("Error: Either Filename or AssemblyName is required." + Environment.NewLine);
        }

        [Fact]
        public void Execute_With_Both_FileName_And_AssemblyName_Leads_To_Error()
        {
            // Arrange
            _fileContentsProviderMock.FileExists(Arg.Any<string>()).Returns(true);

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-s MyShortName", "-f my.template", "-a my.dll", "-n myclass");

            // Assert
            actual.Should().Be("Error: You can either use Filename or AssemblyName, not both." + Environment.NewLine);
        }

        [Fact]
        public void Execute_With_AssemblyName_But_Without_ClassName_Leads_To_Error()
        {
            // Arrange
            _fileContentsProviderMock.FileExists(Arg.Any<string>()).Returns(true);

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-s MyShortName", "-a my.dll");

            // Assert
            actual.Should().Be("Error: When AssemblyName is filled, then ClassName is required." + Environment.NewLine);
        }

        [Fact]
        public void Execute_With_ShortName_And_Filename_Gives_Error_When_File_Does_Not_Exist()
        {
            // Arrange
            _fileContentsProviderMock.FileExists(Arg.Any<string>()).Returns(false);

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f my.template", "-s mytemplate");

            // Assert
            actual.Should().Be("Error: File [my.template] does not exist." + Environment.NewLine);
        }

        [Fact]
        public void Execute_With_ShortName_And_Filename_Adds_Template_To_Repository()
        {
            // Arrange
            _fileContentsProviderMock.FileExists(Arg.Any<string>()).Returns(true);

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f my.template", "-s mytemplate");

            // Assert
            actual.Should().Be("Template has been added successfully." + Environment.NewLine);
            _templateInfoRepositoryMock.Received(1).Add(Arg.Any<TemplateInfo>());
        }

        private AddTemplateCommand CreateSut() => Fixture.Create<AddTemplateCommand>();
    }
}
