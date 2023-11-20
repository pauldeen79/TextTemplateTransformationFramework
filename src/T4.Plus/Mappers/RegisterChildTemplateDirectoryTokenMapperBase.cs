using System;
using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.NamespaceFooterTokens;
using TextTemplateTransformationFramework.T4.Plus.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Models;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [RootTemplate]
    public abstract class RegisterChildTemplateDirectoryTokenMapperBase<TState, TModel> : IMultipleTokenMapper<TState, TModel>, IFileNameProviderContainer, IFileContentsProviderContainer
        where TState : class
        where TModel : RegisterChildTemplateDirectoryDirectiveModel<TState>
    {
        public IFileNameProvider FileNameProvider { get; set; }
        public IFileContentsProvider FileContentsProvider { get; set; }

        protected abstract bool GetIsViewModel();

        public IEnumerable<ITemplateToken<TState>> Map(SectionContext<TState> context, TModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return FileNameProvider
                .GetFiles(model.Path, model.SearchPattern, model.Recurse)
                .SelectMany(file => GetTokensForFile(model, file));
        }

        private IEnumerable<ITemplateToken<TState>> GetTokensForFile(TModel model, string childTemplateFileName)
        {
            //Parse the included file, and add those tokens
            var childTemplateTokens = model.SectionContext.GetChildTemplateTokens(FileContentsProvider, childTemplateFileName);
            var childTemplateName = childTemplateTokens.ChildTokens.GetTemplateName();
            var modelTypeName = childTemplateTokens.ChildTokens.GetModelTypeName();
            var useModelForRoutingOnly = childTemplateTokens.ChildTokens.GetUseModelForRoutingOnly(modelTypeName);
            var useModelForRouting = childTemplateTokens.ChildTokens.GetUseModelForRouting(modelTypeName);
            var rootClassName = model.SectionContext.ExistingTokens.GetTemplateTokensFromSections().GetRootClassName();
            var copyPropertiesFromTemplate = childTemplateTokens.ChildTokens.OfType<ICopyPropertiesToViewModelToken<TState>>().All(t => t.Enabled);
            var baseClassTokenValue = childTemplateTokens.ChildTokens.OfType<IBaseClassToken<TState>>().FirstOrDefault()?.BaseClassName;

            foreach (var token in childTemplateTokens.RootTokens)
            {
                yield return token;
            }

            if (!GetIsViewModel())
            {
                yield return new RegisterChildTemplateToken<TState>
                (
                    model.SectionContext,
                    childTemplateFileName,
                    childTemplateName,
                    true,
                    modelTypeName,
                    useModelForRouting
                );

                yield return new ChildTemplateClassToken<TState>
                (
                    model.SectionContext,
                    childTemplateName,
                    model.BaseClass.WhenNullOrEmpty($"{rootClassName}Child"),
                    rootClassName,
                    modelTypeName,
                    useModelForRoutingOnly,
                    childTemplateTokens.ChildTokens
                );
            }
            else
            {
                yield return new RegisterViewModelToken<TState>
                (
                    model.SectionContext,
                    childTemplateFileName,
                    childTemplateName,
                    true,
                    modelTypeName,
                    useModelForRouting
                );

                yield return new NamespaceFooterChildViewModelClassToken<TState>
                (
                    model.SectionContext,
                    childTemplateName,
                    baseClassTokenValue.WhenNullOrEmpty(model.BaseClass),
                    modelTypeName,
                    copyPropertiesFromTemplate,
                    childTemplateTokens.ChildTokens
                );
            }
        }
    }
}
