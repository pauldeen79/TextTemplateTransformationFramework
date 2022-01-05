using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Moq;
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
    public class RegisterChildTemplateDirectoryTokenMapperTests
    {
        [Theory]
        [InlineData("MyBaseClass", "MyBaseClass")]
        [InlineData("", "GeneratedTemplateBaseChild")]
        public void Map_Returns_Correct_Tokens(string baseClassInput, string expectedBaseClassOutput)
        {
            // Arrange
            var fileNameProviderMock = new Mock<IFileNameProvider>();
            var fileContentsProviderMock = new Mock<IFileContentsProvider>();
            fileNameProviderMock.Setup(x => x.GetFiles(@"C:\", "*.template", true)).Returns(new[] { @"C:\My.template" });
            fileContentsProviderMock.Setup(x => x.GetFileContents(@"C:\My.template")).Returns(@"<# template language=""c#"" #>
Hello world!");
            var sut = new RegisterChildTemplateDirectoryTokenMapper<RegisterChildTemplateDirectoryTokenMapperTests>
            {
                FileNameProvider = fileNameProviderMock.Object,
                FileContentsProvider = fileContentsProviderMock.Object
            };
            var tokenParserCallbackMock = new Mock<ITokenParserCallback<RegisterChildTemplateDirectoryTokenMapperTests>>();
            var loggerMock = new Mock<ILogger>();
            var context = SectionContext.FromSection
            (
                "",
                0,
                1,
                "template.template",
                Enumerable.Empty<ITemplateToken<RegisterChildTemplateDirectoryTokenMapperTests>>(),
                tokenParserCallbackMock.Object,
                this,
                loggerMock.Object
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
            actual.First().Should().BeOfType<RegisterChildTemplateToken<RegisterChildTemplateDirectoryTokenMapperTests>>();
            actual.Last().Should().BeOfType<ChildTemplateClassToken<RegisterChildTemplateDirectoryTokenMapperTests>>();
            var childTemplateClassToken = actual.Last() as ChildTemplateClassToken<RegisterChildTemplateDirectoryTokenMapperTests>;
            if (childTemplateClassToken != null)
            {
                childTemplateClassToken.BaseClass.Should().Be(expectedBaseClassOutput);
            }
        }
    }
}
