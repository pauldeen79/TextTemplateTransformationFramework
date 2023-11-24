using System;
using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
using NSubstitute;
using TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands;
using TextTemplateTransformationFramework.Common.Cmd.Tests.TestFixtures;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateSectionProcessors;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Cmd.Tests.CommandLineCommands
{
    [ExcludeFromCodeCoverage]
    public class ListDirectiveCommandTests : TestBase
    {
        private readonly IScriptBuilder<MyDirectiveModel> _scriptBuilderMock;

        private ListDirectiveCommand<MyDirectiveModel> CreateSut() => Fixture.Create<ListDirectiveCommand<MyDirectiveModel>>();

        public ListDirectiveCommandTests()
        {
            _scriptBuilderMock = Fixture.Freeze<IScriptBuilder<MyDirectiveModel>>();
        }

        [Fact]
        public void Ctor_Throws_On_Null_Argument()
        {
            ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(typeof(ListDirectiveCommand<ListDirectiveCommandTests>));
        }

        [Fact]
        public void Initialize_Adds_Command_To_Application()
        {
            // Arrange
            var app = new CommandLineApplication();
            var scriptBuilderMock = Fixture.Freeze<IScriptBuilder<ListDirectiveCommandTests>>();
            var sut = new ListDirectiveCommand<ListDirectiveCommandTests>(scriptBuilderMock);

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
        public void Execute_With_Missing_Directive_Leads_To_Error()
        {
            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut);

            // Assert
            actual.Should().Be(@"Error: Directive name is required." + Environment.NewLine);
        }

        [Fact]
        public void Execute_With_Unknown_Directive_Leads_To_Error()
        {
            // Arrange
            var argument = "-n unknown";

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, argument);

            // Assert
            actual.Should().Be(@"Error: Could not find directive with name [unknown]" + Environment.NewLine);
        }

        [Fact]
        public void Execute_With_Known_Directive_Generates_Correct_Output()
        {
            // Arrange
            var argument = "-n MyDirective";
            var templateSectionProcessorMock = Fixture.Freeze<IModeledTemplateSectionProcessor<MyDirectiveModel>>();
            templateSectionProcessorMock.ModelType.Returns(typeof(MyDirectiveModel));
            _scriptBuilderMock.GetKnownDirectives()
                              .Returns(new[] { templateSectionProcessorMock });
            _scriptBuilderMock.Build(templateSectionProcessorMock, Arg.Any<object[]>())
                              .Returns($"<# {nameof(ListDirectiveCommandTests)} #>");

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, argument);

            // Assert
            actual.Should().Be(@"Arguments:
MyParameter (System.String) 
");
        }
    }
}
