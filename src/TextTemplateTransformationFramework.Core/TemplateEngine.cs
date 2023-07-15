namespace TemplateFramework.Core;

public class TemplateEngine : ITemplateEngine
{
    private readonly ITemplateInitializer _templateInitializer;
    private readonly IEnumerable<ITemplateRenderer> _templateRenderers;

    public TemplateEngine(ITemplateInitializer templateInitializer, IEnumerable<ITemplateRenderer> templateRenderers)
    {
        Guard.IsNotNull(templateInitializer);
        Guard.IsNotNull(templateRenderers);
        _templateInitializer = templateInitializer;
        _templateRenderers = templateRenderers;
    }

    public void Render<T>(object template,
                          StringBuilder generationEnvironment,
                          T? model,
                          string defaultFilename,
                          object? additionalParameters,
                          ITemplateContext? context)
        => Render(template, generationEnvironment, defaultFilename, model, additionalParameters, context);

    public void Render<T>(object template,
                          IMultipleContentBuilder generationEnvironment,
                          T? model,
                          string defaultFilename,
                          object? additionalParameters,
                          ITemplateContext? context)
        => Render(template, generationEnvironment, defaultFilename, model, additionalParameters, context);

    public void Render<T>(object template,
                          IMultipleContentBuilderContainer generationEnvironment,
                          T? model,
                          string defaultFilename,
                          object? additionalParameters,
                          ITemplateContext? context)
        => Render(template, generationEnvironment, defaultFilename, model, additionalParameters, context);

    protected void Render<T>(object template,
                             object generationEnvironment,
                             string defaultFilename,
                             T? model,
                             object? additionalParameters,
                             ITemplateContext? context)
    {
        Guard.IsNotNull(template);
        Guard.IsNotNull(generationEnvironment);

        _templateInitializer.Initialize(template, defaultFilename, this, model, additionalParameters, context);

        var renderer = _templateRenderers.FirstOrDefault(x => x.Supports(generationEnvironment));
        if (renderer is null)
        {
            throw new ArgumentOutOfRangeException(nameof(generationEnvironment), "Type of GenerationEnvironment is not supported");
        }

        renderer.Render(template, generationEnvironment, defaultFilename);
    }
}
