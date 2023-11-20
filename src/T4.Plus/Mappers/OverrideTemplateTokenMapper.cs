using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(OverrideTemplateDirectiveModel))]
    [DirectivePrefix("overrideTemplate")]
    public sealed class OverrideTemplateTokenMapper<TState> : ISingleTokenMapper<TState, OverrideTemplateDirectiveModel>, IContextValidatableTokenMapper<TState, OverrideTemplateDirectiveModel>
        where TState : class
    {
        public bool IsValidForProcessing(SectionContext<TState> context, OverrideTemplateDirectiveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return model.Enabled;
        }

        public ITemplateToken<TState> Map(SectionContext<TState> context, OverrideTemplateDirectiveModel model)
            => new OverrideTemplateToken<TState>(context);
    }
}
