using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens
{
    public class CallAdditionalActionDelegateToken<TState> : TemplateToken<TState>, ICallAdditionalActionDelegateToken<TState>
        where TState : class
    {
        public bool SkipInitializationCode { get; }

        public CallAdditionalActionDelegateToken(SectionContext<TState> context, bool skipInitializationCode)
            : base(context)
        {
            SkipInitializationCode = skipInitializationCode;
        }
    }
}
