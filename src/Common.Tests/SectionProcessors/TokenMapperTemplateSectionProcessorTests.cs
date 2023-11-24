using System;
using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using FluentAssertions;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.Common.SectionProcessors;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.SectionProcessors
{
    [ExcludeFromCodeCoverage]
    public class TokenMapperTemplateSectionProcessorTests : TestBase
    {
        [Fact]
        public void TokenMapperTemplateSectionProcessor_Using_MapperType_Without_TokenMapperAttribute_Causes_ArgumentException()
        {
            // Arrange
            var fileNameProvider = Fixture.Freeze<IFileNameProvider>();
            var fileContentsProvider = Fixture.Freeze<IFileContentsProvider>();
            var templateCodeCompiler = Fixture.Freeze<ITemplateCodeCompiler<TokenMapperTemplateSectionProcessorTests>>();
            var action = new Action(() => _ = new TokenMapperAdapter<TokenMapperTemplateSectionProcessorTests>(typeof(MapperWithoutTokenMapperAttribute), fileNameProvider, fileContentsProvider, templateCodeCompiler));

            // Act & Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void TokenMapperTemplateSectionProcessor_Using_MapperType_Without_DirectivePrefixAttribute_Causes_ArgumentException()
        {
            // Arrange
            var fileNameProvider = Fixture.Freeze<IFileNameProvider>();
            var fileContentsProvider = Fixture.Freeze<IFileContentsProvider>();
            var templateCodeCompiler = Fixture.Freeze<ITemplateCodeCompiler<TokenMapperTemplateSectionProcessorTests>>();
            var action = new Action(() => _ = new TokenMapperAdapter<TokenMapperTemplateSectionProcessorTests>(typeof(MapperWithoutDirectivePrefixAttribute), fileNameProvider, fileContentsProvider, templateCodeCompiler));

            // Act & Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void TokenMapperTemplateSectionProcessor_Using_MapperType_Without_Map_Method_Causes_ArgumentException()
        {
            // Arrange
            var fileNameProvider = Fixture.Freeze<IFileNameProvider>();
            var fileContentsProvider = Fixture.Freeze<IFileContentsProvider>();
            var templateCodeCompiler = Fixture.Freeze<ITemplateCodeCompiler<TokenMapperTemplateSectionProcessorTests>>();
            var action = new Action(() => _ = new TokenMapperAdapter<TokenMapperTemplateSectionProcessorTests>(typeof(MapperWithoutMapMethod), fileNameProvider, fileContentsProvider, templateCodeCompiler));

            // Act & Assert
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void TokenMapperTemplateSectionProcessor_Can_Be_Instanciated_With_Correct_Mapper_Implementation()
        {
            // Arrange & act
            var fileNameProvider = Fixture.Freeze<IFileNameProvider>();
            var fileContentsProvider = Fixture.Freeze<IFileContentsProvider>();
            var templateCodeCompiler = Fixture.Freeze<ITemplateCodeCompiler<TokenMapperTemplateSectionProcessorTests>>();
            var sut = new TokenMapperAdapter<TokenMapperTemplateSectionProcessorTests>(typeof(CorrectMapper), fileNameProvider, fileContentsProvider, templateCodeCompiler);

            // Assert
            sut.Should().NotBeNull();
        }

#pragma warning disable S2094 // Classes should not be empty
        private sealed class MapperWithoutTokenMapperAttribute
#pragma warning restore S2094 // Classes should not be empty
        {
        }

        [TokenMapper(typeof(object))]
        private sealed class MapperWithoutDirectivePrefixAttribute
        {
        }

        [TokenMapper(typeof(object))]
        [DirectivePrefix("test")]
        private sealed class MapperWithoutMapMethod
        {
        }

        [TokenMapper(typeof(object))]
        [DirectivePrefix("test")]
        private sealed class CorrectMapper : ISingleTokenMapper<TokenMapperTemplateSectionProcessorTests, object>
        {
            public ITemplateToken<TokenMapperTemplateSectionProcessorTests> Map(SectionContext<TokenMapperTemplateSectionProcessorTests> context, object model)
            {
                throw new NotImplementedException();
            }
        }
    }
}
