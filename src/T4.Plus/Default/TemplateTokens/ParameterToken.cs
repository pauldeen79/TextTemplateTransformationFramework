using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens;

namespace TextTemplateTransformationFramework.T4.Plus.Default.TemplateTokens
{
    public class ParameterToken<TState> : TextTemplateTransformationFramework.Common.Default.TemplateTokens.ParameterToken<TState>, IParameterToken<TState>
        where TState : class
    {
        public ParameterToken(SectionContext<TState> context, string name, string typeName, bool netCoreCompatible = true, string defaultValue = null, bool defaultValueIsLiteral = true, bool browsable = true, bool readOnly = false, bool required = false, string displayName = null, string description = null, bool omitValueAssignment = false, bool addPropertySetter = false, string editorAttributeEditorTypeName = null, string editorAttributeEditorBaseType = null, string typeConverterTypeName= null, string category = null, bool omitInitialization = false)
            : base(context, name, typeName, netCoreCompatible)
        {
            DefaultValue = defaultValue;
            DefaultValueIsLiteral = defaultValueIsLiteral;
            Browsable = browsable;
            ReadOnly = readOnly;
            Required = required;
            DisplayName = displayName;
            Description = description;
            OmitValueAssignment = omitValueAssignment;
            AddPropertySetter = addPropertySetter;
            EditorAttributeEditorTypeName = editorAttributeEditorTypeName;
            EditorAttributeEditorBaseType = editorAttributeEditorBaseType;
            TypeConverterTypeName = typeConverterTypeName;
            Category = category;
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
