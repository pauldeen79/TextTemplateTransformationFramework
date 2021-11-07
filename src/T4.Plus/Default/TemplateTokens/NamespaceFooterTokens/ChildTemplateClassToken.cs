using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.NamespaceFooterTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.NamespaceFooterTokens
{
    public class ChildTemplateClassToken<TState> : TemplateToken<TState>, IChildTemplateClassToken<TState>
        where TState : class
    {
        public ChildTemplateClassToken(SectionContext<TState> context,
                                       string className,
                                       string baseClass,
                                       string rootClassName,
                                       string modelType = null,
                                       bool useModelForRoutingOnly = false,
                                       IEnumerable<ITemplateToken<TState>> childTemplateTokens = null)
            : base(context)
        {
            ClassName = className;
            BaseClass = baseClass;
            RootClassName = rootClassName;
            ModelType = modelType;
            UseModelForRoutingOnly = useModelForRoutingOnly;
            ChildTemplateTokens = childTemplateTokens.ToArray();
        }

        public string ClassName { get; }
        public string BaseClass { get; }
        public string RootClassName { get; }
        public string ModelType { get; }
        public bool UseModelForRoutingOnly { get; }

        public IEnumerable<ITemplateToken<TState>> ChildTemplateTokens { get; }
    }
}
