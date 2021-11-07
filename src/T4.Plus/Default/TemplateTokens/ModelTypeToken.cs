using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens
{
    public class ModelTypeToken<TState> : TemplateToken<TState>, IModelTypeToken<TState>
        where TState : class
    {
        public ModelTypeToken(SectionContext<TState> context, string modelTypeName, bool useForRoutingOnly = false, bool useForRouting = true)
            : base(context)
        {
            ModelTypeName = modelTypeName;
            UseForRoutingOnly = useForRoutingOnly;
            UseForRouting = useForRouting;
        }

        public string ModelTypeName { get; }
        public bool UseForRoutingOnly { get; }
        public bool UseForRouting { get; }
    }
}
