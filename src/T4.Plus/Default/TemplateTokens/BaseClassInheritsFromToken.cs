using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens
{
    public class BaseClassInheritsFromToken<TState> : TemplateToken<TState>, IBaseClassInheritsFromToken<TState>
        where TState : class
    {
        public BaseClassInheritsFromToken(SectionContext<TState> context, string className)
            : base(context)
        {
            ClassName = className;
        }

        public string ClassName { get; }
    }
}
