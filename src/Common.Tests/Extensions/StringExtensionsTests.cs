using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens.InitializeTokens;
using TextTemplateTransformationFramework.Common.Extensions;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class StringExtensionsTests
    {
        [Theory,
            InlineData("", null, null),
            InlineData(null, null, null),
            InlineData("c#", typeof(LanguageToken<StringExtensionsTests>), Common.Language.CSharp),
            InlineData("C#", typeof(LanguageToken<StringExtensionsTests>), Common.Language.CSharp),
            InlineData("vb", typeof(LanguageToken<StringExtensionsTests>), Common.Language.VbNet),
            InlineData("VB", typeof(LanguageToken<StringExtensionsTests>), Common.Language.VbNet),
            InlineData("kaboom", typeof(InitializeErrorToken<StringExtensionsTests>), null)]
        public void GetLanguageToken_Returns_Correct_Result(string input, Type expectedResultType, Common.Language? expectedLanguage)
        {
            // Act
            var result = input.GetLanguageToken(SectionContext.FromCurrentMode(Mode.CodeRender, this));

            // Assert
            if (expectedResultType is null)
            {
                result.Should().BeNull();
            }
            else
            {
                result.Should().BeOfType(expectedResultType);
                if (expectedLanguage is not null)
                {
                    ((LanguageToken<StringExtensionsTests>)result).Value.Should().Be(expectedLanguage);
                }
            }
        }
    }
}
