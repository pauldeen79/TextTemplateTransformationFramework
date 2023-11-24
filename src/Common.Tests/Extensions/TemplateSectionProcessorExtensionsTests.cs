using System;
using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using FluentAssertions;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.Common.Mappers;
using TextTemplateTransformationFramework.Common.Models;
using TextTemplateTransformationFramework.Common.SectionProcessors;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class TemplateSectionProcessorExtensionsTests : TestBase
    {
        [Fact]
        public void CanGetModelInstanceFromMappedTemplateSectionProcessor()
        {
            // Arrange
            var fileNameProvider = Fixture.Freeze<IFileNameProvider>();
            var fileContentsProvider = Fixture.Freeze<IFileContentsProvider>();
            var templateCodeCompiler = Fixture.Freeze<ITemplateCodeCompiler<TemplateSectionProcessorExtensionsTests>>();
            var input = new TokenMapperAdapter<TemplateSectionProcessorExtensionsTests>(typeof(AssemblyTokenMapper<TemplateSectionProcessorExtensionsTests>), fileNameProvider, fileContentsProvider, templateCodeCompiler);

            // Act
            var actual = input.GetModel();

            // Assert
            actual.Should().NotBeNull();
            actual.Should().BeOfType<AssemblyDirectiveModel>();
        }

        [Fact]
        public void CanGetModelInstanceFromNonMappedTemplateSectionProcessor()
        {
            // Arrange
            var input = new FakeImportDirective<TemplateSectionProcessorExtensionsTests>();

            // Act
            var actual = input.GetModel();

            // Assert
            actual.Should().NotBeNull();
            actual.Should().BeOfType<ImportDirectiveModel>();
        }

        [Fact]
        public void CanGetDirectivePrefixFromMappedTemplateSectionProcessor()
        {
            // Arrange
            var fileNameProvider = Fixture.Freeze<IFileNameProvider>();
            var fileContentsProvider = Fixture.Freeze<IFileContentsProvider>();
            var templateCodeCompiler = Fixture.Freeze<ITemplateCodeCompiler<TemplateSectionProcessorExtensionsTests>>();
            var input = new TokenMapperAdapter<TemplateSectionProcessorExtensionsTests>(typeof(AssemblyTokenMapper<TemplateSectionProcessorExtensionsTests>), fileNameProvider, fileContentsProvider, templateCodeCompiler);

            // Act
            var actual = input.GetDirectivePrefix();

            // Assert
            actual.Should().Be("assembly");
        }

        [Fact]
        public void CanGetDirectivePrefixFromNonMappedTemplateSectionProcessor()
        {
            // Arrange
            var input = new FakeImportDirective<TemplateSectionProcessorExtensionsTests>();

            // Act
            var actual = input.GetDirectivePrefix();

            // Assert
            actual.Should().Be("import");
        }

        [Fact]
        public void IsProcessorForSection_Throws_On_Null_Context()
        {
            // Arrange
            var sut = Fixture.Freeze<ITemplateSectionProcessor<TemplateSectionProcessorExtensionsTests>>();

            // Act & Assert
            sut.Invoking(x => x.IsProcessorForSection(null))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }
    }
}
