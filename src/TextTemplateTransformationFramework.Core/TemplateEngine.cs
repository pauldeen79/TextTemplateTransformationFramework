namespace TextTemplateTransformationFramework.Core;

public class TemplateEngine : ITemplateEngine
{
    private readonly ITemplateInitializer _templateInitializer;
    private readonly ITemplateRenderer[] _templateRenderers;

    public TemplateEngine()
    {
        _templateInitializer = new DefaultTemplateInitializer();
        _templateRenderers = new ITemplateRenderer[] { new SingleContentTemplateRenderer(), new MultipleContentTemplateRenderer() };
    }

    internal TemplateEngine(ITemplateInitializer templateInitializer, ITemplateRenderer[] templateRenderers)
    {
        _templateInitializer = templateInitializer;
        _templateRenderers = templateRenderers;
    }

    public void Render(object template,
                       StringBuilder generationEnvironment,
                       string defaultFilename,
                       object? additionalParameters,
                       ITemplateContext? context)
        => Render(template, generationEnvironment, defaultFilename, default(object?), additionalParameters, context);

    public void Render(object template,
                       IMultipleContentBuilder generationEnvironment,
                       string defaultFilename,
                       object? additionalParameters,
                       ITemplateContext? context)
        => Render(template, generationEnvironment, defaultFilename, default(object?), additionalParameters, context);

    public void Render(object template,
                       IMultipleContentBuilderContainer generationEnvironment,
                       string defaultFilename,
                       object? additionalParameters,
                       ITemplateContext? context)
        => Render(template, generationEnvironment, defaultFilename, default(object?), additionalParameters, context);

    protected void Render<T>(object template,
                             object generationEnvironment,
                             string defaultFilename,
                             T? model,
                             object? additionalParameters,
                             ITemplateContext? context)
    {
        Guard.IsNotNull(template);
        Guard.IsNotNull(generationEnvironment);

        _templateInitializer.Initialize(template, defaultFilename, model, additionalParameters, context);

        var renderer = Array.Find(_templateRenderers, x => x.Supports(generationEnvironment));
        if (renderer is null)
        {
            throw new ArgumentOutOfRangeException(nameof(generationEnvironment), "Type of GenerationEnvironment is not supported");
        }

        renderer.Render(template, generationEnvironment, defaultFilename);
    }
}

public class TemplateEngine<T> : TemplateEngine, ITemplateEngine<T>
{
    public TemplateEngine()
    {
    }

    internal TemplateEngine(ITemplateInitializer templateInitializer, ITemplateRenderer[] templateRenderers) : base(templateInitializer, templateRenderers)
    {
    }

    public void Render(object template,
                       StringBuilder generationEnvironment,
                       T model,
                       string defaultFilename,
                       object? additionalParameters,
                       ITemplateContext? context)
        => Render(template, generationEnvironment, defaultFilename, model, additionalParameters, context);

    public void Render(object template,
                       IMultipleContentBuilder generationEnvironment,
                       T model,
                       string defaultFilename,
                       object? additionalParameters,
                       ITemplateContext? context)
        => Render(template, generationEnvironment, defaultFilename, model, additionalParameters, context);

    public void Render(object template,
                       IMultipleContentBuilderContainer generationEnvironment,
                       T model,
                       string defaultFilename,
                       object? additionalParameters,
                       ITemplateContext? context)
        => Render(template, generationEnvironment, defaultFilename, model, additionalParameters, context);
}
