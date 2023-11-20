using System;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Attributes;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TokenMappers;
using TextTemplateTransformationFramework.Runtime.ComponentModel;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Models;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.T4.Plus.Mappers
{
    [TokenMapper(typeof(ShadowEnumPropertyDirectiveModel))]
    [DirectivePrefix("shadowEnumProperty")]
    public sealed class ShadowEnumPropertyTokenMapper<TState> : ISingleTokenMapper<TState, ShadowEnumPropertyDirectiveModel>
        where TState : class
    {
        public ITemplateToken<TState> Map(SectionContext<TState> context, ShadowEnumPropertyDirectiveModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return new ParameterToken<TState>
            (
                context,
                model.Name,
                "System.String",
                true,
                new(model.DefaultValue, true),
                model.OmitValueAssignment,
                model.AddPropertySetter,
                omitInitialization: false,
                componentModelData: new(
                    browsable: model.Browsable,
                    readOnly: model.ReadOnly,
                    required: model.Required,
                    displayName: model.DisplayName,
                    description: model.Description,
                    typeNameData: new(typeConverterTypeName: typeof(FormatStringConverter).FullName),
                    category: GetCategory(model))
            );
        }

        private static string GetCategory(ShadowEnumPropertyDirectiveModel model)
        {
            try
            {
                var type = GetEnumType(model.EnumTypeName);
                if (type is null)
                {
                    return "Error: Could not find type '" + model.EnumTypeName + "'";
                }
                return string.Join("|", Enum.GetValues(type).OfType<object>());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private static Type GetEnumType(string typeName)
        {
            foreach (var ass in AppDomain.CurrentDomain.GetAssemblies().Where(asm => !asm.IsDynamic
                        && !string.IsNullOrEmpty(asm.Location)
                        && !asm.FullName.IsUnitTestAssembly()))
            {
                var t = ass.GetType(typeName);
                if (t is not null)
                {
                    return t;
                }
            }

            return null;
        }
    }
}
