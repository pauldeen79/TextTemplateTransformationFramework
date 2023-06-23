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
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            if (generationEnvironment == null)
            {
                throw new ArgumentNullException(nameof(generationEnvironment));
            }

            SetAdditionalParametersOnTemplate(template, model, additionalParameters);

            var templateType = template.GetType();
            var session = model == null
                ? Enumerable.Empty<KeyValuePair<string, object?>>()
                : new[] { new KeyValuePair<string, object?>("Model", model) };
            SetViewModelOnTemplate(templateType, template, session, additionalParameters);

            if (generationEnvironment is StringBuilder stringBuilder)
            {
                DoRender(template, stringBuilder, template.GetType());
            }
            else
            {
                RenderIncludedTemplate(template, generationEnvironment, string.Empty, defaultFileName);
            }
        }

        private static void DoRender(object template, StringBuilder builder, Type templateType)
        {
            var renderMethod = templateType.GetMethod("Render");
            var transformTextMethod = templateType.GetMethod("TransformText");
            if (renderMethod != null)
            {
                renderMethod.Invoke(template, new object[] { builder });
            }
            else if (transformTextMethod != null)
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
            DoRender(template, stringBuilder, template.GetType());
            var builderResult = stringBuilder.ToString();
            var multipleContentBuilderType = multipleContentBuilder.GetType();
            var multipleContentBuilderProperty = multipleContentBuilderType.GetProperty(nameof(MultipleContentBuilder), Constants.BindingFlags);
            if (multipleContentBuilderProperty != null)
            {
                // TemplateFileManager
                multipleContentBuilder = multipleContentBuilderProperty.GetValue(multipleContentBuilder) ?? throw new InvalidOperationException("MultipleContentBuilder property is null");
                multipleContentBuilderType = multipleContentBuilder.GetType();
            }
            var addContentMethod = multipleContentBuilderType.GetMethod(nameof(IMultipleContentBuilder.AddContent));
            if (addContentMethod == null)
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
            if (sessionProperty != null)
            {
                var parameters = model != null
                    ? new Dictionary<string, object?> { { "Model", model } }
                    : new Dictionary<string, object?>();

                foreach (var item in additionalParameters.ToKeyValuePairs())
                {
                    parameters.Add(item.Key, item.Value);
                }

                sessionProperty.SetValue(template, parameters, null);
            }
            else
            {
                var props = template.GetType().GetProperties(Constants.BindingFlags);

                var modelProperty = model == null
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

        private static void SetViewModelOnTemplate(Type templateType,
                                                   object template,
                                                   IEnumerable<KeyValuePair<string, object?>> session,
                                                   object? additionalParameters)
        {
            var viewModelProperty = templateType.GetProperty("ViewModel", Constants.BindingFlags);
            if (viewModelProperty != null && viewModelProperty.PropertyType != typeof(object))
            {
                var viewModelValue = viewModelProperty.GetValue(template);
                if (viewModelValue == null)
                {
                    viewModelValue = Activator.CreateInstance(viewModelProperty.PropertyType);
                    viewModelProperty.SetValue(template, viewModelValue);
                }
                CopySessionVariablesToViewModel(viewModelValue!, CombineSession(session, additionalParameters.ToKeyValuePairs()));
                CopyTemplateContextToViewModel(viewModelValue!, template);
            }
        }

        private static void CopySessionVariablesToViewModel(object viewModelValue,
                                                            IEnumerable<KeyValuePair<string, object?>> session)
        {
            if (session == null)
            {
                return;
            }

            var viewModelValueType = viewModelValue.GetType();
            foreach (var kvp in session.Where(kvp => kvp.Key != "Model"))
            {
                var prop = viewModelValueType.GetProperty(kvp.Key, Constants.BindingFlags);
                if (prop != null && prop.GetSetMethod() == null) { continue; }
                prop?.SetValue(viewModelValue, ConvertType(kvp, viewModelValueType));
            }

            var modelProperty = viewModelValueType.GetProperty("Model", Constants.BindingFlags);
            if (modelProperty != null && modelProperty.GetValue(viewModelValue) == null && session.Any(kvp => kvp.Key == "Model"))
            {
                modelProperty.SetValue(viewModelValue, session.First(kvp => kvp.Key == "Model").Value);
            }
        }

        private static void CopyTemplateContextToViewModel(object viewModelValue, object template)
        {
            var templateContextProperty = template.GetType().GetProperty("TemplateContext", Constants.BindingFlags);
            var templateContextValue = templateContextProperty?.GetValue(template);
            if (templateContextValue == null)
            {
                return;
            }
            var viewModelTemplateContextProperty = viewModelValue.GetType().GetProperty("TemplateContext", Constants.BindingFlags);
            viewModelTemplateContextProperty?.SetValue(viewModelValue, templateContextValue);
        }

        private static object? ConvertType(KeyValuePair<string, object?> parameter, Type parentContainerType)
        {
            var property = parentContainerType.GetProperty(parameter.Key, Constants.BindingFlags);
            if (property == null)
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
            => (session ?? new Dictionary<string, object?>())
                .Where(kvp => additionalParameters?.Any(kvp2 => kvp2.Key == kvp.Key) != true)
                .Concat(additionalParameters ?? Enumerable.Empty<KeyValuePair<string, object?>>())
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
}
