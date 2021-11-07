using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.NamespaceFooterTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.NamespaceFooterTokens
{
    public class TemplateFileManagerToken<TState> : TemplateToken<TState>, ITemplateFileManagerToken<TState>
        where TState : class
    {
        public TemplateFileManagerToken(SectionContext<TState> context)
            : base(context)
        {
        }
    }
}
