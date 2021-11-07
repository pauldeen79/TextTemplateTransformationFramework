using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.ChildTemplateRenderCodeTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.ChildTemplateRenderCodeTokens
{
    public class InitializeTemplateContextToken<TState> : TemplateToken<TState>, IInitializeTemplateContextToken<TState>
        where TState : class
    {
        public InitializeTemplateContextToken(SectionContext<TState> context)
            : base(context)
        {
        }
    }
}
