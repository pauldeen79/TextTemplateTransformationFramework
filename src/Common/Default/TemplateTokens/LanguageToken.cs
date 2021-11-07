using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens
{
    public class LanguageToken<TState> : TemplateToken<TState>, ILanguageToken<TState>
        where TState : class
    {
        public LanguageToken(SectionContext<TState> context, Language value, string sourceValue)
            : base(context)
        {
            Value = value;
            SourceValue = sourceValue;
        }

        public Language Value { get; }
        public string SourceValue { get; }
    }
}
