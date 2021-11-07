namespace TextTemplateTransformationFramework.T4.Plus.Contracts.TemplateTokens
{
    /// <summary>
    /// Contract for a parameter token with additional features.
    /// </summary>
    /// <seealso cref="IParameterToken" />
    public interface IParameterToken<TState> : TextTemplateTransformationFramework.Common.Contracts.TemplateTokens.IParameterToken<TState>
        where TState : class
    {
        string DefaultValue { get; }
        bool DefaultValueIsLiteral { get; }
        bool Browsable { get; }
        bool ReadOnly { get; }
        bool Required { get; }
        string DisplayName { get; }
        string Description { get; }
        bool OmitValueAssignment { get; }
        bool AddPropertySetter { get; }
        string EditorAttributeEditorTypeName { get; }
        string EditorAttributeEditorBaseType { get; }
        string TypeConverterTypeName { get; }
        string Category { get; }
        bool OmitInitialization { get; }
    }
}
