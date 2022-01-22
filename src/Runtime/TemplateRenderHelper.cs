using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using TextTemplateTransformationFramework.Runtime.Extensions;

namespace TextTemplateTransformationFramework.Runtime
{
    public static class TemplateRenderHelper
    {
        public static string GetTemplateOutput(object template,
                                               object model = null,
                                               string modelPropertyName = "Model",
                                               Action preRenderDelegate = null,
                                               Action additionalActionDelegate = null,
                                               StringBuilder builder = null,
                                               object additionalParameters = null)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            if (builder == null)
            {
                var templateType = template.GetType();
                var generationEnvironmentProperty = templateType.GetProperty("GenerationEnvironment", Constants.BindingFlags);
                if (generationEnvironmentProperty != null)
                {
                    builder = generationEnvironmentProperty.GetValue(template) as StringBuilder;
                }

                if (builder == null)
                {
                    builder = new StringBuilder();
                }
            }

            var errors = GetCompilerErrors(template, builder, model, modelPropertyName, preRenderDelegate, additionalActionDelegate, additionalParameters);
            return errors.Any(e => !e.IsWarning)
                ? string.Join(Environment.NewLine, errors.Select(e => string.Format("{0}: {1}", e.GetErrorType(), e.ErrorText)))
                : builder.ToString();
        }

        public static IEnumerable<CompilerError> GetCompilerErrors(object template,
                                                                   StringBuilder builder,
                                                                   object model = null,
                                                                   string modelPropertyName = "Model",
                                                                   Action preRenderDelegate = null,
                                                                   Action additionalActionDelegate = null,
                                                                   object additionalParameters = null)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var templateType = template.GetType();

            SetAdditionalParametersOnTemplate(template, model, additionalParameters, modelPropertyName);
            InitializeTemplate(template, additionalActionDelegate);
            SetViewModelOnTemplate(templateType, template, Enumerable.Empty<KeyValuePair<string, object>>(), additionalParameters); 

            var errorsProperty = templateType.GetProperty("Errors");
            var errors = GetErrors(errorsProperty, template, out bool hasErrors);
            if (hasErrors)
            {
                return errors;
            }

            preRenderDelegate?.Invoke();

            DoRender(template, builder, templateType);

            errors = GetErrors(errorsProperty, template, out hasErrors);
            if (hasErrors)
            {
                return errors;
            }

