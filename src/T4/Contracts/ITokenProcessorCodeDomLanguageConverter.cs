using TextTemplateTransformationFramework.Common;

namespace TextTemplateTransformationFramework.T4.Contracts
{
    public interface ITokenProcessorCodeDomLanguageConverter
    {
        string Convert(Language language);
    }
}
