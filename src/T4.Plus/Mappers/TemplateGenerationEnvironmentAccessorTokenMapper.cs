using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(TemplateGenerationEnvironmentAccessorDirectiveModel))]
    [DirectivePrefix("templateGenerationEnvironmentAccessor")]
    public sealed class TemplateGenerationEnvironmentAccessorTokenMapper<TState> : ISingleTokenMapper<TState, TemplateGenerationEnvironmentAccessorDirectiveModel>, IContextValidatableTokenMapper<TState, TemplateGenerationEnvironmentAccessorDirectiveModel>
        where TState : class
    {
        public bool IsValidForProcessing(SectionContext<TState> context, TemplateGenerationEnvironmentAccessorDirectiveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return !string.IsNullOrEmpty(model.Value);
        }

        public ITemplateToken<TState> Map(SectionContext<TState> context, TemplateGenerationEnvironmentAccessorDirectiveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new TemplateGenerationEnvironmentAccessorToken<TState>(context, model.Value);
        }
    }
}
