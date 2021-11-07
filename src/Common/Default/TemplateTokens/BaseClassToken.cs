using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens
{
    public class BaseClassToken<TState> : TemplateToken<TState>, IBaseClassToken<TState>
        where TState : class
    {
        public BaseClassToken(SectionContext<TState> context, string baseClassName)
            : base(context)
        {
            BaseClassName = baseClassName;
        }

        public string BaseClassName { get; }
    }
}
