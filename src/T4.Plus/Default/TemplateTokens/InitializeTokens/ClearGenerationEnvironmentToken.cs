using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens
{
    public class ClearGenerationEnvironmentToken<TState> : TemplateToken<TState>, IClearGenerationEnvironmentToken<TState>
        where TState : class
    {
        public ClearGenerationEnvironmentToken(SectionContext<TState> context)
            : base(context)
        {
        }
    }
}
