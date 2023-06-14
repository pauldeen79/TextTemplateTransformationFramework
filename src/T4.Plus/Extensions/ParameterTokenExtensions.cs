using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Extensions
{
    public static class ParameterTokenExtensions
    {
        public static IParameterToken<TState> WithPropertySetter<TState>(this IParameterToken<TState> instance)
            where TState : class
            => new ParameterToken<TState>
            (
                instance.SectionContext,
                instance.Name,
                instance.TypeName,
                instance.NetCoreCompatible,
                instance.DefaultValue,
                instance.DefaultValueIsLiteral,
                instance.OmitValueAssignment,
                addPropertySetter: true,
                componentModelData: new(
                    browsable: false /*instance.Browsable*/,
                    readOnly: instance.ReadOnly,
                    required: instance.Required,
                    displayName:  instance.DisplayName,
                    description: instance.Description,
                    typeNameData: new(
                        editorAttributeEditorTypeName: instance.EditorAttributeEditorTypeName,
                        editorAttributeEditorBaseType: instance.EditorAttributeEditorBaseType,
                        typeConverterTypeName: instance.TypeConverterTypeName),
                    category: instance.Category)
            );

        public static TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.IParameterToken<TState> WithPropertySetter<TState>(this TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.IParameterToken<TState> instance)
            where TState : class
            => new ParameterToken<TState>
            (
                instance.SectionContext,
                instance.Name,
                instance.TypeName,
                instance.NetCoreCompatible,
                addPropertySetter: true,
                componentModelData: new(browsable: false)
            );
    }
}
