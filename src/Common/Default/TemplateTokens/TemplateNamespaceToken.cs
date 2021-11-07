using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens
{
    public class TemplateNamespaceToken<TState> : TemplateToken<TState>, ITemplateNamespaceToken<TState>
        where TState : class
    {
        public TemplateNamespaceToken(SectionContext<TState> context, string @namespace)
            : base(context)
        {
            Namespace = @namespace;
        }

        public string Namespace { get; }
    }
}
