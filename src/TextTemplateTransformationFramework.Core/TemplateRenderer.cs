using System.Text;
using TextTemplateTransformationFramework.Abstractions;
using TextTemplateTransformationFramework.Core.Extensions;

namespace TextTemplateTransformationFramework.Core
{
    public class TemplateRenderer : ITemplateRenderer
    {
        public void Render(object template,
                           object generationEnvironment,
                           string defaultFileName = "",
                           object? model = null,
                           object? additionalParameters = null)
        {
            if (template is null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            if (generationEnvironment is null)
            {
                throw new ArgumentNullException(nameof(generationEnvironment));
            }

            SetAdditionalParametersOnTemplate(template, model, additionalParameters);

            //TODO: Review if we want to support ViewModel on templates
            //var templateType = template.GetType();
            //var session = CreateSession(model);
            //SetViewModelOnTemplate(templateType, template, session, additionalParameters);

            if (generationEnvironment is StringBuilder stringBuilder)
            {
                RenderTemplate(template, stringBuilder, template.GetType());
            }
            else
            {
                RenderIncludedTemplate(template, generationEnvironment, string.Empty, defaultFileName);
            }
        }

        private static void RenderTemplate(object template, StringBuilder builder, Type templateType)
        {
            var renderMethod = templateType.GetMethod("Render");
            var transformTextMethod = templateType.GetMethod("TransformText");

            if (renderMethod is not null)
            {
                renderMethod.Invoke(template, new object[] { builder });
            }
            else if (transformTextMethod is not null)
            {
                var output = transformTextMethod.Invoke(template, Array.Empty<object>()) as string;
                
                if (!string.IsNullOrEmpty(output))
                {
                    builder.Append(output);
                }
            }
            else
            {
                var toStringMethod = templateType.GetMethod(nameof(ToString))!;
                var output = toStringMethod.Invoke(template, Array.Empty<object>()) as string;
                
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
            RenderTemplate(template, stringBuilder, template.GetType());
            var builderResult = stringBuilder.ToString();
            var multipleContentBuilderType = multipleContentBuilder.GetType();
            var multipleContentBuilderProperty = multipleContentBuilderType.GetProperty(nameof(MultipleContentBuilder), Constants.BindingFlags);
            
            if (multipleContentBuilderProperty is not null)
            {
                // Use TemplateFileManager
                multipleContentBuilder = multipleContentBuilderProperty.GetValue(multipleContentBuilder)
                    ?? throw new InvalidOperationException("MultipleContentBuilder property is null");
                multipleContentBuilderType = multipleContentBuilder.GetType();
            }

            var addContentMethod = multipleContentBuilderType.GetMethod(nameof(IMultipleContentBuilder.AddContent));
            if (addContentMethod is null)
            {
                throw new ArgumentException("Parameter does not have an AddContent method, and cannot be used as multiple content builder", nameof(multipleContentBuilder));
            }

            if (builderResult.Contains(@"<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TextTemplateTransformationFramework"">"))
            {
                var multipleContents = MultipleContentBuilder.FromString(builderResult);
                foreach (var c in multipleContents.Contents)
                {
                    addContentMethod.Invoke(multipleContentBuilder, new object[] { fileNamePrefix + c.FileName, c.SkipWhenFileExists, c.Builder });
                }
            }
            else
            {
                addContentMethod.Invoke(multipleContentBuilder, new object[] { defaultFileName, defaultSkipWhenFileExists, new StringBuilder(builderResult) });
            }
        }

        private static void SetAdditionalParametersOnTemplate(object template,
                                                              object? model,
                                                              object? additionalParameters)
        {
            var templateType = template.GetType();
            var sessionProperty = templateType.GetProperty("Session", Constants.BindingFlags);
            
            if (sessionProperty is not null)
            {
                var session = CreateSession(model);

                foreach (var item in additionalParameters.ToKeyValuePairs())
                {
                    session.Add(item.Key, item.Value);
                }

                sessionProperty.SetValue(template, session, null);
            }
            else
            {
                var props = template.GetType().GetProperties(Constants.BindingFlags);

                var modelProperty = model is null
                    ? null
                    : Array.Find(props, p => p.Name == "Model");

                modelProperty?.SetValue(template, model);

                foreach (var item in additionalParameters.ToKeyValuePairs())
                {
                    var additionalProperty = Array.Find(props, p => p.Name == item.Key);
                    additionalProperty?.SetValue(template, item.Value);
                }
            }
        }

        private static Dictionary<string, object?> CreateSession(object? model)
        {
            var session = new Dictionary<string, object?>();

            if (model is not null)
            {
                session.Add("Model", model);
            }

            return session;
        }

        //TODO: Review if we want to support ViewModel on templates
        //private static void SetViewModelOnTemplate(Type templateType,
        //                                           object template,
        //                                           IEnumerable<KeyValuePair<string, object?>> session,
        //                                           object? additionalParameters)
        //{
        //    var viewModelProperty = templateType.GetProperty("ViewModel", Constants.BindingFlags);
        //    if (viewModelProperty is not null && viewModelProperty.PropertyType != typeof(object))
        //    {
        //        var viewModelValue = viewModelProperty.GetValue(template);
        //        if (viewModelValue is null)
        //        {
        //            viewModelValue = Activator.CreateInstance(viewModelProperty.PropertyType);
        //            viewModelProperty.SetValue(template, viewModelValue);
        //        }

        //        CopySessionVariablesToViewModel(viewModelValue!, CombineSession(session, additionalParameters.ToKeyValuePairs()));
        //        CopyTemplateContextToViewModel(viewModelValue!, template);
        //    }
        //}

        //private static void CopySessionVariablesToViewModel(object viewModelValue,
        //                                                    IEnumerable<KeyValuePair<string, object?>> session)
        //{
        //    var viewModelValueType = viewModelValue.GetType();
        //    foreach (var kvp in session.Where(kvp => kvp.Key != "Model"))
        //    {
        //        var prop = viewModelValueType.GetProperty(kvp.Key, Constants.BindingFlags);
        //        if (prop is not null && prop.GetSetMethod() is null) { continue; }
        //        prop?.SetValue(viewModelValue, ConvertType(kvp, viewModelValueType));
        //    }

        //    var modelProperty = viewModelValueType.GetProperty("Model", Constants.BindingFlags);
        //    if (modelProperty is not null
        //        && modelProperty.GetValue(viewModelValue) is null
        //        && session.Any(kvp => kvp.Key == "Model"))
        //    {
        //        modelProperty.SetValue(viewModelValue, session.First(kvp => kvp.Key == "Model").Value);
        //    }
        //}

        //private static void CopyTemplateContextToViewModel(object viewModelValue, object template)
        //{
        //    var templateContextProperty = template.GetType().GetProperty("TemplateContext", Constants.BindingFlags);
        //    var templateContextValue = templateContextProperty?.GetValue(template);
        //    if (templateContextValue is null)
        //    {
        //        return;
        //    }

        //    var viewModelTemplateContextProperty = viewModelValue.GetType().GetProperty("TemplateContext", Constants.BindingFlags);
        //    viewModelTemplateContextProperty?.SetValue(viewModelValue, templateContextValue);
        //}

        //private static object? ConvertType(KeyValuePair<string, object?> parameter, Type parentContainerType)
        //{
        //    var property = parentContainerType.GetProperty(parameter.Key, Constants.BindingFlags);
        //    if (property is null)
        //    {
        //        return parameter.Value;
        //    }

        //    if (parameter.Value is int && property.PropertyType.IsEnum)
        //    {
        //        return Enum.ToObject(property.PropertyType, parameter.Value);
        //    }

        //    return Convert.ChangeType(parameter.Value, property.PropertyType);
        //}

        //private static IEnumerable<KeyValuePair<string, object?>> CombineSession(IEnumerable<KeyValuePair<string, object?>> session,
        //                                                                         IEnumerable<KeyValuePair<string, object?>> additionalParameters)
        //    => session
        //        .Where(kvp => !additionalParameters.Any(kvp2 => kvp2.Key == kvp.Key))
        //        .Concat(additionalParameters)
        //        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
}
