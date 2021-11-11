using System;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Common.Testing;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
using Moq;
using TextTemplateTransformationFramework.Common.Cmd.CommandLineCommands;
using TextTemplateTransformationFramework.Common.Cmd.Tests.TestFixtures;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateSectionProcessors;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Cmd.Tests.CommandLineCommands
{
    [ExcludeFromCodeCoverage]
    public class ListDirectiveCommandTests
    {
        private readonly Mock<IScriptBuilder<MyDirectiveModel>> _scriptBuilderMock;

        private ListDirectiveCommand<MyDirectiveModel> CreateSut() => new ListDirectiveCommand<MyDirectiveModel>(_scriptBuilderMock.Object);

        public ListDirectiveCommandTests()
        {
            _scriptBuilderMock = new Mock<IScriptBuilder<MyDirectiveModel>>();
        }

        [Fact]
        public void Ctor_Throws_On_Null_Argument()
        {
            TestHelpers.ConstructorMustThrowArgumentNullException(typeof(ListDirectiveCommand<ListDirectiveCommandTests>));
        }

        [Fact]
        public void Initialize_Adds_Command_To_Application()
        {
            // Arrange
            var app = new CommandLineApplication();
            var scriptBuilderMock = new Mock<IScriptBuilder<ListDirectiveCommandTests>>();
            var sut = new ListDirectiveCommand<ListDirectiveCommandTests>(scriptBuilderMock.Object);

            // Act
            sut.Initialize(app);

            // Assert
            app.Commands.Should().HaveCount(1);
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
            var templateSectionProcessorMock = new Mock<IModeledTemplateSectionProcessor<MyDirectiveModel>>();
            templateSectionProcessorMock.SetupGet(x => x.ModelType).Returns(typeof(MyDirectiveModel));
            _scriptBuilderMock.Setup(x => x.GetKnownDirectives())
                              .Returns(new[] { templateSectionProcessorMock.Object });
            _scriptBuilderMock.Setup(x => x.Build(templateSectionProcessorMock.Object, It.IsAny<object[]>()))
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
