namespace TemplateFramework.Abstractions;

public interface IParameterizedTemplate
{
    void SetParameter(string name, object? value);
}
