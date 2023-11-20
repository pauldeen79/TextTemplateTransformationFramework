using System;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using TextTemplateTransformationFramework.Common.Models;

namespace TextTemplateTransformationFramework.Common.Mappers.Template
{
    [GroupedTokenMapper(typeof(TemplateDirectiveModel))]
    [DirectivePrefix("template")]
    public sealed class BaseClass<TState> : ISingleTokenMapper<TState, TemplateDirectiveModel>, IContextValidatableTokenMapper<TState, TemplateDirectiveModel>
        where TState : class
    {
        public bool IsValidForProcessing(SectionContext<TState> context, TemplateDirectiveModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return !string.IsNullOrEmpty(model.BaseClassName);
        }

        public ITemplateToken<TState> Map(SectionContext<TState> context, TemplateDirectiveModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new BaseClassToken<TState>(context, model.BaseClassName);
        }
    }
}
