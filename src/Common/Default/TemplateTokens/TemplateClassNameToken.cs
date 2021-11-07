using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens
{
    public class TemplateClassNameToken<TState> : TemplateToken<TState>, ITemplateClassNameToken<TState>
        where TState : class
    {
        public TemplateClassNameToken(SectionContext<TState> context, string className, string baseClassName)
            : base(context)
        {
            ClassName = className;
            BaseClassName = baseClassName;
        }

        public string ClassName { get; }
        public string BaseClassName { get; }
    }
}
