using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens
{
    public class TemplateNameToken<TState> : TemplateToken<TState>, ITemplateNameToken<TState>
        where TState : class
    {
        public TemplateNameToken(SectionContext<TState> context, string templateName)
            : base(context)
        {
            TemplateName = templateName;
        }

        public string TemplateName { get; }
    }
}
