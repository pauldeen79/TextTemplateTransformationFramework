namespace TextTemplateTransformationFramework.Abstractions;

public interface ITemplate
{
    void Render(IIndentedStringBuilder builder);
}
