using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens
{
    public class NamespaceImportToken<TState> : TemplateToken<TState>, INamespaceImportToken<TState>
        where TState : class
    {
        public NamespaceImportToken(SectionContext<TState> context, string @namespace)
            : base(context)
        {
            Namespace = @namespace;
        }

        public string Namespace { get; }
    }
}
