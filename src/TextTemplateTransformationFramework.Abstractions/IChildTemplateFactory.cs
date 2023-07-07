namespace TextTemplateTransformationFramework.Abstractions;

public interface IChildTemplateFactory
{
    object CreateByModel(object? model);
    object CreateByName(string name);
}
