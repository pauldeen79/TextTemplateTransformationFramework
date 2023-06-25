namespace TextTemplateTransformationFramework.Core;

public class TemplateRenderer : ITemplateRenderer
{
    public void Render(object template,
                       StringBuilder generationEnvironment,
                       string defaultFileName = "",
                       object? model = null,
                       object? additionalParameters = null)
        => Render(template, (object)generationEnvironment, defaultFileName, model, additionalParameters);

    public void Render(object template,
                       IMultipleContentBuilder generationEnvironment,
                       string defaultFileName = "",
                       object? model = null,
                       object? additionalParameters = null)
        => Render(template, (object)generationEnvironment, defaultFileName, model, additionalParameters);

    public void Render(object template,
                       IMultipleContentBuilderContainer generationEnvironment,
                       string defaultFileName = "",
                       object? model = null,
                       object? additionalParameters = null)
        => Render(template, (object)generationEnvironment, defaultFileName, model, additionalParameters);


    private void Render(object template,
                        object generationEnvironment,
                        string defaultFileName = "",
                        object? model = null,
                        object? additionalParameters = null)
    {
        Guard.IsNotNull(template);
        Guard.IsNotNull(generationEnvironment);

        TrySetAdditionalParametersOnTemplate(template, model, additionalParameters);
        TrySetViewModelOnTemplate(template, CreateSession(model), additionalParameters);

        if (generationEnvironment is StringBuilder stringBuilder)
        {
            RenderTemplate(template, stringBuilder);
        }
        else
        {
            RenderIncludedTemplate(template, generationEnvironment, string.Empty, defaultFileName);
        }
    }

    private static void RenderTemplate(object template, StringBuilder builder)
    {
        if (template is ITemplate typedTemplate)
        {
            typedTemplate.Render(builder);
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
                                               string fileNamePrefix = "",
                                               string defaultFileName = "",
                                               bool defaultSkipWhenFileExists = false)
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

        var builder = (IMultipleContentBuilder)multipleContentBuilder;

        if (builderResult.Contains(@"<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TextTemplateTransformationFramework"">"))
        {
            var multipleContents = MultipleContentBuilder.FromString(builderResult);
            foreach (var c in multipleContents.Contents)
            {
                builder.AddContent(fileNamePrefix + c.FileName, c.SkipWhenFileExists, c.Builder);
            }
        }
        else
        {
            builder.AddContent(defaultFileName, defaultSkipWhenFileExists, new StringBuilder(builderResult));
        }
    }

    private static void TrySetAdditionalParametersOnTemplate(object template,
                                                             object? model,
                                                             object? additionalParameters)
    {
        var props = template.GetType().GetProperties(Constants.BindingFlags);

        var modelProperty = model is null
            ? null
            : Array.Find(props, p => p.Name == Constants.ModelKey);

        modelProperty?.SetValue(template, model);

        foreach (var item in additionalParameters.ToKeyValuePairs())
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

    private static void TrySetViewModelOnTemplate(object template,
                                                  IEnumerable<KeyValuePair<string, object?>> session,
                                                  object? additionalParameters)
    {
        if (template is IViewModelContainer viewModelContainer)
        {
            viewModelContainer.ViewModel.Initialize();
            var viewModelValue = viewModelContainer.ViewModel.Get();

            CopySessionVariablesToViewModel(viewModelValue, CombineSession(session, additionalParameters.ToKeyValuePairs()));
            CopyTemplateContextToViewModel(viewModelValue, template);
        }
    }

    private static void CopySessionVariablesToViewModel(object? viewModelValue,
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

        if (viewModelValue is IModelContainer modelContainer
            && modelContainer.Model.Get() is null
            && session.Any(kvp => kvp.Key == Constants.ModelKey))
        {
            modelContainer.Model.Set(session.First(kvp => kvp.Key == Constants.ModelKey).Value);
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
