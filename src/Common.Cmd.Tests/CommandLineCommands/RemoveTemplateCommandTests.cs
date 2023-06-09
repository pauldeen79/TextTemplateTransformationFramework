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
    public class RemoveTemplateCommandTests
    {
        private readonly Mock<IFileContentsProvider> _fileContentsProviderMock = new();
        private readonly Mock<ITemplateInfoRepository> _templateInfoRepositoryMock = new();

        [Fact]
        public void Ctor_Throws_On_Null_Argument()
        {
            TestHelpers.ConstructorMustThrowArgumentNullException(typeof(RemoveTemplateCommand));
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
            _fileContentsProviderMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f my.template");

            // Assert
            actual.Should().Be("Error: Shortname is required." + Environment.NewLine);
        }

        [Fact]
        public void Execute_With_ShortName_And_Filename_Removes_Template_From_Repository()
        {
            // Arrange
            _fileContentsProviderMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, "-f my.template", "-s mytemplate");

            // Assert
            actual.Should().Be("Template has been removed successfully." + Environment.NewLine);
            _templateInfoRepositoryMock.Verify(x => x.Remove(It.IsAny<TemplateInfo>()), Times.Once);
        }

        private RemoveTemplateCommand CreateSut()
            => new RemoveTemplateCommand(_fileContentsProviderMock.Object, _templateInfoRepositoryMock.Object);
    }
}
