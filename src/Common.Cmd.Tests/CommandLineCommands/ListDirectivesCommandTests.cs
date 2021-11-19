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
    public class ListDirectivesCommandTests
    {
        private readonly Mock<IScriptBuilder<MyDirectiveModel>> _scriptBuilderMock;

        private ListDirectivesCommand<MyDirectiveModel> CreateSut() => new ListDirectivesCommand<MyDirectiveModel>(_scriptBuilderMock.Object);

        public ListDirectivesCommandTests()
        {
            _scriptBuilderMock = new Mock<IScriptBuilder<MyDirectiveModel>>();
        }

        [Fact]
        public void Ctor_Throws_On_Null_Argument()
        {
            TestHelpers.ConstructorMustThrowArgumentNullException(typeof(ListDirectivesCommand<ListDirectivesCommandTests>));
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
        public void Execute_Produces_List_Of_Direcives()
        {
            // Arrange
            var templateSectionProcessorMock = new Mock<IModeledTemplateSectionProcessor<MyDirectiveModel>>();
            templateSectionProcessorMock.SetupGet(x => x.ModelType).Returns(typeof(MyDirectiveModel));
            _scriptBuilderMock.Setup(x => x.GetKnownDirectives())
                              .Returns(new[] { templateSectionProcessorMock.Object });

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut);

            // Assert
            actual.Should().Be("MyDirective" + Environment.NewLine);
        }
    }
}
