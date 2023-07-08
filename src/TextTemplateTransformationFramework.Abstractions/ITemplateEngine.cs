namespace TemplateFramework.Abstractions;

public interface ITemplateEngine<in T>
{
    void Render(object template,
                StringBuilder generationEnvironment,
                T model,
                string defaultFilename,
                object? additionalParameters,
                ITemplateContext? context);

    void Render(object template,
                IMultipleContentBuilder generationEnvironment,
                T model,
                string defaultFilename,
                object? additionalParameters,
                ITemplateContext? context);

    void Render(object template,
                IMultipleContentBuilderContainer generationEnvironment,
                T model,
                string defaultFilename,
                object? additionalParameters,
                ITemplateContext? context);
}

public interface ITemplateEngine
{
    void Render(object template,
                StringBuilder generationEnvironment,
                string defaultFilename,
                object? additionalParameters,
                ITemplateContext? context);

    void Render(object template,
                IMultipleContentBuilder generationEnvironment,
                string defaultFilename,
                object? additionalParameters,
                ITemplateContext? context);

    void Render(object template,
                IMultipleContentBuilderContainer generationEnvironment,
                string defaultFilename,
                object? additionalParameters,
                ITemplateContext? context);
}
