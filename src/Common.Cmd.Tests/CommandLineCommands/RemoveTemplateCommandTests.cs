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
    public class RemoveTemplateCommandTests : TestBase
    {
        private readonly IFileContentsProvider _fileContentsProviderMock;
        private readonly ITemplateInfoRepository _templateInfoRepositoryMock;

        public RemoveTemplateCommandTests()
        {
            _fileContentsProviderMock = Fixture.Freeze<IFileContentsProvider>();
            _templateInfoRepositoryMock = Fixture.Freeze<ITemplateInfoRepository>();
        }

        [Fact]
        public void Ctor_Throws_On_Null_Argument()
        {
            ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(typeof(RemoveTemplateCommand));
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
        public void Execute_With_ShortName_And_Filename_Removes_Template_From_Repository()
        {
            // Arrange
            _fileContentsProviderMock.FileExists(Arg.Any<string>()).Returns(true);

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f my.template", "-s mytemplate");

            // Assert
            actual.Should().Be("Template has been removed successfully." + Environment.NewLine);
            _templateInfoRepositoryMock.Received(1).Remove(Arg.Any<TemplateInfo>());
        }

        private RemoveTemplateCommand CreateSut() => Fixture.Create<RemoveTemplateCommand>();
    }
}
