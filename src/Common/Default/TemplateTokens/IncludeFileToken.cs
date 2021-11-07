using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens
{
    public class IncludeFileToken<TState> : TemplateToken<TState>, IIncludeFileToken<TState>
        where TState : class
    {
        public IncludeFileToken(SectionContext<TState> context, string includeFileName)
            : base(context)
        {
            IncludeFileName = includeFileName;
        }

        public string IncludeFileName { get; }
    }
}
