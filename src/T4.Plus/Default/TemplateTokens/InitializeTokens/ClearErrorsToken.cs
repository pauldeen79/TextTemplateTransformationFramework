using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens
{
    public class ClearErrorsToken<TState> : TemplateToken<TState>, IClearErrorsToken<TState>
        where TState : class
    {
        public ClearErrorsToken(SectionContext<TState> context)
            : base(context)
        {
        }

        public int Order => 0;
    }
}
