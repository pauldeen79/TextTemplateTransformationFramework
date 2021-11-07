using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.NamespaceFooterTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.NamespaceFooterTokens
{
    /// <summary>
    /// Renders a placeholder class.
    /// </summary>
    /// <seealso cref="Contracts.TemplateTokens.IPlaceholderClassToken" />
    public class PlaceholderClassToken<TState> : TemplateToken<TState>, IPlaceholderClassToken<TState>
        where TState : class
    {
        public PlaceholderClassToken(SectionContext<TState> context,
                                     string className,
                                     string baseClass,
                                     string rootClassName,
                                     string modelType = null,
                                     bool useModelForRoutingOnly = false)
            : base(context)
        {
            ClassName = className;
            BaseClass = baseClass;
            RootClassName = rootClassName;
            ModelType = modelType;
            UseModelForRoutingOnly = useModelForRoutingOnly;
        }

        public string ClassName { get; }
        public string BaseClass { get; }
        public string RootClassName { get; }
        public string ModelType { get; }
        public bool UseModelForRoutingOnly { get; }
    }
}
