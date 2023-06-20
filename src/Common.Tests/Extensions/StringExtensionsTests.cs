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
            InlineData("cs", typeof(LanguageToken<StringExtensionsTests>), Common.Language.CSharp),
            InlineData("CS", typeof(LanguageToken<StringExtensionsTests>), Common.Language.CSharp),
            InlineData("csharp", typeof(LanguageToken<StringExtensionsTests>), Common.Language.CSharp),
            InlineData("CSHARP", typeof(LanguageToken<StringExtensionsTests>), Common.Language.CSharp),
            InlineData("vb", typeof(LanguageToken<StringExtensionsTests>), Common.Language.VbNet),
            InlineData("VB", typeof(LanguageToken<StringExtensionsTests>), Common.Language.VbNet),
            InlineData("vbs", typeof(LanguageToken<StringExtensionsTests>), Common.Language.VbNet),
            InlineData("VBS", typeof(LanguageToken<StringExtensionsTests>), Common.Language.VbNet),
            InlineData("visualbasic", typeof(LanguageToken<StringExtensionsTests>), Common.Language.VbNet),
            InlineData("VISUALBASIC", typeof(LanguageToken<StringExtensionsTests>), Common.Language.VbNet),
            InlineData("vb.net", typeof(LanguageToken<StringExtensionsTests>), Common.Language.VbNet),
            InlineData("VB.NET", typeof(LanguageToken<StringExtensionsTests>), Common.Language.VbNet),
            InlineData("vbnet", typeof(LanguageToken<StringExtensionsTests>), Common.Language.VbNet),
            InlineData("VBNET", typeof(LanguageToken<StringExtensionsTests>), Common.Language.VbNet),
            InlineData("vbscript", typeof(LanguageToken<StringExtensionsTests>), Common.Language.VbNet),
            InlineData("VBSCRIPT", typeof(LanguageToken<StringExtensionsTests>), Common.Language.VbNet),
            InlineData("kaboom", typeof(InitializeErrorToken<StringExtensionsTests>), null)]
        public void GetLanguageToken_Returns_Correct_Result(string input, Type expectedResultType, Common.Language? expectedLanguage)
        {
            // Act
            var result = input.GetLanguageToken(SectionContext.FromCurrentMode(Mode.CodeRender, this));

            // Assert
            if (expectedResultType == null)
            {
                result.Should().BeNull();
            }
            else
            {
                result.Should().BeOfType(expectedResultType);
                if (expectedLanguage != null)
                {
                    ((LanguageToken<StringExtensionsTests>)result).Value.Should().Be(expectedLanguage);
                }
            }
        }
    }
}
