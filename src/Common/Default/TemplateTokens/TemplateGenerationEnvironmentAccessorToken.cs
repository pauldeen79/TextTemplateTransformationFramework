using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens
{
    public class TemplateGenerationEnvironmentAccessorToken<TState> : TemplateToken<TState>, ITemplateGenerationEnvironmentAccessorToken<TState>
        where TState : class
    {
        public TemplateGenerationEnvironmentAccessorToken(SectionContext<TState> context, string generationEnvironmentAccessor)
            : base(context)
        {
            GenerationEnvironmentAccessor = generationEnvironmentAccessor;
        }

        public string GenerationEnvironmentAccessor { get; }
    }
}
