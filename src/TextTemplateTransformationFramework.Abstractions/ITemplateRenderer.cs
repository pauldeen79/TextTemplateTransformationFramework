namespace TextTemplateTransformationFramework.Abstractions;

public interface ITemplateRenderer<in T>
{
    void Render(object template,
                StringBuilder generationEnvironment,
                T model,
                string defaultFileName,
                object? additionalParameters);

    void Render(object template,
                IMultipleContentBuilder generationEnvironment,
                T model,
                string defaultFileName,
                object? additionalParameters);

    void Render(object template,
                IMultipleContentBuilderContainer generationEnvironment,
                T model,
                string defaultFileName,
                object? additionalParameters);
}

public interface ITemplateRenderer
{
    void Render(object template,
                StringBuilder generationEnvironment,
                string defaultFileName,
                object? additionalParameters);

    void Render(object template,
                IMultipleContentBuilder generationEnvironment,
                string defaultFileName,
                object? additionalParameters);

    void Render(object template,
                IMultipleContentBuilderContainer generationEnvironment,
                string defaultFileName,
                object? additionalParameters);
}
