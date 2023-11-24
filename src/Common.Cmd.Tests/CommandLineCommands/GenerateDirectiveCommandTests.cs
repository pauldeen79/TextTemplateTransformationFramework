using System;
using System.Diagnostics.CodeAnalysis;
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
    public class GenerateDirectiveCommandTests : TestBase
    {
        private readonly Mock<IScriptBuilder<MyDirectiveModel>> _scriptBuilderMock;

        private GenerateDirectiveCommand<MyDirectiveModel> CreateSut() => new GenerateDirectiveCommand<MyDirectiveModel>(_scriptBuilderMock.Object);

        public GenerateDirectiveCommandTests()
        {
            _scriptBuilderMock = new Mock<IScriptBuilder<MyDirectiveModel>>();
        }

        [Fact]
        public void Ctor_Throws_On_Null_Argument()
        {
            ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(typeof(GenerateDirectiveCommand<MyDirectiveModel>));
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
                              .Returns("<# MyDirective #>");

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, argument);

            // Assert
            actual.Should().Be(@"<# MyDirective #>" + Environment.NewLine);
        }

        [Fact]
        public void Execute_With_Parameter_Generates_Correct_Output()
        {
            // Arrange
            var argument = "-n MyDirective";
            var parametersArgument = "MyParameter:Value";
            var templateSectionProcessorMock = new Mock<IModeledTemplateSectionProcessor<MyDirectiveModel>>();
            templateSectionProcessorMock.SetupGet(x => x.ModelType).Returns(typeof(MyDirectiveModel));
            _scriptBuilderMock.Setup(x => x.GetKnownDirectives())
                              .Returns(new[] { templateSectionProcessorMock.Object });
            _scriptBuilderMock.Setup(x => x.Build(templateSectionProcessorMock.Object, It.IsAny<object[]>()))
                              .Returns("<# MyDirective MyParameter=Value #>");

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut, argument, parametersArgument);

            // Assert
            actual.Should().Be(@"<# MyDirective MyParameter=Value #>" + Environment.NewLine);
        }
    }
}
