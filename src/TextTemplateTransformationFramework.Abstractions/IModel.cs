namespace TextTemplateTransformationFramework.Abstractions;

public interface IModel
{
    object? Get();
    void Set(object? value);
    void Initialize();
}

public interface IModel<T> : IModel
{
    T Value { get; set; }
}
