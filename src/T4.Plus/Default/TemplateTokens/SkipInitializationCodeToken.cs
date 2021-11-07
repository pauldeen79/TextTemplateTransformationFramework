using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens
{
    public class SkipInitializationCodeToken<TState> : TemplateToken<TState>, ISkipInitializationCodeToken<TState>
        where TState : class
    {
        public SkipInitializationCodeToken(SectionContext<TState> context)
            : base(context)
        {
        }
    }
}
