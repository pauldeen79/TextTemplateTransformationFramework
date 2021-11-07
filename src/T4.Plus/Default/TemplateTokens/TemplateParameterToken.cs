using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens
{
    public class TemplateParameterToken<TState> : TemplateToken<TState>, ITemplateParameterToken<TState>
        where TState : class
    {
        public TemplateParameterToken(SectionContext<TState> context, string name, string value)
            : base(context)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public string Value { get; }
    }
}
