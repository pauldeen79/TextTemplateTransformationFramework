using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(ParameterPropertiesDirectiveModel))]
    [DirectivePrefix("parameterProperties")]
    public sealed class ParameterPropertiesTokenMapper<TState> : ISingleTokenMapper<TState, ParameterPropertiesDirectiveModel>, IContextValidatableTokenMapper<TState, ParameterPropertiesDirectiveModel>
        where TState : class
    {
        public bool IsValidForProcessing(SectionContext<TState> context, ParameterPropertiesDirectiveModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return model.Enabled;
        }

        public ITemplateToken<TState> Map(SectionContext<TState> context, ParameterPropertiesDirectiveModel model)
            => new ParameterPropertiesToken<TState>(context);
    }
}
