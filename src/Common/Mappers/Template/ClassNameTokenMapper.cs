using System;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using TextTemplateTransformationFramework.Common.Models;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common.Mappers.Template
{
    [GroupedTokenMapper(typeof(TemplateDirectiveModel))]
    [DirectivePrefix("template")]
    public sealed class ClassName<TState> : ISingleTokenMapper<TState, TemplateDirectiveModel>, IContextValidatableTokenMapper<TState, TemplateDirectiveModel>
        where TState : class
    {
        public bool IsValidForProcessing(SectionContext<TState> context, TemplateDirectiveModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return !string.IsNullOrEmpty(model.ClassName);
        }

        public ITemplateToken<TState> Map(SectionContext<TState> context, TemplateDirectiveModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new TemplateClassNameToken<TState>
            (
                context,
                model.ClassName.GetClassNameWithDefault(),
                model.BaseClassName.WhenNullOrEmpty(model.ClassName.GetClassNameWithDefault() + "Base")
            );
        }
    }
}
