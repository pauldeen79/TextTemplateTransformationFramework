using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace TextTemplateTransformationFramework.Runtime
{
    public class CustomDelegates
    {
        public Func<string, string, Type, object, bool> ResolverDelegate { get; set; }
        public Action<string, object, object, bool, bool, int?, int?> RenderChildTemplateDelegate { get; set; }
        public Func<object, string> TemplateNameDelegate { get; set; }
    }

    public class EnumerableItem
    {
        public int IterationCount { get; set; }
        public int IterationNumber { get; set; }
        public object Model { get; set; }
    }
    public class T4PlusGeneratedTemplateBase : T4GeneratedTemplateBase
    {
        #region Child templates
        private List<Tuple<string, Func<object>, Type>> _childTemplatesField = new List<Tuple<string, Func<object>, Type>>();
        public List<Tuple<string, Func<object>, Type>> ChildTemplates { get { return _childTemplatesField; } protected set { _childTemplatesField = value; } }
        private List<Tuple<string, Func<object>, Type>> _viewModelsField = new List<Tuple<string, Func<object>, Type>>();
        public List<Tuple<string, Func<object>, Type>> ViewModels { get { return _viewModelsField; } protected set { _viewModelsField = value; } }
        private Dictionary<string, IList<object>> _placeholderChildrenDictionaryField = new Dictionary<string, IList<object>>();
        public Dictionary<string, IList<object>> PlaceholderChildrenDictionary { get { return _placeholderChildrenDictionaryField; } protected set { _placeholderChildrenDictionaryField = value; } }

        public void AddTemplateToPlaceholder(string placeholderName, string templateName, object model = null, bool silentlyContinueOnError = false, Func<string, string, Type, object, bool> customResolverDelegate = null)
        {
            PerformActionOnPlaceholder(placeholderName, val =>
            {
                var childTemplate = GetChildTemplate(templateName, model, silentlyContinueOnError, customResolverDelegate);
                if (childTemplate is not null)
                {
                    val.Add(childTemplate);
                }
            });
        }

        public void ProcessPlaceholder(string placeholderName, object model = null)
        {
            if (PlaceholderChildrenDictionary.TryGetValue(placeholderName, out var result))
            {
                foreach (var template in result)
                {
                    RenderTemplate(template, model);
                }
            }
        }

        public void RegisterChildTemplate(string templateName, Func<object> templateDelegate, Type modelType = null)
        {
            _childTemplatesField.Add(new Tuple<string, Func<object>, Type>(templateName, templateDelegate, modelType));
        }

        public void RegisterViewModel(string viewModelName, Func<object> viewModelDelegate, Type modelType = null)
        {
            _viewModelsField.Add(new Tuple<string, Func<object>, Type>(viewModelName, viewModelDelegate, modelType));
        }

        public virtual void RenderTemplate(object template, object model, int? iterationNumber = null, int? iterationCount = null, string resolveTemplateName = null)
        {
            if (template is null)
            {
                return;
            }

            var templateType = template.GetType();

            var childRootTemplateProperty = templateType.GetProperty("RootTemplate");
            if (childRootTemplateProperty is not null && childRootTemplateProperty.GetSetMethod() is not null)
            {
                var currentTemplateRootTemplate = GetType().GetProperty("RootTemplate");
                var rootTemplate = currentTemplateRootTemplate is null
                    ? this
                    : currentTemplateRootTemplate.GetValue(this, null);
                childRootTemplateProperty.SetValue(template, rootTemplate, null);
            }

            var modelProperty = model is null
                ? null
                : templateType.GetProperty("Model", Constants.BindingFlags);
            modelProperty?.SetValue(template, model, null);

            var initializeMethod = templateType.GetMethod("Initialize");
            initializeMethod?.Invoke(template, initializeMethod.GetParameters().Length == 0 ? Array.Empty<object>() : new object[] { null });

            var viewModelProperty = templateType.GetProperty("ViewModel", Constants.BindingFlags);
            if (viewModelProperty is not null && viewModelProperty.PropertyType != typeof(object))
            {
                InitializeViewModel(template, model, iterationNumber, iterationCount, resolveTemplateName, templateType, viewModelProperty);
            }
            else
            {
                var templateContextProperty = templateType.GetProperty("TemplateContext", Constants.BindingFlags);
                templateContextProperty?.SetValue(template, TemplateContext.CreateChildContext(template, model, null, iterationNumber, iterationCount, resolveTemplateName), Constants.BindingFlags, null, null, CultureInfo.CurrentCulture);
            }

            var toStringHelperProperty = templateType.GetProperty("ToStringHelper");
            if (toStringHelperProperty?.GetSetMethod() is not null)
            {
                toStringHelperProperty.SetValue(template, ToStringHelper, null);
            }

            RenderTemplate(template, templateType);

            AddErrorsAndWarnings(template);
        }

        private void AddErrorsAndWarnings(object template)
        {
            var errorsProperty = template.GetType().GetProperty("Errors");
            if (errorsProperty?.GetValue(template, null) is List<CompilerError> errors)
            {
                foreach (var error in errors)
                {
                    if (error.IsWarning)
                    {
                        Warning(error.ErrorText);
                    }
                    else
                    {
                        Error(error.ErrorText);
                    }
                }
            }
        }

        private void InitializeViewModel(object template, object model, int? iterationNumber, int? iterationCount, string resolveTemplateName, Type templateType, PropertyInfo viewModelProperty)
        {
            var viewModelValue = viewModelProperty.GetValue(template);
            if (viewModelValue is null)
            {
                viewModelValue = Activator.CreateInstance(viewModelProperty.PropertyType);
                viewModelProperty.SetValue(template, viewModelValue);
            }

            var viewModelValueType = viewModelValue.GetType();
            foreach (var kvp in Session.Where(kvp => kvp.Key != "Model"))
            {
                var prop = viewModelValueType.GetProperty(kvp.Key);
                if (prop is not null && prop.GetSetMethod() is null) { continue; }
                prop?.SetValue(viewModelValue, ConvertType(kvp, viewModelValueType));
            }

            var viewModelModelProperty = viewModelValueType.GetProperty("Model", Constants.BindingFlags);
            viewModelModelProperty?.SetValue(viewModelValue, model);

            var viewModelRootTemplateProp = viewModelValueType.GetProperty("RootTemplate", BindingFlags.Public | BindingFlags.Instance);
            var templateRootTemplateProp = templateType.GetProperty("RootTemplate", Constants.BindingFlags);
            if (viewModelRootTemplateProp is not null && templateRootTemplateProp is not null && viewModelRootTemplateProp.GetSetMethod() is not null)
            {
                viewModelRootTemplateProp.SetValue(viewModelValue, templateRootTemplateProp.GetValue(template));
            }

            var viewModelTemplateContextProperty = viewModelValueType.GetProperty("TemplateContext", Constants.BindingFlags);
            if (viewModelTemplateContextProperty is not null)
            {
                var currentContext = TemplateContext;
                viewModelTemplateContextProperty.SetValue(viewModelValue, currentContext.CreateChildContext(template, model, viewModelValue, iterationNumber, iterationCount, resolveTemplateName), Constants.BindingFlags, null, null, CultureInfo.CurrentCulture);
            }

            var templateContextProperty = templateType.GetProperty("TemplateContext", Constants.BindingFlags);
            templateContextProperty?.SetValue(template, TemplateContext.CreateChildContext(template, model, viewModelValue, iterationNumber, iterationCount, resolveTemplateName), Constants.BindingFlags, null, null, CultureInfo.CurrentCulture);
        }

        public void RenderChildTemplate(string templateName,
                                        object model = null,
                                        bool? renderAsEnumerable = null,
                                        bool silentlyContinueOnError = false,
                                        string separatorTemplateName = null,
                                        object resolverDelegateModel = null,
                                        CustomDelegates customDelegates = null)
        {
            if (renderAsEnumerable is null)
            {
                renderAsEnumerable = model is IEnumerable && model is not string;
            }
            if (renderAsEnumerable == true && model is not null && model is IEnumerable)
            {
                var items = ((IEnumerable)model).OfType<object>().ToArray();
                var iterationCount = items.Length;
                var iterationNumber = 0;
                var originalTemplateName = templateName;
                foreach (var item in items)
                {
                    templateName = GetTemplateName(templateName, customDelegates?.TemplateNameDelegate, originalTemplateName, item);
                    var template = GetChildTemplate(templateName, resolverDelegateModel ?? item, silentlyContinueOnError, customDelegates?.ResolverDelegate);
                    if (customDelegates?.RenderChildTemplateDelegate is not null)
                    {
                        customDelegates?.RenderChildTemplateDelegate.Invoke(templateName, template, item, renderAsEnumerable.Value, silentlyContinueOnError, iterationNumber, iterationCount);
                    }
                    else
                    {
                        RenderTemplate(template, item, iterationNumber, iterationCount, templateName);
                    }
                    RenderSeparator(model, renderAsEnumerable, silentlyContinueOnError, separatorTemplateName, resolverDelegateModel, new EnumerableItem { IterationCount = iterationCount, IterationNumber = iterationNumber, Model = item }, customDelegates);
                    iterationNumber++;
                }
            }
            else
            {
                templateName = GetTemplateName(templateName, customDelegates?.TemplateNameDelegate, templateName, model);
                var template = GetChildTemplate(templateName, resolverDelegateModel ?? model, silentlyContinueOnError, customDelegates?.ResolverDelegate);
                if (customDelegates?.RenderChildTemplateDelegate is not null)
                {
                    customDelegates?.RenderChildTemplateDelegate.Invoke(templateName, template, model, renderAsEnumerable.Value, silentlyContinueOnError, null, null);
                }
                else
                {
                    RenderTemplate(template, model, null, null, templateName);
                }
            }
        }

        private void RenderTemplate(object template, Type templateType)
        {
            var renderMethod = templateType.GetMethod("Render");
            var transformTextMethod = templateType.GetMethod("TransformText");
            if (renderMethod is not null)
            {
                renderMethod.Invoke(template, new object[] { GenerationEnvironment });
            }
            else if (transformTextMethod is not null)
            {
                GenerationEnvironment.Append((string)transformTextMethod.Invoke(template, Array.Empty<object>()));
            }
            else
            {
                var toStringMethod = templateType.GetMethod("ToString");
                GenerationEnvironment.Append((string)toStringMethod.Invoke(template, Array.Empty<object>()));
            }
        }

        private void RenderSeparator(object model,
                                     bool? renderAsEnumerable,
                                     bool silentlyContinueOnError,
                                     string separatorTemplateName,
                                     object resolverDelegateModel,
                                     EnumerableItem item,
                                     CustomDelegates customDelegates)
        {
            if (item.IterationNumber + 1 >= item.IterationCount || string.IsNullOrEmpty(separatorTemplateName))
            {
                return;
            }

            var separatorTemplate = GetChildTemplate(separatorTemplateName, resolverDelegateModel ?? model, silentlyContinueOnError, customDelegates?.ResolverDelegate);
            if (separatorTemplate is null)
            {
                return;
            }

            if (customDelegates?.RenderChildTemplateDelegate is not null)
            {
                customDelegates.RenderChildTemplateDelegate.Invoke(separatorTemplateName, separatorTemplate, item.Model, renderAsEnumerable.Value, silentlyContinueOnError, item.IterationNumber, item.IterationCount);
            }
            else
            {
                RenderTemplate(separatorTemplate,item.Model, item.IterationNumber, item.IterationCount, separatorTemplateName);
            }
        }

        private static string GetTemplateName(string templateName, Func<object, string> customTemplateNameDelegate, string originalTemplateName, object item)
        {
            if (customTemplateNameDelegate is null)
            {
                return templateName;
            }

            var customTemplateName = customTemplateNameDelegate(item);
            if (!string.IsNullOrEmpty(customTemplateName))
            {
                templateName = customTemplateName;
            }
            else
            {
                templateName = originalTemplateName;
            }

            return templateName;
        }

        protected virtual object GetChildTemplate(string templateName, object model = null, bool silentlyContinueOnError = false, Func<string, string, Type, object, bool> customResolverDelegate = null)
        {
            return GetRegisteredObject(_childTemplatesField, "Child template", templateName, model, silentlyContinueOnError, customResolverDelegate);
        }

        protected virtual object GetViewModel(string viewModelName, object model = null, bool silentlyContinueOnError = false, Func<string, string, Type, object, bool> customResolverDelegate = null)
        {
            var returnValue = GetRegisteredObject(_viewModelsField, "View model", viewModelName, model, silentlyContinueOnError, customResolverDelegate);
            if (returnValue is not null && model is not null)
            {
                var modelProperty = returnValue.GetType().GetProperty("Model", Constants.BindingFlags);
                modelProperty?.SetValue(returnValue, model);
            }
            return returnValue;
        }

        protected object GetRegisteredObject(List<Tuple<string, Func<object>, Type>> registrations, string objectName, string name, object model = null, bool silentlyContinueOnError = false, Func<string, string, Type, object, bool> customResolverDelegate = null)
        {
            if (objectName is null)
            {
                throw new ArgumentNullException(nameof(objectName));
            }

            var registrationTuples = customResolverDelegate is not null
                ? registrations.Where(t => customResolverDelegate(name, t.Item1, t.Item3, model))
                : Resolve(registrations, name, model);

            if (!registrationTuples.Any() && customResolverDelegate is null && string.IsNullOrEmpty(name) && model is not null)
            {
                registrationTuples = registrations.Where(t => t.Item3?.IsAssignableFrom(model.GetType()) == true);
            }

            var templateTuplesArray = registrationTuples.ToArray();
            if (templateTuplesArray.Length > 1)
            {
                ReportErrorOnMultipleRegistrations(objectName, model, silentlyContinueOnError, templateTuplesArray);
                return null;
            }
            else
            {
                var registrationTuple = templateTuplesArray.Length == 1 ? templateTuplesArray[0] : null;
                if (registrationTuple is null)
                {
                    ReportErrorOnMissingRegistration(objectName, name, model, silentlyContinueOnError);
                    return null;
                }

                var registeredInstance = registrationTuple.Item2();
                if (registeredInstance is null)
                {
                    ReportErrorOnRegistrationInstanciation(objectName, silentlyContinueOnError, registrationTuple);
                    return null;
                }

                return registeredInstance;
            }
        }

        private void ReportErrorOnMultipleRegistrations(string objectName, object model, bool silentlyContinueOnError, Tuple<string, Func<object>, Type>[] templateTuplesArray)
        {
            if (silentlyContinueOnError)
            {
                return;
            }

            Error("Multiple " + objectName.ToLower(CultureInfo.InvariantCulture) + "s found with model type " + (model is null ? "{null}" : model.GetType().FullName) + ": " + string.Join(", ", templateTuplesArray.Select(t => t.Item1)));
        }

        private void ReportErrorOnRegistrationInstanciation(string objectName, bool silentlyContinueOnError, Tuple<string, Func<object>, Type> registrationTuple)
        {
            if (silentlyContinueOnError)
            {
                return;
            }

            Error(objectName + " [" + registrationTuple.Item1 + "] did not instanciate");
        }

        private void ReportErrorOnMissingRegistration(string objectName, string name, object model, bool silentlyContinueOnError)
        {
            if (silentlyContinueOnError)
            {
                return;
            }

            var errorMessage = string.IsNullOrEmpty(name) && model is not null
                ? "Could not resolve " + objectName.ToLower(CultureInfo.InvariantCulture) + " with model type " + model.GetType().FullName
                : "Could not resolve " + objectName.ToLower(CultureInfo.InvariantCulture) + " with name " + name;

            Error(errorMessage);
        }

        protected void PerformActionOnPlaceholder(string placeholderName, Action<IList<object>> placeholderAction)
        {
            if (placeholderAction is null)
            {
                throw new ArgumentNullException(nameof(placeholderAction));
            }

            if (!PlaceholderChildrenDictionary.TryGetValue(placeholderName, out _))
            {
                PlaceholderChildrenDictionary.Add(placeholderName, new List<object>());
            }

            var childrenList = PlaceholderChildrenDictionary[placeholderName];
            placeholderAction(childrenList);
        }

        protected static object ConvertType(KeyValuePair<string, object> parameter, Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var property = type.GetProperty(parameter.Key);

            return property is null
                ? parameter.Value
                : ConvertParameter(parameter, property);
        }

        protected static object ConvertParameter(KeyValuePair<string, object> parameter, PropertyInfo property)
        {
            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            return parameter.Value is int && property.PropertyType.IsEnum
                ? Enum.ToObject(property.PropertyType, parameter.Value)
                : Convert.ChangeType(parameter.Value, property.PropertyType);
        }

        private static IEnumerable<Tuple<string, Func<object>, Type>> Resolve(List<Tuple<string, Func<object>, Type>> registrations, string name, object model)
            => string.IsNullOrEmpty(name) && model is not null
                ? registrations.Where(t => t.Item3 == model.GetType())
                : registrations.Where(t => t.Item1 == name);
        #endregion

        #region Template context
        private TemplateInstanceContext _templateContextField;

        public TemplateInstanceContext TemplateContext
        {
            get
            {
                return _templateContextField ?? (_templateContextField = TemplateInstanceContext.CreateRootContext(this));
            }
            set { _templateContextField = value; }
        }
        #endregion 
    }
}
