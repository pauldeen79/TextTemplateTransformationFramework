﻿using System.Diagnostics.CodeAnalysis;
using CrossCutting.Common.Testing;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
using Moq;
using TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands;
using TextTemplateTransformationFramework.Common.Contracts;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Cmd.Tests.CommandLineCommands
{
    [ExcludeFromCodeCoverage]
    public class SourceCodeCommandTests
    {
        [Fact]
        public void Ctor_Throws_On_Null_Argument()
        {
            TestHelpers.ConstructorMustThrowArgumentNullException(typeof(SourceCodeCommand));
        }

        [Fact]
        public void Initialize_Adds_Command_To_Application()
        {
            // Arrange
            var app = new CommandLineApplication();
            var textTemplateProcessorMock = new Mock<ITextTemplateProcessor>();
            var fileContentsProviderMock = new Mock<IFileContentsProvider>();
            var sut = new SourceCodeCommand(textTemplateProcessorMock.Object, fileContentsProviderMock.Object);

            // Act
            sut.Initialize(app);

            // Assert
            app.Commands.Should().HaveCount(1);
        }
    }
}
