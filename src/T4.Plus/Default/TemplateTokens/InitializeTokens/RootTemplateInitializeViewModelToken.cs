using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens
{
    public class RootTemplateInitializeViewModelToken<TState> : TemplateToken<TState>, IInitializeViewModelToken<TState>, IPostParameterInitializeToken<TState>
        where TState : class
    {
        public RootTemplateInitializeViewModelToken(SectionContext<TState> context,
                                                    string viewModelName,
                                                    bool viewModelNameIsLiteral,
                                                    string model,
                                                    bool modelIsLiteral,
                                                    bool silentlyContinueOnError,
                                                    string customResolverDelegateExpression,
                                                    bool customResolverDelegateExpressionIsLiteral,
                                                    string resolverDelegateModel,
                                                    bool resolverDelegateModelIsLiteral,
                                                    bool addRootTemplatePrefix = false)
            : base(context)
        {
            ViewModelName = viewModelName;
            ViewModelNameIsLiteral = viewModelNameIsLiteral;
            Model = model;
            ModelIsLiteral = modelIsLiteral;
            SilentlyContinueOnError = silentlyContinueOnError;
            CustomResolverDelegateExpression = customResolverDelegateExpression;
            CustomResolverDelegateExpressionIsLiteral = customResolverDelegateExpressionIsLiteral;
            ResolverDelegateModel = resolverDelegateModel;
            ResolverDelegateModelIsLiteral = resolverDelegateModelIsLiteral;
            AddRootTemplatePrefix = addRootTemplatePrefix;
        }

        public string ViewModelName { get; }
        public bool ViewModelNameIsLiteral { get; }
        public bool CustomResolverDelegateExpressionIsLiteral { get; }
        public string ResolverDelegateModel { get; }
        public bool ResolverDelegateModelIsLiteral { get; }
        public bool AddRootTemplatePrefix { get; }
        public string Model { get; }
        public bool ModelIsLiteral { get; }
        public bool SilentlyContinueOnError { get; }
        public string CustomResolverDelegateExpression { get; }
    }
}
