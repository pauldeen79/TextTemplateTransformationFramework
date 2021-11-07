using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(TemplateNameDirectiveModel))]
    [DirectivePrefix("templateName")]
    public sealed class TemplateNameTokenMapper<TState> : ISingleTokenMapper<TState, TemplateNameDirectiveModel>, IContextValidatableTokenMapper<TState, TemplateNameDirectiveModel>
        where TState : class
    {
        public bool IsValidForProcessing(SectionContext<TState> context, TemplateNameDirectiveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return !string.IsNullOrEmpty(model.Value);
        }

        public ITemplateToken<TState> Map(SectionContext<TState> context, TemplateNameDirectiveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new TemplateNameToken<TState>(context, model.Value);
        }
    }
}
