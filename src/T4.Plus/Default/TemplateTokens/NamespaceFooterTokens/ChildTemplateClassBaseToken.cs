using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.NamespaceFooterTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.NamespaceFooterTokens
{
    public class ChildTemplateClassBaseToken<TState> : TemplateToken<TState>, IChildTemplateClassBaseToken<TState>
        where TState : class
    {
        public ChildTemplateClassBaseToken(SectionContext<TState> context, string className)
            : base(context)
        {
            ClassName = className;
        }

        public string ClassName { get; }
    }
}
