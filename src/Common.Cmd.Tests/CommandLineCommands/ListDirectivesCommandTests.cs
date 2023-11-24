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
    public class ListDirectivesCommandTests : TestBase
    {
        private readonly IScriptBuilder<MyDirectiveModel> _scriptBuilderMock;

        private ListDirectivesCommand<MyDirectiveModel> CreateSut() => Fixture.Create<ListDirectivesCommand<MyDirectiveModel>>();

        public ListDirectivesCommandTests()
        {
            _scriptBuilderMock = Fixture.Freeze<IScriptBuilder<MyDirectiveModel>>();
        }

        [Fact]
        public void Ctor_Throws_On_Null_Argument()
        {
            ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(typeof(ListDirectivesCommand<ListDirectivesCommandTests>));
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
        public void Execute_Produces_List_Of_Direcives()
        {
            // Arrange
            var templateSectionProcessorMock = Fixture.Freeze<IModeledTemplateSectionProcessor<MyDirectiveModel>>();
            templateSectionProcessorMock.ModelType.Returns(typeof(MyDirectiveModel));
            _scriptBuilderMock.GetKnownDirectives()
                              .Returns(new[] { templateSectionProcessorMock });

            // Act
            var actual = CommandLineCommandHelper.ExecuteCommand(CreateSut);

            // Assert
            actual.Should().Be("MyDirective" + Environment.NewLine);
        }
    }
}
