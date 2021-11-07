using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.NamespaceFooterTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.NamespaceFooterTokens
{
    public class CompositionRootClassToken<TState> : TemplateToken<TState>, ICompositionRootClassToken<TState>
        where TState : class
    {
        public CompositionRootClassToken(SectionContext<TState> context, string className, string registrationMethodsAccessor)
            : base(context)
        {
            ClassName = className;
            RegistrationMethodsAccessor = registrationMethodsAccessor;
        }

        public string ClassName { get; }
        public string RegistrationMethodsAccessor { get; }
    }
}
