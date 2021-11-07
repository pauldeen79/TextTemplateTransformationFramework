using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens
{
    public class OutputExtensionToken<TState> : TemplateToken<TState>, IOutputExtensionToken<TState>
        where TState : class
    {
        public OutputExtensionToken(SectionContext<TState> context, string extension)
            : base(context)
        {
            Extension = extension;
        }

        public string Extension { get; }
    }
}
