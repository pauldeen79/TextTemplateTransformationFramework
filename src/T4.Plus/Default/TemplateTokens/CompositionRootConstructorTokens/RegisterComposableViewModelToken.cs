using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.CompositionRootConstructorTokens;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.CompositionRootConstructorTokens
{
    public class RegisterComposableViewModelToken<TState> : TemplateToken<TState>, IRegisterComposableViewModelToken<TState>
        where TState : class
    {
        public RegisterComposableViewModelToken(IRegisterViewModelToken<TState> originalToken)
            : base(originalToken?.SectionContext)
        {
            if (originalToken != null)
            {
                ViewModelName = originalToken.ViewModelName;
                ViewModelNameIsLiteral = originalToken.ViewModelNameIsLiteral;
                ViewModelFileName = originalToken.ViewModelFileName;
                ModelTypeName = originalToken.ModelTypeName;
                UseForRouting = originalToken.UseForRouting;
            }
        }

        public string ViewModelName { get; }
        public bool ViewModelNameIsLiteral { get; }
        public string ViewModelFileName { get; }
        public string ModelTypeName { get; }
        public bool UseForRouting { get; }
    }
}
