using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Tests.TestFixtures;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Plus.Tests
{
    [ExcludeFromCodeCoverage]
    public class TemplateInitializeParameterSetterTests : TestBase
    {
        [Fact]
        public void Set_Throws_On_Null_Context()
        {
            // Arrange
            var sut = new TemplateInitializeParameterSetter<TemplateInitializeParameterSetterTests>();
            var action = new Action(() => sut.Set(null));

            // Act & Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Set_Initializes_Property_And_ViewModel_Correctly()
        {
            // Arrange
            var template = new ViewModelTemplate();
            template.Session.Add("Key", "Value");
            var sut = new TemplateInitializeParameterSetter<TemplateInitializeParameterSetterTests>();
            var textTemplateProcessorContextMock = Fixture.Freeze<ITextTemplateProcessorContext<TemplateInitializeParameterSetterTests>>();
            textTemplateProcessorContextMock.Parameters.Returns(new[] { new TemplateParameter { Name = nameof(ViewModelTemplate.Property), Value = "test" } });
            var templateCompilerOutput = TemplateCompilerOutput.Create(GetType().Assembly, template, Enumerable.Empty<CompilerError>(), "", "cs", Enumerable.Empty<ITemplateToken<TemplateInitializeParameterSetterTests>>(), null);
            var context = new TemplateProcessorContext<TemplateInitializeParameterSetterTests>(textTemplateProcessorContextMock, templateCompilerOutput);

            // Act
            sut.Set(context);

            // Assert
            template.Property.Should().Be("test");
            template.ViewModel.Should().NotBeNull();
            template.ViewModel.Property.Should().Be("test");
        }
    }
}
