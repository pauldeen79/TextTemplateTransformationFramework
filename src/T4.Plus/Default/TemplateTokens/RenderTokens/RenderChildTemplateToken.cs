using System;
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
            ValueSpecifier childTemplate,
            ValueSpecifier model,
            bool? enumerable,
            bool silentlyContinueOnError,
            ValueSpecifier separatorTemplate,
            CustomRenderData customRenderData)
            : base(context)
        {
            if (childTemplate == null)
            {
                throw new ArgumentNullException(nameof(childTemplate));
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (separatorTemplate == null)
            {
                throw new ArgumentNullException(nameof(separatorTemplate));
            }

            if (customRenderData == null)
            {
                throw new ArgumentNullException(nameof(customRenderData));
            }

            ChildTemplateName = childTemplate.Value;
            ChildTemplateNameIsLiteral = childTemplate.ValueIsLiteral;
            Model = model.Value;
            ModelIsLiteral = model.ValueIsLiteral;
            Enumerable = enumerable;
            SilentlyContinueOnError = silentlyContinueOnError;
            SeparatorTemplateName = separatorTemplate.Value;
            SeparatorTemplateNameIsLiteral = separatorTemplate.ValueIsLiteral;
            CustomResolverDelegateExpression = customRenderData.CustomResolverDelegate.Value;
            CustomResolverDelegateExpressionIsLiteral = customRenderData.CustomResolverDelegate.ValueIsLiteral;
            CustomRenderChildTemplateDelegateExpression = customRenderData.CustomRenderChildTemplateDelegate.Value;
            CustomRenderChildTemplateDelegateExpressionIsLiteral = customRenderData.CustomRenderChildTemplateDelegate.ValueIsLiteral;
            ResolverDelegateModel = customRenderData.ResolverDelegateModel.Value;
            ResolverDelegateModelIsLiteral = customRenderData.ResolverDelegateModel.ValueIsLiteral;
            CustomTemplateNameDelegateExpression = customRenderData.CustomTemplateNameDelegate.Value;
            CustomTemplateNameDelegateExpressionIsLiteral = customRenderData.CustomTemplateNameDelegate.ValueIsLiteral;
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
