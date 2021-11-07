using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens
{
    public class TempPathToken<TState> : TemplateToken<TState>, ITempPathToken<TState>
        where TState : class
    {
        public TempPathToken(SectionContext<TState> context,
                             string value,
                             string overrideFileName = null)
            : base(context, overrideFileName)
        {
            Value = value;
        }

        public string Value { get; }
    }
}
