namespace TextTemplateTransformationFramework.T4.Plus
{
    public class TypeNameData
    {
        public TypeNameData(string editorAttributeEditorTypeName = null,
                            string editorAttributeEditorBaseType = null,
                            string typeConverterTypeName = null)
        {
            EditorAttributeEditorTypeName = editorAttributeEditorTypeName;
            EditorAttributeEditorBaseType = editorAttributeEditorBaseType;
            TypeConverterTypeName = typeConverterTypeName;
        }

        public string EditorAttributeEditorTypeName { get; }
        public string EditorAttributeEditorBaseType { get; }
        public string TypeConverterTypeName { get; }
    }
}
