using System.Collections.Generic;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.NamespaceFooterTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.NamespaceFooterTokens
{
    public class NamespaceFooterChildViewModelClassToken<TState> : ChildViewModelClassToken<TState>, IChildViewModelNamespaceFooterClassToken<TState>
        where TState : class
    {
        public NamespaceFooterChildViewModelClassToken(SectionContext<TState> context,
                                                       string className,
                                                       string baseClass,
                                                       string modelType,
                                                       bool copyPropertiesFromTemplate,
                                                       IEnumerable<ITemplateToken<TState>> childTemplateTokens = null)
            : base(context,
                   className,
                   baseClass,
                   modelType,
                   copyPropertiesFromTemplate,
                   childTemplateTokens)
        {
        }
    }
}
