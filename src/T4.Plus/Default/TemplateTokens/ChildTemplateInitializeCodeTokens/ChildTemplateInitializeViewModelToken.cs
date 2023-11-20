using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.ChildTemplateInitializeCodeTokens
{
    public class ChildTemplateInitializeViewModelToken<TState> : TemplateToken<TState>, IInitializeViewModelToken<TState>, IChildInitializeToken<TState>
        where TState : class
    {
        public ChildTemplateInitializeViewModelToken(SectionContext<TState> context,
                                                     ValueSpecifier viewModel,
                                                     ValueSpecifier model,
                                                     bool silentlyContinueOnError,
                                                     ValueSpecifier customResolverDelegate,
                                                     ValueSpecifier resolverDelegateModel)
            : base(context)
        {
            if (viewModel is null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            if (customResolverDelegate is null)
            {
                throw new ArgumentNullException(nameof(customResolverDelegate));
            }

            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (resolverDelegateModel is null)
            {
                throw new ArgumentNullException(nameof(resolverDelegateModel));
            }

            ViewModelName = viewModel.Value;
            ViewModelNameIsLiteral = viewModel.ValueIsLiteral;
            Model = model.Value;
            ModelIsLiteral = model.ValueIsLiteral;
            SilentlyContinueOnError = silentlyContinueOnError;
            CustomResolverDelegateExpression = customResolverDelegate.Value;
            CustomResolverDelegateExpressionIsLiteral = customResolverDelegate.ValueIsLiteral;
            ResolverDelegateModel = resolverDelegateModel.Value;
            ResolverDelegateModelIsLiteral = resolverDelegateModel.ValueIsLiteral;
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
