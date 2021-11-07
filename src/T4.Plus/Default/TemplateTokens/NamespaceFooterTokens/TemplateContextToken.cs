using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.NamespaceFooterTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.NamespaceFooterTokens
{
    public class TemplateContextToken<TState> : TemplateToken<TState>, ITemplateContextToken<TState>
        where TState : class
    {
        public TemplateContextToken(SectionContext<TState> context)
            : base(context)
        {
        }
    }
}
