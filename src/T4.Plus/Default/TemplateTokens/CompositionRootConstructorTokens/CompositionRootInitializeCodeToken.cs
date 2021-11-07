using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.CompositionRootConstructorTokens
{
    public class CompositionRootInitializeCodeToken<TState> : TemplateToken<TState>, ICompositionRootInitializeToken<TState>, ICodeToken<TState>
        where TState : class
    {
        public CompositionRootInitializeCodeToken(SectionContext<TState> context, string code)
            : base(context)
        {
            Code = code;
        }

        public string Code { get; }
    }
}
