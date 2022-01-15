using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.RenderTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.RenderTokens
{
    public class RenderChildTemplateToken<TState> : TemplateToken<TState>, IRenderChildTemplateToken<TState>
        where TState : class
    {
        public RenderChildTemplateToken
        (
            SectionContext<TState> context,
            ValueSpecifier childTemplate = null,
            ValueSpecifier model = null,
            bool? enumerable = null,
            bool silentlyContinueOnError = false,
            ValueSpecifier separatorTemplate = null,
            ValueSpecifier customResolverDelegate = null,
            ValueSpecifier resolverDelegateModel = null,
            ValueSpecifier customRenderChildTemplateDelegate = null,
            ValueSpecifier customTemplateNameDelegate = null)
            : base(context)
        {
            ChildTemplateName = childTemplate?.Value;
            ChildTemplateNameIsLiteral = childTemplate?.ValueIsLiteral ?? true;
            Model = model?.Value;
            ModelIsLiteral = model?.ValueIsLiteral ?? true;
            Enumerable = enumerable;
            SilentlyContinueOnError = silentlyContinueOnError;
            SeparatorTemplateName = separatorTemplate?.Value;
            SeparatorTemplateNameIsLiteral = separatorTemplate?.ValueIsLiteral ?? true;
            CustomResolverDelegateExpression = customResolverDelegate?.Value;
            CustomResolverDelegateExpressionIsLiteral = customResolverDelegate?.ValueIsLiteral ?? true;
            CustomRenderChildTemplateDelegateExpression = customRenderChildTemplateDelegate?.Value;
            CustomRenderChildTemplateDelegateExpressionIsLiteral = customRenderChildTemplateDelegate?.ValueIsLiteral ?? true;
            ResolverDelegateModel = resolverDelegateModel?.Value;
            ResolverDelegateModelIsLiteral = resolverDelegateModel?.ValueIsLiteral ?? true;
            CustomTemplateNameDelegateExpression = customTemplateNameDelegate?.Value;
            CustomTemplateNameDelegateExpressionIsLiteral = customTemplateNameDelegate?.ValueIsLiteral ?? true;
        }

        public string ChildTemplateName { get; }
        public bool ChildTemplateNameIsLiteral { get; }
        public bool? Enumerable { get; }
        public string Model { get; }
        public bool ModelIsLiteral { get; }
        public bool SilentlyContinueOnError { get; }
        public string SeparatorTemplateName { get; }
        public bool SeparatorTemplateNameIsLiteral { get; }
        public string CustomResolverDelegateExpression { get; }
        public bool CustomResolverDelegateExpressionIsLiteral { get; }
        public string ResolverDelegateModel { get; }
        public bool ResolverDelegateModelIsLiteral { get; }
        public string CustomRenderChildTemplateDelegateExpression { get; }
        public bool CustomRenderChildTemplateDelegateExpressionIsLiteral { get; }
        public string CustomTemplateNameDelegateExpression { get; }
        public bool CustomTemplateNameDelegateExpressionIsLiteral { get; }
    }
}
