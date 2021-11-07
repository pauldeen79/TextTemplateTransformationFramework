using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.ChildTemplateInitializeCodeTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.ChildTemplateInitializeCodeTokens
{
    public class RouteChildTemplatesFieldToRootTemplateToken<TState> : TemplateToken<TState>, IRouteChildTemplatesFieldToRootTemplateToken<TState>
        where TState : class
    {
        public RouteChildTemplatesFieldToRootTemplateToken(SectionContext<TState> context)
            : base(context)
        {
        }
    }
}