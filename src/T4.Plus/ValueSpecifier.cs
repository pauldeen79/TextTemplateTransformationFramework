namespace TextTemplateTransformationFramework.T4.Plus
{
    public class ValueSpecifier
    {
        public ValueSpecifier(string value, bool valueIsLiteral)
        {
            Value = value;
            ValueIsLiteral = valueIsLiteral;
        }

        public string Value { get; }
        public bool ValueIsLiteral { get; }
    }
}
