namespace TextTemplateTransformationFramework.T4.Plus
{
    public class ComponentModelData
    {
        public ComponentModelData(bool browsable = true,
                                  bool readOnly = false,
                                  bool required = false,
                                  string displayName = null,
                                  string description = null,
                                  string category = null,
                                  TypeNameData typeNameData = null)
        {
            Browsable = browsable;
            ReadOnly = readOnly;
            Required = required;
            DisplayName = displayName;
            Description = description;
            TypeNameData = typeNameData ?? new();
            Category = category;
        }

        public bool Browsable { get; }
        public bool ReadOnly { get; }
        public bool Required { get; }
        public string DisplayName { get; }
        public string Description { get; }
        public TypeNameData TypeNameData { get; }
        public string Category { get; }
    }
}
