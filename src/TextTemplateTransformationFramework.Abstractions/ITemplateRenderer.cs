namespace TextTemplateTransformationFramework.Abstractions;

public interface ITemplateRenderer<in T>
{
    void Render(object template,
                StringBuilder generationEnvironment,
                string defaultFileName = "",
                T? model = default,
                object? additionalParameters = null);

    void Render(object template,
                IMultipleContentBuilder generationEnvironment,
                string defaultFileName = "",
                T? model = default,
                object? additionalParameters = null);

    void Render(object template,
                IMultipleContentBuilderContainer generationEnvironment,
                string defaultFileName = "",
                T? model = default,
                object? additionalParameters = null);
}

public interface ITemplateRenderer : ITemplateRenderer<object?>
{
}
