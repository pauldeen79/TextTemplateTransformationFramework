using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.T4.Plus.Extensions;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(RenderChildTemplateDirectiveModel))]
    [DirectivePrefix("renderChildTemplate")]
    public sealed class RenderChildTemplate<TState> : ISingleTokenMapper<TState, RenderChildTemplateDirectiveModel>
        where TState : class
    {
        public ITemplateToken<TState> Map(SectionContext<TState> context, RenderChildTemplateDirectiveModel model)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return context.CreateRenderChildTemplateToken
            (
                model.Name,
                model.NameIsLiteral,
                model.Model,
                model.ModelIsLiteral,
                model.Enumerable,
                model.SilentlyContinueOnError,
                model.SeparatorTemplateName,
                model.SeparatorTemplateNameIsLiteral,
                model.HeaderTemplateName,
                model.HeaderTemplateNameIsLiteral,
                model.HeaderCondition,
                model.FooterTemplateName,
                model.FooterTemplateNameIsLiteral,
                model.FooterCondition,
                model.CustomResolverDelegate,
                model.CustomResolverDelegateIsLiteral,
                model.ResolverDelegateModel,
                model.ResolverDelegateModelIsLiteral,
                model.CustomRenderChildTemplateDelegate,
                model.CustomRenderChildTemplateDelegateIsLiteral,
                model.CustomTemplateNameDelegate,
                model.CustomTemplateNameDelegateIsLiteral
            );
        }
    }
}
