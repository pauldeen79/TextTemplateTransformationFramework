using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens
{
    public class ParameterPropertiesToken<TState> : TemplateToken<TState>, IParameterPropertiesToken<TState>
        where TState : class
    {
        public ParameterPropertiesToken(SectionContext<TState> context)
            : base(context)
        {
        }
    }
}
