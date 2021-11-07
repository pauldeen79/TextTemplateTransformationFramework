using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
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
                model.TypeName.FixTypeName().MakeGenericTypeName(model.GenericParameterTypeName.FixTypeName()),
                model.NetCoreCompatible,
                model.DefaultValue,
                model.DefaultValueIsLiteral,
                model.Browsable,
                model.ReadOnly,
                model.Required,
                model.DisplayName,
                model.Description,
                model.OmitValueAssignment,
                model.AddPropertySetter,
                model.EditorAttributeEditorTypeName,
                model.EditorAttributeEditorBaseType,
                model.TypeConverterTypeName,
                model.Category,
                model.OmitInitialization
            );
        }
    }
}
