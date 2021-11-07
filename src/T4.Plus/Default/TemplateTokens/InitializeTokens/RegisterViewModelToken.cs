using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens
{
    public class RegisterViewModelToken<TState> : TemplateToken<TState>, IRegisterViewModelToken<TState>
        where TState : class
    {
        public RegisterViewModelToken(SectionContext<TState> context, string viewModelFileName, string viewModelName = null, bool viewModelNameIsLiteral = true, string modelTypeName = null, bool useForRouting = true)
            : base(context)
        {
            ViewModelFileName = viewModelFileName;
            ViewModelName = viewModelName;
            ViewModelNameIsLiteral = viewModelNameIsLiteral;
            ModelTypeName = modelTypeName;
            UseForRouting = useForRouting;
        }

        public string ViewModelName { get; }
        public bool ViewModelNameIsLiteral { get; }
        public string ViewModelFileName { get; }
        public string ModelTypeName { get; }
        public bool UseForRouting { get; }
    }
}
