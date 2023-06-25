namespace TextTemplateTransformationFramework.Core.Extensions;

public static class ObjectExtensions
{
    public static IEnumerable<KeyValuePair<string, object?>> ToKeyValuePairs(this object? instance)
    {
        var result = new Dictionary<string, object?>();

        if (instance is null)
        {
            return result;
        }

        if (instance is IEnumerable<KeyValuePair<string, object?>> kvpEnum)
        {
            foreach (var kvp in kvpEnum)
            {
                result.Add(kvp.Key, kvp.Value);
            }
        }
        else
        {
            var properties = TypeDescriptor.GetProperties(instance).Cast<PropertyDescriptor>();
            foreach (var prop in properties.OrderBy(p => p.Name))
            {
                result.Add(prop.Name, prop.GetValue(instance));
            }
        }

        return result;
    }
}
