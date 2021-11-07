using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens
{
    public class RegisterChildTemplateToken<TState> : TemplateToken<TState>, IRegisterChildTemplateToken<TState>
        where TState : class
    {
        public RegisterChildTemplateToken(SectionContext<TState> context, string childTemplateFileName, string childTemplateName = null, bool childTemplateNameIsLiteral = true, string modelTypeName = null, bool useForRouting = true)
            : base(context)
        {
            ChildTemplateFileName = childTemplateFileName;
            ChildTemplateName = childTemplateName;
            ChildTemplateNameIsLiteral = childTemplateNameIsLiteral;
            ModelTypeName = modelTypeName;
            UseForRouting = useForRouting;
        }

        public string ChildTemplateName { get; }
        public bool ChildTemplateNameIsLiteral { get; }
        public string ChildTemplateFileName { get; }
        public string ModelTypeName { get; }
        public bool UseForRouting { get; }
    }
}
