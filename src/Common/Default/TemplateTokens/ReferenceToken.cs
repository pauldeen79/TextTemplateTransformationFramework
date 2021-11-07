using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens
{
    public class ReferenceToken<TState> : TemplateToken<TState>, IReferenceToken<TState>
        where TState : class
    {
        public ReferenceToken(SectionContext<TState> context, string name, string overrideFileName = null)
            : base(context, overrideFileName)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
