using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens
{
    public class RegisterPlaceholderToken<TState> : TemplateToken<TState>, IRegisterPlaceholderToken<TState>
        where TState : class
    {
        public RegisterPlaceholderToken(SectionContext<TState> context, string childTemplateName, bool childTemplateNameIsLiteral = true, string modelTypeName = null)
            : base(context)
        {
            ChildTemplateName = childTemplateName;
            ChildTemplateNameIsLiteral = childTemplateNameIsLiteral;
            ModelTypeName = modelTypeName;
        }

        public string ChildTemplateName { get; }
        public bool ChildTemplateNameIsLiteral { get; }
        public string ModelTypeName { get; }

        public int Order => 1;
    }
}
