namespace TextTemplateTransformationFramework.Core.Contracts;

public interface ITemplateInitializer
{
    void Initialize<T>(object template,
                       string defaultFilename,
                       ITemplateEngine engine,
                       T? model,
                       object? additionalParameters,
                       ITemplateContext? context);
}
