using System;
using System.Runtime.Serialization;

namespace TextTemplateTransformationFramework.Common
{
    /// <summary>
    /// Represents a parameter that is used in the template.
    /// </summary>
    public sealed class TemplateParameter
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
    }
}
