using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.Common.Default.TemplateTokens
{
    public class EnvironmentVersionToken<TState> : TemplateToken<TState>, IEnvironmentVersionToken<TState>
        where TState : class
    {
        public EnvironmentVersionToken(SectionContext<TState> context, string value)
            : base(context)
        {
            Value = value;
        }

        public string Value { get; }
    }
}
