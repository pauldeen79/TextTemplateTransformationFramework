using System;
using System.ComponentModel;
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
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new ParameterToken<TState>
            (
                context,
                model.Name,
                model.TypeName.FixTypeName().MakeGenericTypeName(model.GenericParameterTypeName.FixTypeName()),
                model.NetCoreCompatible,
                new(model.DefaultValue, model.DefaultValueIsLiteral),
                model.OmitValueAssignment,
                model.AddPropertySetter,
                model.OmitInitialization,
                componentModelData: new (browsable: model.Browsable,
                    readOnly: model.ReadOnly,
                    required: model.Required,
                    displayName: model.DisplayName,
                    description: model.Description,
                    typeNameData: new(
                        editorAttributeEditorTypeName: model.EditorAttributeEditorTypeName,
                        editorAttributeEditorBaseType: model.EditorAttributeEditorBaseType,
                        typeConverterTypeName: model.TypeConverterTypeName),
                    category: model.Category)
            );
        }
    }
}
