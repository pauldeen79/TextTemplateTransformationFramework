﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using CrossCutting.Common.Testing;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
using Moq;
using TextCopy;
using TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands;
using TextTemplateTransformationFramework.Common.Cmd.Tests.TestFixtures;
using TextTemplateTransformationFramework.Runtime;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Cmd.Tests.CommandLineCommands
{
    [ExcludeFromCodeCoverage]
    public class RunCodeGenerationProviderAssemblyCommandTests
    {
        private readonly Mock<IClipboard> _clipboardMock;

        private RunCodeGenerationProviderAssemblyCommand CreateSut() => new RunCodeGenerationProviderAssemblyCommand(_clipboardMock.Object);

        public RunCodeGenerationProviderAssemblyCommandTests()
        {
            _clipboardMock = new Mock<IClipboard>();
        }

        [Fact]
        public void Ctor_Throws_On_Null_Argument()
        {
            TestHelpers.ConstructorMustThrowArgumentNullException(typeof(RunCodeGenerationProviderAssemblyCommand));
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
        public void Execute_Without_Assembly_Leads_To_Error()
        {
            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut);

            // Assert
            actual.Should().Be("Error: Assembly name is required." + Environment.NewLine);
        }

        [Fact]
        public void Execute_With_Path_Option_Saves_Output_From_TemplateFileManager()
        {
            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, $"-a {GetType().Assembly.FullName}", $"-p {Directory.GetCurrentDirectory()}");

            // Assert
            actual.Should().Be($@"Written code generation output to path: {Directory.GetCurrentDirectory()}
");
        }

        [Fact]
        public void Execute_With_Clipboard_Option_Saves_Output_To_Clipboard()
        {
            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, $"-a {GetType().Assembly.FullName}", "-c");

            // Assert
            actual.Should().Be(@"Copied code generation output to clipboard
");
            var content = new MultipleContentBuilder().ToString();
            _clipboardMock.Verify(x => x.SetText(content), Times.Once);
        }

        [Fact]
        public void Execute_Without_Path_And_Clipboard_Options_Writes_Output_To_Host()
        {
            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, $"-a {GetType().Assembly.FullName}");

            // Assert
            actual.Should().Be(@$"Code generation output:
{new MultipleContentBuilder()}
");
        }
    }
}
