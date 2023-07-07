namespace TextTemplateTransformationFramework.Abstractions;

public interface IChildTemplateCreator
{
    bool SupportsModel(object? model);
    bool SupportsName(string name);
    object CreateByModel(object? model);
    object CreateByName(string name);
}
