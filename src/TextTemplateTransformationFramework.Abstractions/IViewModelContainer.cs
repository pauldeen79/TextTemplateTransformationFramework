namespace TextTemplateTransformationFramework.Abstractions;

public interface IViewModelContainer<T>
{
    T? ViewModel { get; set; }
}
