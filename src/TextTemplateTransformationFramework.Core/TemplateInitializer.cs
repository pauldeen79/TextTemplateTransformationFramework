namespace TextTemplateTransformationFramework.Core;

internal class TemplateInitializer : ITemplateInitializer
{
    public void Initialize<T>(object template, string defaultFilename, T? model, object? additionalParameters, ITemplateContext? context)
    {
        TrySetAdditionalParametersOnTemplate(template, model, additionalParameters);
        TrySetTemplateContextOnTemplate(template, model, context);
        TrySetViewModelOnTemplate<T>(template, CreateSession(model), additionalParameters);
    }

    private static void TrySetAdditionalParametersOnTemplate<T>(object template, T? model, object? additionalParameters)
    {
        if (template is IModelContainer<T> modelContainer)
        {
            modelContainer.Model = model;
        }

        var properties = template.GetType().GetProperties(Constants.BindingFlags);

        foreach (var item in additionalParameters.ToKeyValuePairs().Where(x => x.Key != Constants.ModelKey)) //note: also set ViewModel property here
        {
            var additionalProperty = Array.Find(properties, p => p.Name == item.Key);
            additionalProperty?.SetValue(template, item.Value);
        }
    }

    private static void TrySetTemplateContextOnTemplate(object template, object? model, ITemplateContext? context)
    {
        if (template is not ITemplateContextContainer templateContextContainer)
        {
            return;
        }

        if (context == null)
        {
            context = new TemplateContext(template, model);
        }

        templateContextContainer.Context = context;
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

    private static void TrySetViewModelOnTemplate<T>(object template, IEnumerable<KeyValuePair<string, object?>> session, object? additionalParameters)
    {
        var templateType = template.GetType();
        if (templateType.IsGenericTypeDefinition && templateType.GetGenericTypeDefinition() == typeof(IViewModelContainer<>))
        {
            var viewModelValue = templateType.GetProperty(Constants.ViewModelKey)!.GetValue(template);
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

            CopySessionVariablesToViewModel<T>(viewModelValue, CombineSession(session, additionalParameters.ToKeyValuePairs()));
            CopyTemplateContextToViewModel(viewModelValue, template);
        }
    }

    private static void CopySessionVariablesToViewModel<T>(object? viewModelValue, IEnumerable<KeyValuePair<string, object?>> session)
    {
        if (viewModelValue is null)
        {
            return;
        }

        var viewModelValueType = viewModelValue.GetType();
        foreach (var kvp in session.Where(kvp => kvp.Key != Constants.ModelKey && kvp.Key != Constants.ViewModelKey)) //note: do not copy ViewModel property to ViewModel instance... this would be unlogical
        {
            var prop = viewModelValueType.GetProperty(kvp.Key, Constants.BindingFlags);
            if (prop is not null && prop.GetSetMethod() is null)
            {
                continue;
            }

            prop?.SetValue(viewModelValue, ConvertType(kvp, viewModelValueType));
        }

        if (viewModelValue is IModelContainer<T> modelContainer && session.Any(kvp => kvp.Key == Constants.ModelKey))
        {
            modelContainer.Model = (T?)session.First(kvp => kvp.Key == Constants.ModelKey).Value;
        }
    }

    private static void CopyTemplateContextToViewModel(object? viewModelValue, object template)
    {
        if (viewModelValue is null)
        {
            return;
        }

        if (template is not ITemplateContextContainer templateContextContainer)
        {
            return;
        }


        if (viewModelValue is not ITemplateContextContainer viewModelTemplateContextContainer)
        {
            return;
        }

        viewModelTemplateContextContainer.Context = templateContextContainer.Context;
    }

    private static object? ConvertType(KeyValuePair<string, object?> parameter, Type parentContainerType)
    {
        var property = parentContainerType.GetProperty(parameter.Key, Constants.BindingFlags);
        if (property is null)
        {
            return parameter.Value;
        }

        if (parameter.Value is int && property.PropertyType.IsEnum)
        {
            return Enum.ToObject(property.PropertyType, parameter.Value);
        }

        return Convert.ChangeType(parameter.Value, property.PropertyType);
    }

    private static IEnumerable<KeyValuePair<string, object?>> CombineSession(IEnumerable<KeyValuePair<string, object?>> session, IEnumerable<KeyValuePair<string, object?>> additionalParameters)
        => session
            .Where(x => !additionalParameters.Any(y => y.Key == x.Key))
            .Concat(additionalParameters)
            .ToDictionary(x => x.Key, x => x.Value);
}
