using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.NamespaceFooterTokens;
using TextTemplateTransformationFramework.T4.Plus.Mappers;
using TextTemplateTransformationFramework.T4.Plus.Models;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Plus.Tests.Mappers
{
    [ExcludeFromCodeCoverage]
    public class RegisterChildTemplateDirectoryTokenMapperTests : TestBase
    {
        [Theory]
        [InlineData("MyBaseClass", "MyBaseClass")]
        [InlineData("", "GeneratedTemplateBaseChild")]
        public void Map_Returns_Correct_Tokens(string baseClassInput, string expectedBaseClassOutput)
        {
            // Arrange
            var fileNameProviderMock = Fixture.Freeze<IFileNameProvider>();
            var fileContentsProviderMock = Fixture.Freeze<IFileContentsProvider>();
            fileNameProviderMock.GetFiles(@"C:\", "*.template", true).Returns(new[] { @"C:\My.template" });
            fileContentsProviderMock.GetFileContents(@"C:\My.template").Returns(@"<# template language=""c#"" #>
Hello world!");
            var sut = new RegisterChildTemplateDirectoryTokenMapper<RegisterChildTemplateDirectoryTokenMapperTests>
            {
                FileNameProvider = fileNameProviderMock,
                FileContentsProvider = fileContentsProviderMock
            };
            var tokenParserCallbackMock = Fixture.Freeze<ITokenParserCallback<RegisterChildTemplateDirectoryTokenMapperTests>>();
            var loggerMock = Fixture.Freeze<ILogger>();
            var context = SectionContext.FromSection
            (
                new Section("test.template", 1, string.Empty),
                0,
                Enumerable.Empty<ITemplateToken<RegisterChildTemplateDirectoryTokenMapperTests>>(),
                tokenParserCallbackMock,
                this,
                loggerMock,
                Array.Empty<TemplateParameter>()
            );
            var model = new RegisterChildTemplateDirectoryDirectiveModel<RegisterChildTemplateDirectoryTokenMapperTests>()
            {
                BaseClass = baseClassInput,
                Path = @"C:\",
                Recurse = true,
                SearchPattern = "*.template",
                SectionContext = context
            };

            // Act
            var actual = sut.Map(context, model).ToArray();

            // Assert
            actual.Should().HaveCount(2);
            actual[0].Should().BeOfType<RegisterChildTemplateToken<RegisterChildTemplateDirectoryTokenMapperTests>>();
            actual[^1].Should().BeOfType<ChildTemplateClassToken<RegisterChildTemplateDirectoryTokenMapperTests>>();
            var childTemplateClassToken = actual[^1] as ChildTemplateClassToken<RegisterChildTemplateDirectoryTokenMapperTests>;
            if (childTemplateClassToken != null)
            {
                childTemplateClassToken.BaseClass.Should().Be(expectedBaseClassOutput);
            }
        }
    }
}
