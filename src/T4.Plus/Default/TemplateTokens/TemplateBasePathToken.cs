using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens
{
    public class TemplateBasePathToken<TState> : TemplateToken<TState>, ITemplateBasePathToken<TState>
        where TState : class
    {
        public TemplateBasePathToken(SectionContext<TState> context, string basePath)
            : base(context)
        {
            BasePath = basePath;
        }

        public string BasePath { get; }
    }
}
