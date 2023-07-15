namespace TemplateFramework.Abstractions;

public interface ITemplateEngine
{
    void Render<T>(object template,
                   StringBuilder generationEnvironment,
                   T? model,
                   string defaultFilename,
                   object? additionalParameters,
                   ITemplateContext? context);

    void Render<T>(object template,
                   IMultipleContentBuilder generationEnvironment,
                   T? model,
                   string defaultFilename,
                   object? additionalParameters,
                   ITemplateContext? context);

    void Render<T>(object template,
                   IMultipleContentBuilderContainer generationEnvironment,
                   T? model,
                   string defaultFilename,
                   object? additionalParameters,
                   ITemplateContext? context);
}
