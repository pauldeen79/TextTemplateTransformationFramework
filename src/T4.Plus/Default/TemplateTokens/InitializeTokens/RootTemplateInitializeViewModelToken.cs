using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens.InitializeTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens
{
    public class RootTemplateInitializeViewModelToken<TState> : TemplateToken<TState>, IInitializeViewModelToken<TState>, IPostParameterInitializeToken<TState>
        where TState : class
    {
        public RootTemplateInitializeViewModelToken(SectionContext<TState> context,
                                                    ValueSpecifier viewModel,
                                                    ValueSpecifier model,
                                                    bool silentlyContinueOnError,
                                                    ValueSpecifier customResolverDelegate,
                                                    ValueSpecifier customResolverDelegateModel)
            : base(context)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (customResolverDelegate == null)
            {
                throw new ArgumentNullException(nameof(customResolverDelegate));
            }

            if (customResolverDelegateModel == null)
            {
                throw new ArgumentNullException(nameof(customResolverDelegateModel));
            }

            ViewModelName = viewModel.Value;
            ViewModelNameIsLiteral = viewModel.ValueIsLiteral;
            Model = model.Value;
            ModelIsLiteral = model.ValueIsLiteral;
            SilentlyContinueOnError = silentlyContinueOnError;
            CustomResolverDelegateExpression = customResolverDelegate.Value;
            CustomResolverDelegateExpressionIsLiteral = customResolverDelegate.ValueIsLiteral;
            ResolverDelegateModel = customResolverDelegateModel.Value;
            ResolverDelegateModelIsLiteral = customResolverDelegateModel.ValueIsLiteral;
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
