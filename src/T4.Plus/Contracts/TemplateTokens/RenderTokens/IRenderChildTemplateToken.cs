using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.RenderTokens
{
    public interface IRenderChildTemplateToken<TState> : IRenderToken<TState>
        where TState : class
    {
        string ChildTemplateName { get; }
        bool ChildTemplateNameIsLiteral { get; }
        bool? Enumerable { get; }
        string Model { get; }
        bool ModelIsLiteral { get; }
        bool SilentlyContinueOnError { get; }
        string SeparatorTemplateName { get; }
        bool SeparatorTemplateNameIsLiteral { get; }
        string HeaderTemplateName { get; }
        bool HeaderTemplateNameIsLiteral { get; }
        string HeaderCondition { get; }
        string FooterTemplateName { get; }
        bool FooterTemplateNameIsLiteral { get; }
        string FooterCondition { get; }
        string CustomResolverDelegateExpression { get; }
        bool CustomResolverDelegateExpressionIsLiteral { get; }
        string ResolverDelegateModel { get; }
        bool ResolverDelegateModelIsLiteral { get; }
        string CustomRenderChildTemplateDelegateExpression { get; }
        bool CustomRenderChildTemplateDelegateExpressionIsLiteral { get; }
        string CustomTemplateNameDelegateExpression { get; }
        bool CustomTemplateNameDelegateExpressionIsLiteral { get; }
    }
}
