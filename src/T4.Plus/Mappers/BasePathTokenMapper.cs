using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(BasePathDirectiveModel))]
    [DirectivePrefix("basePath")]
    public sealed class BasePathTokenMapper<TState> : ISingleTokenMapper<TState, BasePathDirectiveModel>, IContextValidatableTokenMapper<TState, BasePathDirectiveModel>
        where TState : class
    {
        public bool IsValidForProcessing(SectionContext<TState> context, BasePathDirectiveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return !string.IsNullOrEmpty(model.Value);
        }

        public ITemplateToken<TState> Map(SectionContext<TState> context, BasePathDirectiveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new TemplateBasePathToken<TState>(context, model.Value);
        }
    }
}
