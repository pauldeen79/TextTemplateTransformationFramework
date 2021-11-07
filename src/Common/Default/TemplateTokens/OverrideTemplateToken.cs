using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens
{
    public class OverrideTemplateToken<TState> : TemplateToken<TState>, IOverrideTemplateToken<TState>
        where TState : class
    {
        public OverrideTemplateToken(SectionContext<TState> context)
            : base(context)
        {
        }
    }
}
