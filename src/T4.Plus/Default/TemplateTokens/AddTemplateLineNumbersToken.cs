using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens
{
    public class AddTemplateLineNumbersToken<TState> : TemplateToken<TState>, IAddTemplateLineNumbersToken<TState>
        where TState : class
    {
        public AddTemplateLineNumbersToken(SectionContext<TState> context, bool enabled)
            : base(context)
        {
            Enabled = enabled;
        }

        public bool Enabled { get; }
    }
}
