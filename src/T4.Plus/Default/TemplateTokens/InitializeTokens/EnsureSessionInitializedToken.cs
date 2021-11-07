using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens
{
    public class EnsureSessionInitializedToken<TState> : TemplateToken<TState>, IEnsureSessionInitializedToken<TState>
        where TState : class
    {
        public EnsureSessionInitializedToken(SectionContext<TState> context)
            : base(context)
        {
        }
    }
}
