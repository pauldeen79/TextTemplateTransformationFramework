using System;
using System.Linq;
using FluentAssertions;
using Moq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.RenderTokens;
using TextTemplateTransformationFramework.T4.Plus.Mappers;
using TextTemplateTransformationFramework.T4.Plus.Models;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.Mappers
{
    public class GeneratorAssemblyTokenMapperTests
    {
        [Fact]
        public void Map_Throws_On_Null_Argument()
        {
            // Arrange
            var sut = new GeneratorAssemblyTokenMapper<GeneratorAssemblyTokenMapperTests>();
            var context = CreateContext();

            // Act & Assert
            sut.Invoking(x => x.Map(context, null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Map_Returns_Correct_Tokens()
        {
            // Arrange
            var sut = new GeneratorAssemblyTokenMapper<GeneratorAssemblyTokenMapperTests>();
            var context = CreateContext();
            var model = new GeneratorAssemblyDirectiveModel()
            {
                AssemblyName = GetType().AssemblyQualifiedName,
                BasePath = @"C:\",
                CurrentDirectory = @"D:\",
                DryRun = true,
                GenerateMultipleFiles = false,
            };

            // Act
            var actual = sut.Map(context, model);

            // Assert
            actual.Should().BeOfType<RenderGeneratorAssemblyToken<GeneratorAssemblyTokenMapperTests>>();
            ((RenderGeneratorAssemblyToken<GeneratorAssemblyTokenMapperTests>)actual).AssemblyName.Should().Be(model.AssemblyName);
            ((RenderGeneratorAssemblyToken<GeneratorAssemblyTokenMapperTests>)actual).BasePath.Should().Be(model.BasePath);
            ((RenderGeneratorAssemblyToken<GeneratorAssemblyTokenMapperTests>)actual).CurrentDirectory.Should().Be(model.CurrentDirectory);
            ((RenderGeneratorAssemblyToken<GeneratorAssemblyTokenMapperTests>)actual).DryRun.Should().Be(model.DryRun);
            ((RenderGeneratorAssemblyToken<GeneratorAssemblyTokenMapperTests>)actual).GenerateMultipleFiles.Should().Be(model.GenerateMultipleFiles);
        }

        private SectionContext<GeneratorAssemblyTokenMapperTests> CreateContext()
        {
            var tokenParserCallbackMock = new Mock<ITokenParserCallback<GeneratorAssemblyTokenMapperTests>>();
            var loggerMock = new Mock<ILogger>();
            var context = SectionContext.FromSection
            (
                new Section("test.template", 1, string.Empty),
                0,
                Enumerable.Empty<ITemplateToken<GeneratorAssemblyTokenMapperTests>>(),
                tokenParserCallbackMock.Object,
                this,
                loggerMock.Object,
                Array.Empty<TemplateParameter>()
            );
            return context;
        }
    }
}
