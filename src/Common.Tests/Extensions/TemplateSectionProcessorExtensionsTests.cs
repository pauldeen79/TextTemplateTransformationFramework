using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.Common.Mappers;
using TextTemplateTransformationFramework.Common.Models;
using TextTemplateTransformationFramework.Common.SectionProcessors;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class TemplateSectionProcessorExtensionsTests
    {
        [Fact]
        public void CanGetModelInstanceFromMappedTemplateSectionProcessor()
        {
            // Arrange
            var fileNameProvider = new Mock<IFileNameProvider>().Object;
            var fileContentsProvider = new Mock<IFileContentsProvider>().Object;
            var templateCodeCompiler = new Mock<ITemplateCodeCompiler<TemplateSectionProcessorExtensionsTests>>().Object;
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
            var fileNameProvider = new Mock<IFileNameProvider>().Object;
            var fileContentsProvider = new Mock<IFileContentsProvider>().Object;
            var templateCodeCompiler = new Mock<ITemplateCodeCompiler<TemplateSectionProcessorExtensionsTests>>().Object;
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
    }
}
