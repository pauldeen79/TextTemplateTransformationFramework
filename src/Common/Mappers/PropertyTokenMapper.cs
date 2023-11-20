using System;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using TextTemplateTransformationFramework.Common.Models;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common.Mappers
{
    [TokenMapper(typeof(PropertyDirectiveModel))]
    [DirectivePrefix("property")]
    public sealed class PropertyTokenMapper<TState> : ISingleTokenMapper<TState, PropertyDirectiveModel>
        where TState : class
    {
        public ITemplateToken<TState> Map(SectionContext<TState> context, PropertyDirectiveModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new ParameterToken<TState>
            (
                context,
                model.Name,
                model.TypeName.FixTypeName().MakeGenericTypeName(model.GenericParameterTypeName.FixTypeName())
            );
        }
    }
}
