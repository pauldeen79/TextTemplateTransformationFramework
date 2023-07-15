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
        context = TrySetTemplateContextOnTemplate(template, model, context);
        TrySetViewModelOnTemplate<T>(template, CreateSession(model), session, context!);
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

        foreach (var item in additionalParameters.ToKeyValuePairs().Where(x => x.Key != Constants.ModelKey)) //note: also set ViewModel property here
        {
            parameterizedTemplate.SetParameter(item.Key, item.Value);
        }
    }

    private static ITemplateContext TrySetTemplateContextOnTemplate(object template, object? model, ITemplateContext? context)
    {
        if (context is null)
        {
            context = new TemplateContext(template, model);
        }

        if (template is not ITemplateContextContainer templateContextContainer)
        {
            return context;
        }

        templateContextContainer.Context = context;

        return context;
    }

    private static void TrySetViewModelOnTemplate<T>(object template, IEnumerable<KeyValuePair<string, object?>> session, IEnumerable<KeyValuePair<string, object?>> additionalParameters, ITemplateContext context)
    {
        var templateType = template.GetType();
        if (!Array.Exists(templateType.GetInterfaces(), x => x.FullName?.StartsWith(Constants.ViewModelTypeName) == true))
        {
            return;
        }

        var viewModelValue = templateType.GetProperty(Constants.ViewModelKey, Constants.CustomBindingFlags)!
                                         .GetValue(template, Constants.CustomBindingFlags, null, null, null);
        if (viewModelValue is null)
        {
            // Allow dynamic construction of ViewModel.
            // Note that if you need injected values, then you have to inject the ViewModel into the template engine using additionalProperties: new { ViewModel = myViewModelInstance }
            var viewModelType = templateType.GetGenericArguments().FirstOrDefault();
            if (viewModelType is not null && Array.Exists(viewModelType.GetConstructors(), x => x.GetParameters().Length == 0))
            {
                viewModelValue = Activator.CreateInstance(viewModelType);
                templateType.GetProperty(Constants.ViewModelKey)!.SetValue(template, viewModelValue);
            }
        }

        if (viewModelValue is null)
        {
            return;
        }

        CopyAdditionalParametersAndModelToViewModel<T>(viewModelValue, CombineSession(session, additionalParameters.ToKeyValuePairs()));
        CopyTemplateContextToViewModel(viewModelValue, context);
    }

    private static void TrySetTemplateEngineOnTemplate(object template, ITemplateEngine engine)
    {
        if (template is not ITemplateEngineContainer templateEngineContainer)
        {
            return;
        }

        templateEngineContainer.TemplateEngine = engine;
    }

    private static Dictionary<string, object?> CreateSession(object? model)
    {
        var session = new Dictionary<string, object?>();

        if (model is not null)
        {
            session.Add(Constants.ModelKey, model);
        }

        return session;
    }

    private static void CopyAdditionalParametersAndModelToViewModel<T>(object viewModelValue, IEnumerable<KeyValuePair<string, object?>> session)
    {
        if (viewModelValue is IParameterizedTemplate parameterizedViewModel)
        {
            foreach (var kvp in session.Where(kvp => kvp.Key != Constants.ModelKey && kvp.Key != Constants.ViewModelKey)) //note: do not copy ViewModel property to ViewModel instance... this would be unlogical
            {
                parameterizedViewModel.SetParameter(kvp.Key, kvp.Value);
            }
        }

        if (viewModelValue is IModelContainer<T> modelContainer)
        {
            var kvp = session.Select(x => new { x.Key, x.Value }).FirstOrDefault(kvp => kvp.Key == Constants.ModelKey);
            if (kvp is not null)
            {
                modelContainer.Model = (T?)ConvertType(kvp.Value, typeof(T));
            }
        }
    }

    private static void CopyTemplateContextToViewModel(object viewModelValue, ITemplateContext context)
    {
        if (viewModelValue is not ITemplateContextContainer viewModelTemplateContextContainer)
        {
            return;
        }

        viewModelTemplateContextContainer.Context = context;
    }

    private static object? ConvertType(object? value, Type type)
    {
        if (value is null)
        {
            return null;
        }

        if (value is int && type.IsEnum)
        {
            return Enum.ToObject(type, value);
        }

        if (type.IsInstanceOfType(value))
        {
            return value;
        }

        return Convert.ChangeType(value, type);
    }

    private static IEnumerable<KeyValuePair<string, object?>> CombineSession(IEnumerable<KeyValuePair<string, object?>> session, IEnumerable<KeyValuePair<string, object?>> additionalParameters)
        => session
            .Where(x => !additionalParameters.Any(y => y.Key == x.Key))
            .Concat(additionalParameters)
            .ToDictionary(x => x.Key, x => x.Value);
}
