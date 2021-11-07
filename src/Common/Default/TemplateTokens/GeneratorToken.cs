using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens
{
    public class GeneratorToken<TState> : TemplateToken<TState>, IGeneratorToken<TState>
        where TState : class
    {
        public GeneratorToken(SectionContext<TState> context, string name, string version)
            : base(context)
        {
            Name = name;
            Version = version;
        }

        public string Name { get; }

        public string Version { get; }
    }
}
