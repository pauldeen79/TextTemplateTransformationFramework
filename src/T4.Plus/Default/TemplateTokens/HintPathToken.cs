using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens
{
    public class HintPathToken<TState> : TemplateToken<TState>, IHintPathToken<TState>
        where TState : class
    {
        public HintPathToken(SectionContext<TState> context, string name, string hintPath, bool recursive)
            : base(context)
        {
            Name = name;
            HintPath = hintPath;
            Recursive = recursive;
        }

        public string Name { get; }
        public string HintPath { get; }
        public bool Recursive { get; }
    }
}
