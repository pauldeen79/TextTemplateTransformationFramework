namespace TextTemplateTransformationFramework.Core;

public class TemplateRenderer : ITemplateRenderer
{
    public void Render(object template,
                       StringBuilder generationEnvironment,
                       string defaultFileName = "",
                       object? additionalParameters = null)
        => Render<object?>(template, generationEnvironment, defaultFileName, default, additionalParameters);

    public void Render(object template,
                       IMultipleContentBuilder generationEnvironment,
                       string defaultFileName = "",
                       object? additionalParameters = null)
        => Render<object?>(template, generationEnvironment, defaultFileName, default, additionalParameters);

    public void Render(object template,
                       IMultipleContentBuilderContainer generationEnvironment,
                       string defaultFileName = "",
                       object? additionalParameters = null)
        => Render<object?>(template, generationEnvironment, defaultFileName, default, additionalParameters);


    protected void Render<T>(object template,
                             object generationEnvironment,
                             string defaultFileName = "",
                             T? model = default,
                             object? additionalParameters = null)
    {
        Guard.IsNotNull(template);
        Guard.IsNotNull(generationEnvironment);

        TrySetAdditionalParametersOnTemplate(template, model, additionalParameters);
        TrySetViewModelOnTemplate<T>(template, CreateSession(model), additionalParameters);

        if (generationEnvironment is StringBuilder stringBuilder)
        {
            // This path is for StringBuilder
            RenderTemplate(template, stringBuilder);
        }
        else
        {
            // This path includes IMultipleContentBuilder and IMultipleContentBuilderContainer (which includes ITemplateFileManager)
            RenderIncludedTemplate(template, generationEnvironment, defaultFileName);
        }
    }

    private static void RenderTemplate(object template, StringBuilder builder)
    {
        if (template is ITemplate typedTemplate)
        {
            typedTemplate.Render(builder);
        }
        else if (template is ITextTransformTemplate textTransformTemplate)
        {
            var output = textTransformTemplate.TransformText();
            if (!string.IsNullOrEmpty(output))
            {
                builder.Append(output);
            }
        }
        else
        {
            var output = template.ToString();
            if (!string.IsNullOrEmpty(output))
            {
                builder.Append(output);
            }
        }
    }

    private static void RenderIncludedTemplate(object template,
                                               object multipleContentBuilder,
                                               string defaultFileName = "")
    {
        var stringBuilder = new StringBuilder();
        RenderTemplate(template, stringBuilder);
        var builderResult = stringBuilder.ToString();

        if (multipleContentBuilder is IMultipleContentBuilderContainer container)
        {
            // Use TemplateFileManager
            multipleContentBuilder = container.MultipleContentBuilder
                ?? throw new InvalidOperationException("MultipleContentBuilder property is null");
        }

        // Note that we can safely cast here, as the public method only accepts IMultipleContentBuilder and IMultipleContentBuilderContainer
        var builder = (IMultipleContentBuilder)multipleContentBuilder;

        if (builderResult.Contains(@"<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TextTemplateTransformationFramework"">"))
        {
            var multipleContents = MultipleContentBuilder.FromString(builderResult);
            foreach (var c in multipleContents.Contents.Select(x => x.Build()))
            {
                builder.AddContent(c.FileName, c.SkipWhenFileExists, c.Builder);
            }
        }
        else
        {
            builder.AddContent(defaultFileName, false, new StringBuilder(builderResult));
        }
    }

    private static void TrySetAdditionalParametersOnTemplate<T>(object template,
                                                                T? model,
                                                                object? additionalParameters)
    {
        if (template is IModelContainer<T> modelContainer)
        {
            modelContainer.Model = model;
        }

        var props = template.GetType().GetProperties(Constants.BindingFlags);

        foreach (var item in additionalParameters.ToKeyValuePairs().Where(x => x.Key != Constants.ModelKey))
        {
            var additionalProperty = Array.Find(props, p => p.Name == item.Key);
            additionalProperty?.SetValue(template, item.Value);
        }
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

    private static void TrySetViewModelOnTemplate<T>(object template,
                                                     IEnumerable<KeyValuePair<string, object?>> session,
                                                     object? additionalParameters)
    {
        var templateType = template.GetType();
        if (templateType.IsGenericTypeDefinition && templateType.GetGenericTypeDefinition() == typeof(IViewModelContainer<>))
        {
            var viewModelValue = templateType.GetProperty(nameof(IViewModelContainer<object>.ViewModel))!.GetValue(template);
            if (viewModelValue is null)
            {
                var viewModelType = templateType.GetGenericArguments().FirstOrDefault();
                if (viewModelType is not null && viewModelType.GetConstructors().Any(x => x.GetParameters().Length == 0))
                {
                    viewModelValue = Activator.CreateInstance(viewModelType);
                    templateType.GetProperty(nameof(IViewModelContainer<object>.ViewModel))!.SetValue(template, viewModelValue);
                }
            }

            CopySessionVariablesToViewModel<T>(viewModelValue, CombineSession(session, additionalParameters.ToKeyValuePairs()));
            CopyTemplateContextToViewModel(viewModelValue, template);
        }
    }

    private static void CopySessionVariablesToViewModel<T>(object? viewModelValue,
                                                           IEnumerable<KeyValuePair<string, object?>> session)
    {
        if (viewModelValue is null)
        {
            return;
        }

        var viewModelValueType = viewModelValue.GetType();
        foreach (var kvp in session.Where(kvp => kvp.Key != Constants.ModelKey))
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

    private static IEnumerable<KeyValuePair<string, object?>> CombineSession(IEnumerable<KeyValuePair<string, object?>> session,
                                                                             IEnumerable<KeyValuePair<string, object?>> additionalParameters)
        => session
            .Where(x => !additionalParameters.Any(y => y.Key == x.Key))
            .Concat(additionalParameters)
            .ToDictionary(x => x.Key, x => x.Value);
}

public class TemplateRenderer<T> : TemplateRenderer, ITemplateRenderer<T>
{
    public void Render(object template,
                       StringBuilder generationEnvironment,
                       T model,
                       string defaultFileName = "",
                       object? additionalParameters = null)
        => Render(template, generationEnvironment, defaultFileName, model, additionalParameters);

    public void Render(object template,
                       IMultipleContentBuilder generationEnvironment,
                       T model,
                       string defaultFileName = "",
                       object? additionalParameters = null)
        => Render(template, generationEnvironment, defaultFileName, model, additionalParameters);

    public void Render(object template,
                       IMultipleContentBuilderContainer generationEnvironment,
                       T model,
                       string defaultFileName = "",
                       object? additionalParameters = null)
        => Render(template, generationEnvironment, defaultFileName, model, additionalParameters);
}
