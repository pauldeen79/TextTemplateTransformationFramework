using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens
{
    public class ClearChildTemplatesFieldToken<TState> : TemplateToken<TState>, IClearChildTemplatesFieldToken<TState>
        where TState : class
    {
        public ClearChildTemplatesFieldToken(SectionContext<TState> context)
            : base(context)
        {
        }
    }
}
