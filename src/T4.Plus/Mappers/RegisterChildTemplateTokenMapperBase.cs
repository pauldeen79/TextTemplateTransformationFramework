using System;
using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.InitializeTokens;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens.NamespaceFooterTokens;
using TextTemplateTransformationFramework.T4.Plus.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Models;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [RootTemplate]
    public abstract class RegisterChildTemplateTokenMapperBase<TState, T> : IMultipleTokenMapper<TState, T>, IFileContentsProviderContainer
        where TState : class
        where T : RegisterChildTemplateDirectiveModel<TState>
    {
        public IFileContentsProvider FileContentsProvider { get; set; }

        protected abstract bool GetIsViewModel();

        public IEnumerable<ITemplateToken<TState>> Map(SectionContext<TState> context, T model)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var templateTokens = context.GetChildTemplateTokens(FileContentsProvider, model.FileName);
            var baseClassTokenValue = templateTokens.ChildTokens.OfType<IBaseClassToken<TState>>().FirstOrDefault()?.BaseClassName;

            return templateTokens.RootTokens
                .Concat
                (
                    !GetIsViewModel()
                    ? new ITemplateToken<TState>[]
                    {
                        new RegisterChildTemplateToken<TState>
                        (
                            context,
                            model.FileName,
                            model.Name,
                            model.NameIsLiteral,
                            model.ModelTypeName.WhenNullOrEmpty(context.GetChildTemplateTokens(FileContentsProvider, model.FileName).ChildTokens.GetModelTypeName),
                            model.UseModelForRouting
                        ),
                        new ChildTemplateClassToken<TState>
                        (
                            context,
                            model.Name,
                            model.BaseClass.WhenNullOrEmpty($"{context.GetRootClassName()}Child"),
                            context.GetRootClassName(),
                            model.ModelTypeName.WhenNullOrEmpty(context.GetChildTemplateTokens(FileContentsProvider, model.FileName).ChildTokens.GetModelTypeName),
                            model.UseModelForRoutingOnly,
                            context.GetChildTemplateTokens(FileContentsProvider, model.FileName).ChildTokens
                        )
                    }
                    : new ITemplateToken<TState>[]
                    {
                        new RegisterViewModelToken<TState>
                        (
                            context,
                            model.FileName,
                            model.Name,
                            model.NameIsLiteral,
                            model.ModelTypeName.WhenNullOrEmpty(context.GetChildTemplateTokens(FileContentsProvider, model.FileName).ChildTokens.GetModelTypeName),
                            model.UseModelForRouting
                        ),
                        new NamespaceFooterChildViewModelClassToken<TState>
                        (
                            context,
                            model.Name,
                            baseClassTokenValue.WhenNullOrEmpty(model.BaseClass),
                            model.ModelTypeName.WhenNullOrEmpty(context.GetChildTemplateTokens(FileContentsProvider, model.FileName).ChildTokens.GetModelTypeName),
                            templateTokens.ChildTokens.OfType<ICopyPropertiesToViewModelToken<TState>>().All(t => t.Enabled),
                            templateTokens.ChildTokens
                        )
                    }
                );
        }
    }
}