            return Enumerable.Empty<CompilerError>();
        }

        public static void RenderTemplate(object template,
                                          object generationEnvironment,
                                          IEnumerable<KeyValuePair<string, object>> session = null,
                                          string fileNamePrefix = "",
                                          string defaultFileName = null,
                                          Action additionalActionDelegate = null,
                                          object additionalParameters = null)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            if (generationEnvironment == null)
            {
                throw new ArgumentNullException(nameof(generationEnvironment));
            }

            SetSessionOnIncludedTemplate(template, session, additionalParameters);
            InitializeTemplate(template, additionalActionDelegate);
            SetViewModelOnTemplate(template.GetType(), template, session, additionalParameters);
            var errorsProperty = template.GetType().GetProperty("Errors");
            var errors = GetErrors(errorsProperty, template, out bool hasErrors);
            if (hasErrors)
            {
                throw new AggregateException(string.Join(Environment.NewLine, errors.Select(e => string.Format("{0}: {1}", e.GetErrorType(), e.ErrorText))));
            }

            if (generationEnvironment is StringBuilder sb)
            {
                DoRender(template, sb, template.GetType());
            }
            else
            {
                RenderIncludedTemplate(template, generationEnvironment, fileNamePrefix, defaultFileName);
            }
        }

        public static void RenderTemplateWithModel(object template,
                                                   object generationEnvironment,
                                                   object model,
                                                   string modelPropertyName = "Model",
                                                   string defaultFileName = null,
                                                   Action additionalActionDelegate = null,
                                                   object additionalParameters = null)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            if (generationEnvironment == null)
            {
                throw new ArgumentNullException(nameof(generationEnvironment));
            }

            SetAdditionalParametersOnTemplate(template, model, additionalParameters, modelPropertyName);
            InitializeTemplate(template, additionalActionDelegate);

            var templateType = template.GetType();
            SetViewModelOnTemplate(templateType, template, Enumerable.Empty<KeyValuePair<string, object>>(), additionalParameters);
            var errorsProperty = templateType.GetProperty("Errors");
            var errors = GetErrors(errorsProperty, template, out bool hasErrors);
            if (hasErrors)
            {
                throw new AggregateException(string.Join(Environment.NewLine, errors.Select(e => string.Format("{0}: {1}", e.GetErrorType(), e.ErrorText))));
            }

            if (generationEnvironment is StringBuilder sb)
            {
                DoRender(template, sb, template.GetType());
            }
            else
            {
                RenderIncludedTemplate(template, generationEnvironment, string.Empty, defaultFileName);
            }
        }

        public static TChild CreateNestedTemplate<TParent, TChild>(object model = null,
                                                                   object rootModel = null,
                                                                   Action additionalActionDelegate = null,
                                                                   object rootAdditionalParameters = null,
                                                                   object iterationContextModel = null,
                                                                   string modelPropertyName = "Model")
            where TParent : new()
            where TChild : new()
        {
            var context = CreateNestedTemplateContext<TParent, TChild>(model, rootModel, additionalActionDelegate, rootAdditionalParameters, iterationContextModel, modelPropertyName);

            return (TChild)context.GetType().GetProperty("Template").GetValue(context);
        }

        public static TContext CreateNestedTemplateContext<TParent, TChild, TContext>(object model = null,
                                                                                      object rootModel = null,
                                                                                      Action additionalActionDelegate = null,
                                                                                      object rootAdditionalParameters = null,
                                                                                      object iterationContextModel = null,
                                                                                      string modelPropertyName = "Model")
            where TParent : new()
            where TChild : new()
            => (TContext)CreateNestedTemplateContext<TParent, TChild>(model, rootModel, additionalActionDelegate, rootAdditionalParameters, iterationContextModel, modelPropertyName);

        public static object CreateNestedTemplateContext<TParent, TChild>(object model = null,
                                                                          object rootModel = null,
                                                                          Action additionalActionDelegate = null,
                                                                          object rootAdditionalParameters = null,
                                                                          object iterationContextModel = null,
                                                                          string modelPropertyName = "Model")
            where TParent : new()
            where TChild : new()
        {
            var rootTemplate = new TParent();
            SetAdditionalParametersOnTemplate(rootTemplate, rootModel, rootAdditionalParameters, modelPropertyName);
            InitializeTemplate(rootTemplate, additionalActionDelegate);

            var childTemplate = new TChild();
            if (model != null)
            {
                SetModelOnTemplate(childTemplate, model);
            }
            SetRootTemplateOnChildTemplate(rootTemplate, childTemplate, true);

            var viewModel = childTemplate.GetType().GetProperty("ViewModelProperty")?.GetValue(childTemplate);
            var context = SetTemplateContextOnChildTemplate(rootTemplate, childTemplate, model, viewModel, rootModel, null, iterationContextModel);

            var builder = new StringBuilder();

            SetGenerationEnvironmentOnTemplate(builder, rootTemplate);
            SetGenerationEnvironmentOnTemplate(builder, childTemplate);

            return context;
        }

        public static void SetModelOnTemplate(object template, object model, bool continueOnError = false)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            var modelProperty = template.GetType().GetProperty("Model");
            if (modelProperty == null || modelProperty.GetSetMethod() == null)
            {
                var modelField = template.GetType().GetField("_modelField", Constants.BindingFlags);
                if (modelField == null)
                {
                    if (continueOnError)
                    {
                        return;
                    }
                    throw new InvalidOperationException($"Template of type [{template.GetType().FullName}] doesn't have a Model property or _modelField field");
                }
                modelField.SetValue(template, model);
            }
            else
            {
                modelProperty.SetValue(template, model);
            }
        }

        public static object SetTemplateContextOnChildTemplate(object rootTemplate,
                                                               object childTemplate,
                                                               object model,
                                                               object viewModel = null,
                                                               object rootModel = null,
                                                               object childTemplateContext = null,
                                                               object iterationContextModel = null)
        {
            if (rootTemplate == null)
            {
                throw new ArgumentNullException(nameof(rootTemplate));
            }

            if (childTemplate == null)
            {
                throw new ArgumentNullException(nameof(childTemplate));
            }

            var rootTemplateContextProperty = rootTemplate.GetType().GetProperty("TemplateContext");
            var childTemplateContextProperty = childTemplate.GetType().GetProperty("TemplateContext");

            object childContext = childTemplateContext;
            if (rootTemplateContextProperty != null && childContext == null)
            {
                var rootTemplateContext = rootTemplateContextProperty.GetValue(rootTemplate);
                var rootModelProperty = rootTemplateContext.GetType().GetProperty("Model");
                if (rootModelProperty != null && rootModelProperty.GetValue(rootTemplateContext) == null)
                {
                    rootModelProperty.SetValue(rootTemplateContext, rootModel);
                }
                if (iterationContextModel != null)
                {
                    childContext = rootTemplateContext.GetType().GetMethod("CreateChildContext").Invoke(rootTemplateContext, new[] { rootTemplate, iterationContextModel, null, null, null, null });
                    childContext = childContext.GetType().GetMethod("CreateChildContext").Invoke(childContext, new[] { childTemplate, model, viewModel, null, null, null });
                }
                else
                {
                    childContext = rootTemplateContext.GetType().GetMethod("CreateChildContext").Invoke(rootTemplateContext, new[] { childTemplate, model, viewModel, null, null, null });
                }
            }

            if (childContext == null)
            {
                //no child context on root template, and no custom child context via argument
                if (iterationContextModel != null)
                {
                    childContext = TemplateInstanceContext.CreateRootContext(rootTemplate)
                        .CreateChildContext(rootTemplate, iterationContextModel)
                        .CreateChildContext(childTemplate, model, viewModel);
                }
                else
                {
                    childContext = TemplateInstanceContext.CreateRootContext(rootTemplate).CreateChildContext(childTemplate, model, viewModel);
                }
            }

            childTemplateContextProperty?.SetValue(childTemplate, childContext);

            return childContext;
        }

        public static void SetRootTemplateOnChildTemplate(object rootTemplate, object childTemplate, bool continueOnError = false)
        {
            if (childTemplate == null)
            {
                throw new ArgumentNullException(nameof(childTemplate));
            }

            var rootTemplateProperty = childTemplate.GetType().GetProperty("RootTemplate");
            if (rootTemplateProperty == null || rootTemplateProperty.GetSetMethod() == null)
            {
                if (continueOnError)
                {
                    return;
                }
                throw new InvalidOperationException($"Child type [{childTemplate.GetType().FullName}] doesn't have a RootTemplate property");
            }
            rootTemplateProperty.SetValue(childTemplate, rootTemplate);
        }

        private static IEnumerable<CompilerError> GetErrors(PropertyInfo errorsProperty, object template, out bool hasErrors)
        {
            if (errorsProperty == null)
            {
                hasErrors = false;
                return Enumerable.Empty<CompilerError>();
            }

            var errorsValue = errorsProperty.GetValue(template, null);
            if (!(errorsValue is IEnumerable enumerableErrors))
            {
                hasErrors = false;
                return Enumerable.Empty<CompilerError>();
            }

            var errors = enumerableErrors.OfType<object>().Select(CreateCompilerError).ToArray();
            if (errors.All(e => e.IsWarning))
            {
                hasErrors = false;
                return Enumerable.Empty<CompilerError>();
            }

            hasErrors = true;
            return errors;
        }

        private static CompilerError CreateCompilerError(object error)
        {
            var errorType = error.GetType();
            var column = Convert.ToInt32(errorType.GetProperty("Column").GetValue(error));
            var errorNumber = Convert.ToString(errorType.GetProperty("ErrorNumber").GetValue(error));
            var errorText = Convert.ToString(errorType.GetProperty("ErrorText").GetValue(error));
            var fileName = Convert.ToString(errorType.GetProperty("FileName").GetValue(error));
            var isWarning = Convert.ToBoolean(errorType.GetProperty("IsWarning").GetValue(error));
            var line = Convert.ToInt32(errorType.GetProperty("Line").GetValue(error));
            return new CompilerError(column, errorNumber, errorText, fileName, isWarning, line);
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
                builder.Append((string)transformTextMethod.Invoke(template, Array.Empty<object>()));
            }
            else
            {
                var toStringMethod = templateType.GetMethod("ToString");
                builder.Append((string)toStringMethod.Invoke(template, Array.Empty<object>()));
            }
        }

        private static void SetSessionOnIncludedTemplate(object template,
                                                         IEnumerable<KeyValuePair<string, object>> session,
                                                         object additionalParameters)
            => template.GetType().GetProperty("Session")?.SetValue(template, CombineSession(ConvertSession(session, template), additionalParameters.ToKeyValuePairs()), null);

        private static IEnumerable<KeyValuePair<string, object>> ConvertSession(IEnumerable<KeyValuePair<string, object>> session,
                                                                                object template)
        {
            if (session != null)
            {
                var templateType = template.GetType();
                foreach (var kvp in session)
                {
                    var prop = templateType.GetProperty(kvp.Key);
                    if (prop == null)
                    {
                        yield return kvp;
                    }
                    else
                    {
                        yield return new KeyValuePair<string, object>(kvp.Key, ConvertType(kvp, templateType));
                    }
                }
            }
        }

        private static void SetViewModelOnTemplate(Type templateType,
                                                   object template,
                                                   IEnumerable<KeyValuePair<string, object>> session,
                                                   object additionalParameters)
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
                CopySessionVariablesToViewModel(viewModelValue, session);
                CopySessionVariablesToViewModel(viewModelValue, additionalParameters.ToKeyValuePairs());
                CopyTemplateContextToViewModel(viewModelValue, template);
            }
        }

        private static void CopySessionVariablesToViewModel(object viewModelValue, IEnumerable<KeyValuePair<string, object>> session)
        {
            if (session == null)
            {
                return;
            }

            var viewModelValueType = viewModelValue.GetType();
            foreach (var kvp in session.Where(kvp => kvp.Key != "Model"))
            {
                var prop = viewModelValueType.GetProperty(kvp.Key);
                if (prop != null && prop.GetSetMethod() == null) { continue; }
                prop?.SetValue(viewModelValue, ConvertType(kvp, viewModelValueType));
            }
        }

        private static void CopyTemplateContextToViewModel(object viewModelValue, object template)
        {
            var templateContextProperty = template.GetType().GetProperty("TemplateContext");
            var templateContextValue = templateContextProperty?.GetValue(template);
            if (templateContextValue == null)
            {
                return;
            }
            var viewModelTemplateContextProperty = viewModelValue.GetType().GetProperty("TemplateContext");
            viewModelTemplateContextProperty?.SetValue(viewModelValue, templateContextValue);
        }

        private static object ConvertType(KeyValuePair<string, object> parameter, Type parentContainerType)
        {
            var property = parentContainerType.GetProperty(parameter.Key);

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

        private static IEnumerable<KeyValuePair<string, object>> CombineSession(IEnumerable<KeyValuePair<string, object>> session,
                                                                                IEnumerable<KeyValuePair<string, object>> additionalParameters)
            => (session ?? new Dictionary<string, object>())
                .Where(kvp => additionalParameters?.Any(kvp2 => kvp2.Key == kvp.Key) != true)
                .Concat(additionalParameters ?? Enumerable.Empty<KeyValuePair<string, object>>())
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        private static void RenderIncludedTemplate(object template,
                                                   object multipleContentBuilder,
                                                   string fileNamePrefix = "",
                                                   string defaultFileName = null,
                                                   bool defaultSkipWhenFileExists = false)
        {
            var builder = new StringBuilder();
            DoRender(template, builder, template.GetType());
            var s = builder.ToString();
            var multipleContentBuilderType = multipleContentBuilder.GetType();
            var multipleContentBuilderProperty = multipleContentBuilderType.GetProperty("MultipleContentBuilder");
            if (multipleContentBuilderProperty != null)
            {
                // TemplateFileManager
                multipleContentBuilder = multipleContentBuilderProperty.GetValue(multipleContentBuilder);
                multipleContentBuilderType = multipleContentBuilder.GetType();
            }
            var addContentMethod = multipleContentBuilderType.GetMethod("AddContent");
            if (addContentMethod == null)
            {
                throw new ArgumentException("Parameter does not have an AddContent method, and cannot be used as multiple content builder", nameof(multipleContentBuilder));
            }
            if (s.StartsWith(@"<?xml version=""1.0"" encoding=""utf-16""?>
<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TextTemplateTransformationFramework"">"))
            {
                var multipleContents = MultipleContentBuilder.FromString(s);
                foreach (var c in multipleContents.Contents)
                {
                    addContentMethod.Invoke(multipleContentBuilder, new object[] { fileNamePrefix + c.FileName, c.SkipWhenFileExists, c.Builder });
                }
            }
            else
            {
                addContentMethod.Invoke(multipleContentBuilder, new object[] { defaultFileName, defaultSkipWhenFileExists, new StringBuilder(s) });
            }
        }

        private static void SetAdditionalParametersOnTemplate(object template,
                                                              object model,
                                                              object additionalParameters,
                                                              string modelPropertyName)
        {
            var templateType = template.GetType();
            var sessionProperty = templateType.GetProperty("Session");
            if (sessionProperty != null)
            {
                var parameters = model != null && !string.IsNullOrEmpty(modelPropertyName)
                    ? new Dictionary<string, object> { { modelPropertyName, model } }
                    : new Dictionary<string, object>();

                foreach (var item in additionalParameters.ToKeyValuePairs())
                {
                    parameters.Add(item.Key, item.Value);
                }

                sessionProperty.SetValue(template, parameters, null);
            }
            else
            {
                var props = TypeDescriptor
                    .GetProperties(template)
                    .OfType<PropertyDescriptor>()
                    .ToArray();

                var modelProperty = model == null || string.IsNullOrEmpty(modelPropertyName)
                    ? null
                    : Array.Find(props, p => p.Name == modelPropertyName);

                modelProperty?.SetValue(template, model);

                foreach (var item in additionalParameters.ToKeyValuePairs())
                {
                    var additionalProperty = Array.Find(props, p => p.Name == item.Key);
                    additionalProperty?.SetValue(template, item.Value);
                }
            }
        }

        private static void SetGenerationEnvironmentOnTemplate(StringBuilder builder, object rootTemplate)
        {
            var generationEnvironmentProperty = rootTemplate.GetType().GetProperty("GenerationEnvironment");
            if (generationEnvironmentProperty == null)
            {
                throw new InvalidOperationException($"Parent type [{rootTemplate.GetType().FullName}] doesn't have a GenerationEnvironment property");
            }

            generationEnvironmentProperty.SetValue(rootTemplate, builder);
        }

        private static void InitializeTemplate(object template, Action additionalActionDelegate = null)
        {
            var initializeMethod = template.GetType().GetMethod("Initialize");
            if (initializeMethod != null)
            {
                switch (initializeMethod.GetParameters().Length)
                {
                    case 0:
                        initializeMethod.Invoke(template, null);
                        break;
                    case 1:
                        initializeMethod.Invoke(template, new object[] { additionalActionDelegate });
                        break;
                    default:
                        throw new InvalidOperationException($"Template of type [{template.GetType().FullName}] has Initialize method with more than 1 argument. This is not supported.");
                }
            }
        }
    }
}
