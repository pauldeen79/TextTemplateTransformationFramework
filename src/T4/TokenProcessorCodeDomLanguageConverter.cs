using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.T4.Contracts;

namespace TextTemplateTransformationFramework.T4
{
    public class TokenProcessorCodeDomLanguageConverter : ITokenProcessorCodeDomLanguageConverter
    {
        public string Convert(Language language)
            => language == Language.CSharp
                ? CodeDomLanguage.CSharp
                : null;
    }
}
