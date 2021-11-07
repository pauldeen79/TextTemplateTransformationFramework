using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens
{
    public class PreLoadToken<TState> : TemplateToken<TState>, IPreLoadToken<TState>
        where TState : class
    {
        public PreLoadToken(SectionContext<TState> context, string name)
            : base(context)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
