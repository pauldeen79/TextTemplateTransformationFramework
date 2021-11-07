using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens
{
    public class InitializeAddTemplateToPlaceholderToken<TState> : TemplateToken<TState>, IInitializeAddTemplateToPlaceholderToken<TState>
        where TState : class
    {
        public InitializeAddTemplateToPlaceholderToken(SectionContext<TState> context, string placeholderName, bool placeholderNameIsLiteral, string childTemplateName, bool childTemplateNameIsLiteral = true)
            : base(context)
        {
            PlaceholderName = placeholderName;
            PlaceholderNameIsLiteral = placeholderNameIsLiteral;
            ChildTemplateName = childTemplateName;
            ChildTemplateNameIsLiteral = childTemplateNameIsLiteral;
        }

        public string PlaceholderName { get; }
        public bool PlaceholderNameIsLiteral { get; }
        public string ChildTemplateName { get; }
        public bool ChildTemplateNameIsLiteral { get; }
    }
}
