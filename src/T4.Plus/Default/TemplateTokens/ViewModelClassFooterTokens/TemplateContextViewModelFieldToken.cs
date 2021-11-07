using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.ViewModelClassFooterTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.ViewModelClassFooterTokens
{
    public class TemplateContextViewModelFieldToken<TState> : TemplateToken<TState>, ITemplateContextViewModelFieldToken<TState>
        where TState : class
    {
        public TemplateContextViewModelFieldToken(SectionContext<TState> context) : base(context)
        {
        }
    }
}
