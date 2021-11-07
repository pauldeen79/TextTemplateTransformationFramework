using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.NamespaceFooterTokens
{
    public interface ICompositionRootClassToken<TState> : INamespaceFooterToken<TState>
        where TState : class
    {
        string ClassName { get; }
        string RegistrationMethodsAccessor { get; }
    }
}
