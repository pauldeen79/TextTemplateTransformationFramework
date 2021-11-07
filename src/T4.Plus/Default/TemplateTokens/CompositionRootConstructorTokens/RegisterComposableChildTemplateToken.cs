using System;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.CompositionRootConstructorTokens;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.CompositionRootConstructorTokens
{
    public class RegisterComposableChildTemplateToken<TState> : TemplateToken<TState>, IRegisterComposableChildTemplateToken<TState>
        where TState : class
    {
        public RegisterComposableChildTemplateToken(IRegisterChildTemplateToken<TState> originalToken)
            : base(originalToken?.SectionContext)
        {
            if (originalToken == null)
            {
                throw new ArgumentNullException(nameof(originalToken));
            }
            ChildTemplateName = originalToken.ChildTemplateName;
            ChildTemplateNameIsLiteral = originalToken.ChildTemplateNameIsLiteral;
            ChildTemplateFileName = originalToken.ChildTemplateFileName;
            ModelTypeName = originalToken.ModelTypeName;
            UseForRouting = originalToken.UseForRouting;
        }

        public string ChildTemplateName { get; }
        public bool ChildTemplateNameIsLiteral { get; }
        public string ChildTemplateFileName { get; }
        public string ModelTypeName { get; }
        public bool UseForRouting { get; }
    }
}
