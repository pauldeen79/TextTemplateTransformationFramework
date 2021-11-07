using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.BaseClassFooterTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.BaseClassFooterTokens
{
    public class AllowNullExpressionsToken<TState> : TemplateToken<TState>, IAllowNullExpressionsToken<TState>
        where TState : class
    {
        public AllowNullExpressionsToken(SectionContext<TState> context, string templateClassName)
            : base(context)
        {
            TemplateClassName = templateClassName;
        }

        public string TemplateClassName { get; }
    }
}
