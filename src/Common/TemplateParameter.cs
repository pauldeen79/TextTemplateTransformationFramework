using System;
using System.Runtime.Serialization;

namespace TextTemplateTransformationFramework.Common
{
    /// <summary>
    /// Represents a parameter that is used in the template.
    /// </summary>
    [Serializable]
    public sealed class TemplateParameter : ISerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateParameter"/> class.
        /// </summary>
        public TemplateParameter()
        {
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the name of the editor attribute editor type.
        /// </summary>
        /// <value>
        /// The name of the editor attribute editor type.
        /// </value>
        public string EditorAttributeEditorTypeName { get; set; }

        /// <summary>
        /// Gets or sets the name of the editor attribute editor base type.
        /// </summary>
        /// <value>
        /// The name of the editor attribute editor base type.
        /// </value>
        public string EditorAttributeEditorBaseTypeName { get; set; }

        /// <summary>
        /// Gets or sets the type name of the type converter.
        /// </summary>
        /// <value>
        /// The type name of the type converter.
        /// </value>
        public string TypeConverterTypeName { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [omit value assignment].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [omit value assignment]; otherwise, <c>false</c>.
        /// </value>
        public bool OmitValueAssignment { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TemplateParameter"/> is browsable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if browsable; otherwise, <c>false</c>.
        /// </value>
        public bool Browsable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [read only].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [read only]; otherwise, <c>false</c>.
        /// </value>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets possible values
        /// </summary>
        public string[] PossibleValues { get; set; }

        #region ISerializable Members
        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateParameter"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        private TemplateParameter(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("name");
            Description = info.GetString("description");
            DisplayName = info.GetString("displayName");
            Browsable = info.GetBoolean("browsable");
            ReadOnly = info.GetBoolean("readOnly");
            var typeName = info.GetString("typeName");
            Type = Type.GetType(typeName);
            EditorAttributeEditorTypeName = info.GetString("editorAttributeEditorTypeName");
            EditorAttributeEditorBaseTypeName = info.GetString("editorAttributeEditorBaseTypeName");
            Value = GetValue(info);
            OmitValueAssignment = info.GetBoolean("omitValueAssignment");
            TypeConverterTypeName = info.GetString("typeConverterTypeName");
            PossibleValues = info.GetString("possibleValues").Split('|');
        }

        private object GetValue(SerializationInfo info) =>
            Type == null
                ? null
                : Convert.ChangeType
                (
                    GetValueToConvert(info),
                    GetConversionType()
                );

        private Type GetConversionType()
            => Type.IsEnum
                ? typeof(int)
                : Type;

        private object GetValueToConvert(SerializationInfo info)
            => Type.IsEnum && info.GetValue("value", typeof(object)) is string
                ? Enum.Parse(Type, info.GetValue("value", typeof(object)).ToString().Replace(Type + ".", string.Empty))
                : info.GetValue("value", typeof(object));

        /// <summary>
        /// Populates a <see cref="SerializationInfo" /> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo" /> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="StreamingContext" />) for this serialization.</param>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", Name);
            info.AddValue("value", Value);
            info.AddValue("description", Description);
            info.AddValue("displayName", DisplayName);
            info.AddValue("browsable", Browsable);
            info.AddValue("readOnly", ReadOnly);
            info.AddValue("typeName", Type.AssemblyQualifiedName);
            info.AddValue("editorAttributeEditorTypeName", EditorAttributeEditorTypeName);
            info.AddValue("editorAttributeEditorBaseTypeName", EditorAttributeEditorBaseTypeName);
            info.AddValue("typeConverterTypeName", TypeConverterTypeName);
            info.AddValue("omitValueAssignment", OmitValueAssignment);
            info.AddValue("possibleValues", PossibleValues == null ? string.Empty : string.Join("|", PossibleValues));
        }
        #endregion
    }
}
