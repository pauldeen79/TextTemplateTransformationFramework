using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens
{
    public class InitializeBaseToken<TState> : TemplateToken<TState>, IInitializeBaseToken<TState>
        where TState : class
    {
        public InitializeBaseToken(SectionContext<TState> context, bool additionalActionDelegate)
            : base(context)
        {
            AdditionalActionDelegate = additionalActionDelegate;
        }

        public bool AdditionalActionDelegate { get; }
    }
}
