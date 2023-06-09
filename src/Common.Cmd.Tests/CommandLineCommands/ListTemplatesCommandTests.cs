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
    public class ListTemplatesCommandTests
    {
        private readonly Mock<ITemplateInfoRepository> _templateInfoRepositoryMock = new();

        [Fact]
        public void Ctor_Throws_On_Null_Argument()
        {
            TestHelpers.ConstructorMustThrowArgumentNullException(typeof(ListTemplatesCommand));
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
        public void Execute_Returns_Data_In_Output()
        {
            // Arrange
            _templateInfoRepositoryMock.Setup(x => x.GetTemplates()).Returns(new[] { new TemplateInfo("ShortName", "FileName.template", string.Empty, string.Empty, TemplateType.TextTemplate, Array.Empty<TemplateParameter>()) });

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut);

            // Assert
            actual.Should().Be("TemplateInfo { ShortName = ShortName, FileName = FileName.template, AssemblyName = , ClassName = , Type = TextTemplate, Parameters = TextTemplateTransformationFramework.Common.TemplateParameter[] }" + Environment.NewLine);
        }

        private ListTemplatesCommand CreateSut() => new(_templateInfoRepositoryMock.Object);
    }
}
