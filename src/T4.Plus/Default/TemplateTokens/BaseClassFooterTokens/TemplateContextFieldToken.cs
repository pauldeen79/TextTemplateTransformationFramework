using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.BaseClassFooterTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.BaseClassFooterTokens
{
    public class TemplateContextFieldToken<TState> : TemplateToken<TState>, ITemplateContextFieldToken<TState>
        where TState : class
    {
        public TemplateContextFieldToken(SectionContext<TState> context) : base(context)
        {
        }
    }
}
