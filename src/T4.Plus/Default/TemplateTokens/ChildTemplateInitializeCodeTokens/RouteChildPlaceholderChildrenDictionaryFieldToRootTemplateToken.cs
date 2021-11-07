using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.ChildTemplateInitializeCodeTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.ChildTemplateInitializeCodeTokens
{
    public class RouteChildPlaceholderChildrenDictionaryFieldToRootTemplateToken<TState> : TemplateToken<TState>, IRouteChildPlaceholderChildrenDictionaryFieldToRootTemplateToken<TState>
        where TState : class
    {
        public RouteChildPlaceholderChildrenDictionaryFieldToRootTemplateToken(SectionContext<TState> context)
            : base(context)
        {
        }
    }
}
