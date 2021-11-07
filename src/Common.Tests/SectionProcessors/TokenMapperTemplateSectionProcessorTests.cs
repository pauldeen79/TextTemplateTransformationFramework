using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.Common.SectionProcessors;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.SectionProcessors
{
    [ExcludeFromCodeCoverage]
    public class TokenMapperTemplateSectionProcessorTests
    {
        [Fact]
        public void TokenMapperTemplateSectionProcessor_Using_MapperType_Without_TokenMapperAttribute_Causes_ArgumentException()
        {
            // Arrange
            var fileNameProvider = new Mock<IFileNameProvider>().Object;
            var fileContentsProvider = new Mock<IFileContentsProvider>().Object;
            var templateCodeCompiler = new Mock<ITemplateCodeCompiler<TokenMapperTemplateSectionProcessorTests>>().Object;
            var action = new Action(() => _ = new TokenMapperAdapter<TokenMapperTemplateSectionProcessorTests>(typeof(MapperWithoutTokenMapperAttribute), fileNameProvider, fileContentsProvider, templateCodeCompiler));

            // Act & Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void TokenMapperTemplateSectionProcessor_Using_MapperType_Without_DirectivePrefixAttribute_Causes_ArgumentException()
        {
            // Arrange
            var fileNameProvider = new Mock<IFileNameProvider>().Object;
            var fileContentsProvider = new Mock<IFileContentsProvider>().Object;
            var templateCodeCompiler = new Mock<ITemplateCodeCompiler<TokenMapperTemplateSectionProcessorTests>>().Object;
            var action = new Action(() => _ = new TokenMapperAdapter<TokenMapperTemplateSectionProcessorTests>(typeof(MapperWithoutDirectivePrefixAttribute), fileNameProvider, fileContentsProvider, templateCodeCompiler));

            // Act & Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void TokenMapperTemplateSectionProcessor_Using_MapperType_Without_Map_Method_Causes_ArgumentException()
        {
            // Arrange
            var fileNameProvider = new Mock<IFileNameProvider>().Object;
            var fileContentsProvider = new Mock<IFileContentsProvider>().Object;
            var templateCodeCompiler = new Mock<ITemplateCodeCompiler<TokenMapperTemplateSectionProcessorTests>>().Object;
            var action = new Action(() => _ = new TokenMapperAdapter<TokenMapperTemplateSectionProcessorTests>(typeof(MapperWithoutMapMethod), fileNameProvider, fileContentsProvider, templateCodeCompiler));

            // Act & Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void TokenMapperTemplateSectionProcessor_Can_Be_Instanciated_With_Correct_Mapper_Implementation()
        {
            // Arrange & act
            var fileNameProvider = new Mock<IFileNameProvider>().Object;
            var fileContentsProvider = new Mock<IFileContentsProvider>().Object;
            var templateCodeCompiler = new Mock<ITemplateCodeCompiler<TokenMapperTemplateSectionProcessorTests>>().Object;
            var sut = new TokenMapperAdapter<TokenMapperTemplateSectionProcessorTests>(typeof(CorrectMapper), fileNameProvider, fileContentsProvider, templateCodeCompiler);

            // Assert
            sut.Should().NotBeNull();
        }

        private class MapperWithoutTokenMapperAttribute
        {
        }

        [TokenMapper(typeof(object))]
        private class MapperWithoutDirectivePrefixAttribute
        {
        }

        [TokenMapper(typeof(object))]
        [DirectivePrefix("test")]
        private class MapperWithoutMapMethod
        {
        }

        [TokenMapper(typeof(object))]
        [DirectivePrefix("test")]
        private class CorrectMapper : ISingleTokenMapper<TokenMapperTemplateSectionProcessorTests, object>
        {
            public ITemplateToken<TokenMapperTemplateSectionProcessorTests> Map(SectionContext<TokenMapperTemplateSectionProcessorTests> context, object model)
            {
                throw new NotImplementedException();
            }
        }
    }
}
