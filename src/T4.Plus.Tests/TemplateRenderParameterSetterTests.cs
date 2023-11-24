using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Tests.TestFixtures;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Plus.Tests
{
    [ExcludeFromCodeCoverage]
    public class TemplateRenderParameterSetterTests : TestBase
    {
        [Fact]
        public void Set_Throws_On_Null_Context()
        {
            // Arrange
            var sut = new TemplateRenderParameterSetter<TemplateRenderParameterSetterTests>();
            var action = new Action(() => sut.Set(null));

            // Act & Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Set_Works_On_Template_Without_ViewModel()
        {
            // Arrange
            var sut = new TemplateRenderParameterSetter<TemplateRenderParameterSetterTests>();
            var template = new object();
            var templateProcessorContextMock = Fixture.Freeze<ITemplateProcessorContext<TemplateRenderParameterSetterTests>>();
            templateProcessorContextMock.TemplateCompilerOutput.Returns(TemplateCompilerOutput.Create(GetType().Assembly, template, Enumerable.Empty<CompilerError>(), "", "cs", Enumerable.Empty<ITemplateToken<TemplateRenderParameterSetterTests>>(), null));
            var action = new Action(() => sut.Set(templateProcessorContextMock));

            // Act
            action.Should().NotThrow();
        }

        [Fact]
        public void Set_Sets_Property_On_ViewModel()
        {
            // Arrange
            var sut = new TemplateRenderParameterSetter<TemplateRenderParameterSetterTests>();
            var template = new ViewModelTemplate
            {
                Property = "Ignored",
                TemplateContext = new object()
            };
            template.Session.Add(nameof(ViewModelTemplate.Property), "Test");
            template.Session.Add("Ignored", "Value");
            var templateProcessorContextMock = Fixture.Freeze<ITemplateProcessorContext<TemplateRenderParameterSetterTests>>();
            templateProcessorContextMock.TemplateCompilerOutput.Returns(TemplateCompilerOutput.Create(GetType().Assembly, template, Enumerable.Empty<CompilerError>(), "", "cs", Enumerable.Empty<ITemplateToken<TemplateRenderParameterSetterTests>>(), null));

            // Act
            sut.Set(templateProcessorContextMock);

            // Assert
            template.ViewModel.Should().NotBeNull();
            template.ViewModel.PropertyWithDefaultValue.Should().Be("Default");
            template.ViewModel.Property.Should().Be("Test");
            template.ViewModel.TemplateContext.Should().BeSameAs(template.TemplateContext);
            template.Session.Should().ContainKey(nameof(template.ViewModel.PropertyWithDefaultValue)).WhoseValue.Should().Be("Default");
        }
    }
}
