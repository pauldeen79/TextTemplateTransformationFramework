namespace TemplateFramework.Core.TemplateInitializers;

public class DefaultTemplateInitializer : ITemplateInitializer
{
    public void Initialize<T>(object template, string defaultFilename, ITemplateEngine engine, T? model, object? additionalParameters, ITemplateContext? context)
    {
        Guard.IsNotNull(template);
        Guard.IsNotNull(defaultFilename);
        Guard.IsNotNull(engine);

        var session = additionalParameters.ToKeyValuePairs();
        TrySetAdditionalParametersOnTemplate(template, model, session);
        TrySetTemplateContextOnTemplate(template, model, context);
        TrySetTemplateEngineOnTemplate(template, engine);
    }

    private static void TrySetAdditionalParametersOnTemplate<T>(object template, T? model, IEnumerable<KeyValuePair<string, object?>> additionalParameters)
    {
        if (template is IModelContainer<T> modelContainer)
        {
            modelContainer.Model = model;
        }

        if (template is not IParameterizedTemplate parameterizedTemplate)
        {
            return;
        }

        foreach (var item in additionalParameters.ToKeyValuePairs().Where(x => x.Key != Constants.ModelKey))
        {
            parameterizedTemplate.SetParameter(item.Key, item.Value);
        }
    }

    private static void TrySetTemplateContextOnTemplate(object template, object? model, ITemplateContext? context)
    {
        if (context is null)
        {
            context = new TemplateContext(template, model);
        }

        if (template is not ITemplateContextContainer templateContextContainer)
        {
            return;
        }

        templateContextContainer.Context = context;
    }

    private static void TrySetTemplateEngineOnTemplate(object template, ITemplateEngine engine)
    {
        if (template is not ITemplateEngineContainer templateEngineContainer)
        {
            return;
        }

        templateEngineContainer.TemplateEngine = engine;
    }
}
