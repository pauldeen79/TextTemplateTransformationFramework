using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.RenderTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.RenderTokens
{
    public class RenderChildTemplateToken<TState> : TemplateToken<TState>, IRenderChildTemplateToken<TState>
        where TState : class
    {
        public RenderChildTemplateToken
        (
            SectionContext<TState> context,
            RenderChildTemplateDirectiveModel model)
            : base(context)
        {
            ChildTemplateName = model.Name;
            ChildTemplateNameIsLiteral = model.NameIsLiteral;
            Model = model.Model;
            ModelIsLiteral = model.ModelIsLiteral;
            Enumerable = model.Enumerable;
            SilentlyContinueOnError = model.SilentlyContinueOnError;
            SeparatorTemplateName = model.SeparatorTemplateName;
            SeparatorTemplateNameIsLiteral = model.SeparatorTemplateNameIsLiteral;
            HeaderTemplateName = model.HeaderTemplateName;
            HeaderTemplateNameIsLiteral = model.HeaderTemplateNameIsLiteral;
            HeaderCondition = model.HeaderCondition;
            FooterTemplateName = model.FooterTemplateName;
            FooterTemplateNameIsLiteral = model.FooterTemplateNameIsLiteral;
            FooterCondition = model.FooterCondition;
            CustomResolverDelegateExpression = model.CustomResolverDelegate;
            CustomResolverDelegateExpressionIsLiteral = model.CustomResolverDelegateIsLiteral;
            CustomRenderChildTemplateDelegateExpression = model.CustomRenderChildTemplateDelegate;
            CustomRenderChildTemplateDelegateExpressionIsLiteral = model.CustomRenderChildTemplateDelegateIsLiteral;
            ResolverDelegateModel = model.ResolverDelegateModel;
            ResolverDelegateModelIsLiteral = model.ResolverDelegateModelIsLiteral;
            CustomTemplateNameDelegateExpression = model.CustomTemplateNameDelegate;
            CustomTemplateNameDelegateExpressionIsLiteral = model.CustomTemplateNameDelegateIsLiteral;
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
