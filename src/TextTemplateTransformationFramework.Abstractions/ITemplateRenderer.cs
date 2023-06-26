namespace TextTemplateTransformationFramework.Abstractions;

public interface ITemplateRenderer
{
    void Render(object template,
                IIndentedStringBuilder generationEnvironment,
                string defaultFileName = "",
                object? model = null,
                object? additionalParameters = null);

    void Render(object template,
                IMultipleContentBuilder generationEnvironment,
                string defaultFileName = "",
                object? model = null,
                object? additionalParameters = null);
}
