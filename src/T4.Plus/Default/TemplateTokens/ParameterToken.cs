using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens
{
    public class ParameterToken<TState> : TextTemplateTransformationFramework.Common.Default.TemplateTokens.ParameterToken<TState>, IParameterToken<TState>
        where TState : class
    {
        public ParameterToken(SectionContext<TState> context,
                              string name,
                              string typeName,
                              bool netCoreCompatible = true,
                              string defaultValue = null,
                              bool defaultValueIsLiteral = true,
                              bool omitValueAssignment = false,
                              bool addPropertySetter = false,
                              bool omitInitialization = false,
                              ComponentModelData componentModelData = null)
            : base(context, name, typeName, netCoreCompatible)
        {
            DefaultValue = defaultValue;
            DefaultValueIsLiteral = defaultValueIsLiteral;
            Browsable = componentModelData?.Browsable ?? true;
            ReadOnly = componentModelData?.ReadOnly ?? false;
            Required = componentModelData?.Required ?? false;
            DisplayName = componentModelData?.DisplayName;
            Description = componentModelData?.Description;
            OmitValueAssignment = omitValueAssignment;
            AddPropertySetter = addPropertySetter;
            EditorAttributeEditorTypeName = componentModelData?.EditorAttributeEditorTypeName;
            EditorAttributeEditorBaseType = componentModelData?.EditorAttributeEditorBaseType;
            TypeConverterTypeName = componentModelData?.TypeConverterTypeName;
            Category = componentModelData?.Category;
            OmitInitialization = omitInitialization;
        }

        public string DefaultValue { get; }
        public bool Browsable { get; }
        public bool ReadOnly { get; }
        public bool Required { get; }
        public string DisplayName { get; }
        public string Description { get; }
        public bool OmitValueAssignment { get; }
        public bool DefaultValueIsLiteral { get; }
        public bool AddPropertySetter { get; }
        public string EditorAttributeEditorTypeName { get; }
        public string EditorAttributeEditorBaseType { get; }
        public string TypeConverterTypeName { get; }
        public string Category { get; }
        public bool OmitInitialization { get; }
    }
}
