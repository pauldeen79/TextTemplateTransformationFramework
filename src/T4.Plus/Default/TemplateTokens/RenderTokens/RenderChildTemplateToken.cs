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
            string childTemplateName = null,
            bool childTemplateNameIsLiteral = true,
            string model = null,
            bool modelIsLiteral = false,
            bool? enumerable = null,
            bool silentlyContinueOnError = false,
            string separatorTemplateName = null,
            bool separatorTemplateNameIsLiteral = true,
            string headerTemplateName = null,
            bool headerTemplateNameIsLiteral = true,
            string headerCondition = null,
            string footerTemplateName = null,
            bool footerTemplateNameIsLiteral = true,
            string footerCondition = null,
            string customResolverDelegateExpression = null,
            bool customResolverDelegateExpressionIsLiteral = true,
            string resolverDelegateModel = null,
            bool resolverDelegateModelIsLiteral = false,
            string customRenderChildTemplateDelegateExpression = null,
            bool customRenderChildTemplateDelegateExpressionIsLiteral = true,
            string customTemplateNameDelegateExpression = null,
            bool customTemplateNameDelegateExpressionIsLiteral = true)
            : base(context)
        {
            ChildTemplateName = childTemplateName;
            ChildTemplateNameIsLiteral = childTemplateNameIsLiteral;
            Model = model;
            ModelIsLiteral = modelIsLiteral;
            Enumerable = enumerable;
            SilentlyContinueOnError = silentlyContinueOnError;
            SeparatorTemplateName = separatorTemplateName;
            SeparatorTemplateNameIsLiteral = separatorTemplateNameIsLiteral;
            HeaderTemplateName = headerTemplateName;
            HeaderTemplateNameIsLiteral = headerTemplateNameIsLiteral;
            HeaderCondition = headerCondition;
            FooterTemplateName = footerTemplateName;
            FooterTemplateNameIsLiteral = footerTemplateNameIsLiteral;
            FooterCondition = footerCondition;
            CustomResolverDelegateExpression = customResolverDelegateExpression;
            CustomResolverDelegateExpressionIsLiteral = customResolverDelegateExpressionIsLiteral;
            CustomRenderChildTemplateDelegateExpression = customRenderChildTemplateDelegateExpression;
            CustomRenderChildTemplateDelegateExpressionIsLiteral = customRenderChildTemplateDelegateExpressionIsLiteral;
            ResolverDelegateModel = resolverDelegateModel;
            ResolverDelegateModelIsLiteral = resolverDelegateModelIsLiteral;
            CustomTemplateNameDelegateExpression = customTemplateNameDelegateExpression;
            CustomTemplateNameDelegateExpressionIsLiteral = customTemplateNameDelegateExpressionIsLiteral;
        }

        public string ChildTemplateName { get; }
        public bool ChildTemplateNameIsLiteral { get; }
        public bool? Enumerable { get; }
        public string Model { get; }
        public bool ModelIsLiteral { get; }
        public bool SilentlyContinueOnError { get; }
        public string SeparatorTemplateName { get; }
        public bool SeparatorTemplateNameIsLiteral { get; }
        public string HeaderTemplateName { get; }
        public bool HeaderTemplateNameIsLiteral { get; }
        public string HeaderCondition { get; }
        public string FooterTemplateName { get; }
        public bool FooterTemplateNameIsLiteral { get; }
        public string FooterCondition { get; }
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
