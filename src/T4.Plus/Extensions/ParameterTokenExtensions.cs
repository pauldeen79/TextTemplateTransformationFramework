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
                false /*instance.Browsable*/,
                instance.ReadOnly,
                instance.Required,
                instance.DisplayName,
                instance.Description,
                instance.OmitValueAssignment,
                true,
                instance.EditorAttributeEditorTypeName,
                instance.EditorAttributeEditorBaseType,
                instance.TypeConverterTypeName,
                instance.Category
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
                browsable: false
            );
    }
}
