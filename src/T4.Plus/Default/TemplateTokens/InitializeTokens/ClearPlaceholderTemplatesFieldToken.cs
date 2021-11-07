using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens
{
    public class ClearPlaceholderTemplatesFieldToken<TState> : TemplateToken<TState>, IClearPlaceholderTemplatesFieldToken<TState>
        where TState : class
    {
        public ClearPlaceholderTemplatesFieldToken(SectionContext<TState> context)
            : base(context)
        {
        }
    }
}
