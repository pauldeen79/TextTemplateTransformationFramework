namespace TextTemplateTransformationFramework.Core.Models;

internal class Model : IModel
{
    private object? _value;

    public object? Get() => _value;

    public void Initialize()
    {
        // Method left empty intentionally
    }

    public void Set(object? value) => _value = value;
}
