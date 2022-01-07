using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace TextTemplateTransformationFramework.Runtime
{
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
                if (childTemplate != null)
                {
                    val.Add(childTemplate);
                }
            });
        }

        public void ProcessPlaceholder(string placeholderName, object model = null)
        {
            if (PlaceholderChildrenDictionary.ContainsKey(placeholderName))
            {
                foreach (var template in PlaceholderChildrenDictionary[placeholderName])
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
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            var templateType = template.GetType();

            var childRootTemplateProperty = templateType.GetProperty("RootTemplate");
            if (childRootTemplateProperty != null && childRootTemplateProperty.GetSetMethod() != null)
            {
                var currentTemplateRootTemplate = GetType().GetProperty("RootTemplate");
                var rootTemplate = currentTemplateRootTemplate == null
                    ? this
                    : currentTemplateRootTemplate.GetValue(this, null);
                childRootTemplateProperty.SetValue(template, rootTemplate, null);
            }

            var modelProperty = model == null
                ? null
                : templateType.GetProperty("Model", Constants.BindingFlags);
            modelProperty?.SetValue(template, model, null);

            var initializeMethod = templateType.GetMethod("Initialize");
            initializeMethod?.Invoke(template, initializeMethod.GetParameters().Length == 0 ? Array.Empty<object>() : new object[] { null });

            var viewModelProperty = templateType.GetProperty("ViewModel", Constants.BindingFlags);
            if (viewModelProperty != null && viewModelProperty.PropertyType != typeof(object))
            {
                InitializeViewModel(template, model, iterationNumber, iterationCount, resolveTemplateName, templateType, viewModelProperty);
            }
            else
            {
                var templateContextProperty = templateType.GetProperty("TemplateContext", Constants.BindingFlags);
                templateContextProperty?.SetValue(template, TemplateContext.CreateChildContext(template, model, null, iterationNumber, iterationCount, resolveTemplateName), Constants.BindingFlags, null, null, CultureInfo.CurrentCulture);
            }

            var toStringHelperProperty = templateType.GetProperty("ToStringHelper");
            if (toStringHelperProperty?.GetSetMethod() != null)
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
            if (viewModelValue == null)
            {
                viewModelValue = Activator.CreateInstance(viewModelProperty.PropertyType);
                viewModelProperty.SetValue(template, viewModelValue);
            }

            var viewModelValueType = viewModelValue.GetType();
            foreach (var kvp in Session.Where(kvp => kvp.Key != "Model"))
            {
                var prop = viewModelValueType.GetProperty(kvp.Key);
                if (prop != null && prop.GetSetMethod() == null) { continue; }
                prop?.SetValue(viewModelValue, ConvertType(kvp, viewModelValueType));
            }

            var viewModelModelProperty = viewModelValueType.GetProperty("Model", Constants.BindingFlags);
            viewModelModelProperty?.SetValue(viewModelValue, model);

            var viewModelRootTemplateProp = viewModelValueType.GetProperty("RootTemplate", BindingFlags.Public | BindingFlags.Instance);
            var templateRootTemplateProp = templateType.GetProperty("RootTemplate", Constants.BindingFlags);
            if (viewModelRootTemplateProp != null && templateRootTemplateProp != null && viewModelRootTemplateProp.GetSetMethod() != null)
            {
                viewModelRootTemplateProp.SetValue(viewModelValue, templateRootTemplateProp.GetValue(template));
            }

            var viewModelTemplateContextProperty = viewModelValueType.GetProperty("TemplateContext", Constants.BindingFlags);
            if (viewModelTemplateContextProperty != null)
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
                                        string headerTemplateName = null,
                                        bool headerCondition = true,
                                        string footerTemplateName = null,
                                        bool footerCondition = true,
                                        Func<string, string, Type, object, bool> customResolverDelegate = null,
                                        object resolverDelegateModel = null,
                                        Action<string, object, object, bool, bool, int?, int?> customRenderChildTemplateDelegate = null,
                                        Func<object, string> customTemplateNameDelegate = null)
        {
            if (renderAsEnumerable == null)
            {
                renderAsEnumerable = model is IEnumerable && !(model is string);
            }
            if (renderAsEnumerable == true && model != null && model is IEnumerable)
            {
                var items = ((IEnumerable)model).OfType<object>().ToArray();
                var iterationCount = items.Length;
                var iterationNumber = 0;
                var originalTemplateName = templateName;
                foreach (var item in items)
                {
                    templateName = GetTemplateName(templateName, customTemplateNameDelegate, originalTemplateName, item);
                    RenderHeader(templateName, model, renderAsEnumerable, silentlyContinueOnError, headerTemplateName, headerCondition, customResolverDelegate, resolverDelegateModel, customRenderChildTemplateDelegate, iterationCount, iterationNumber);
                    var template = GetChildTemplate(templateName, resolverDelegateModel ?? item, silentlyContinueOnError, customResolverDelegate);
                    RenderTemplateFromEnumerable(templateName, renderAsEnumerable, silentlyContinueOnError, customRenderChildTemplateDelegate, iterationCount, iterationNumber, item, template);
                    RenderSeparator(templateName, model, renderAsEnumerable, silentlyContinueOnError, separatorTemplateName, customResolverDelegate, resolverDelegateModel, customRenderChildTemplateDelegate, iterationCount, iterationNumber, item);
                    iterationNumber++;
                }
                RenderFooter(templateName, model, renderAsEnumerable, silentlyContinueOnError, footerTemplateName, footerCondition, customResolverDelegate, resolverDelegateModel, customRenderChildTemplateDelegate, iterationCount, iterationNumber);
            }
            else
            {
                templateName = GetTemplateName(templateName, customTemplateNameDelegate, templateName, model);
                var template = GetChildTemplate(templateName, resolverDelegateModel ?? model, silentlyContinueOnError, customResolverDelegate);
                RenderSingleTemplate(templateName, model, renderAsEnumerable, silentlyContinueOnError, customRenderChildTemplateDelegate, template);
            }
        }

        private void RenderTemplate(object template, Type templateType)
        {
            var renderMethod = templateType.GetMethod("Render");
            var transformTextMethod = templateType.GetMethod("TransformText");
            if (renderMethod != null)
            {
                renderMethod.Invoke(template, new object[] { GenerationEnvironment });
            }
            else if (transformTextMethod != null)
            {
                GenerationEnvironment.Append((string)transformTextMethod.Invoke(template, Array.Empty<object>()));
            }
            else
            {
                var toStringMethod = templateType.GetMethod("ToString");
                GenerationEnvironment.Append((string)toStringMethod.Invoke(template, Array.Empty<object>()));
            }
        }

        private void RenderSingleTemplate(string templateName, object model, bool? renderAsEnumerable, bool silentlyContinueOnError, Action<string, object, object, bool, bool, int?, int?> customRenderChildTemplateDelegate, object template)
        {
            if (template == null)
            {
                return;
            }

            if (customRenderChildTemplateDelegate != null)
            {
                customRenderChildTemplateDelegate.Invoke(templateName, template, model, renderAsEnumerable.Value, silentlyContinueOnError, null, null);
            }
            else
            {
                RenderTemplate(template, model, null, null, templateName);
            }
        }

        private void RenderTemplateFromEnumerable(string templateName, bool? renderAsEnumerable, bool silentlyContinueOnError, Action<string, object, object, bool, bool, int?, int?> customRenderChildTemplateDelegate, int iterationCount, int iterationNumber, object item, object template)
        {
            if (template == null)
            {
                return;
            }

            if (customRenderChildTemplateDelegate != null)
            {
                customRenderChildTemplateDelegate.Invoke(templateName, template, item, renderAsEnumerable.Value, silentlyContinueOnError, iterationNumber, iterationCount);
            }
            else
            {
                RenderTemplate(template, item, iterationNumber, iterationCount, templateName);
            }
        }

        private void RenderHeader(string templateName, object model, bool? renderAsEnumerable, bool silentlyContinueOnError, string headerTemplateName, bool headerCondition, Func<string, string, Type, object, bool> customResolverDelegate, object resolverDelegateModel, Action<string, object, object, bool, bool, int?, int?> customRenderChildTemplateDelegate, int iterationCount, int iterationNumber)
        {
            if (iterationNumber != 0 || string.IsNullOrEmpty(headerTemplateName) || !headerCondition)
            {
                return;
            }

            var headerTemplate = GetChildTemplate(headerTemplateName, resolverDelegateModel ?? model, silentlyContinueOnError, customResolverDelegate);
            if (headerTemplate == null)
            {
                return;
            }

            if (customRenderChildTemplateDelegate != null)
            {
                customRenderChildTemplateDelegate.Invoke(templateName, headerTemplate, null, renderAsEnumerable.Value, silentlyContinueOnError, iterationNumber + 1, iterationCount);
            }
            else
            {
                RenderTemplate(headerTemplate, null, iterationNumber - 1, iterationCount, templateName);
            }
        }

        private void RenderSeparator(string templateName, object model, bool? renderAsEnumerable, bool silentlyContinueOnError, string separatorTemplateName, Func<string, string, Type, object, bool> customResolverDelegate, object resolverDelegateModel, Action<string, object, object, bool, bool, int?, int?> customRenderChildTemplateDelegate, int iterationCount, int iterationNumber, object item)
        {
            if (iterationNumber + 1 >= iterationCount || string.IsNullOrEmpty(separatorTemplateName))
            {
                return;
            }

            var separatorTemplate = GetChildTemplate(separatorTemplateName, resolverDelegateModel ?? model, silentlyContinueOnError, customResolverDelegate);
            if (separatorTemplate == null)
            {
                return;
            }

            if (customRenderChildTemplateDelegate != null)
            {
                customRenderChildTemplateDelegate.Invoke(templateName, separatorTemplate, item, renderAsEnumerable.Value, silentlyContinueOnError, iterationNumber, iterationCount);
            }
            else
            {
                RenderTemplate(separatorTemplate, item, iterationNumber, iterationCount, templateName);
            }
        }

        private void RenderFooter(string templateName, object model, bool? renderAsEnumerable, bool silentlyContinueOnError, string footerTemplateName, bool footerCondition, Func<string, string, Type, object, bool> customResolverDelegate, object resolverDelegateModel, Action<string, object, object, bool, bool, int?, int?> customRenderChildTemplateDelegate, int iterationCount, int iterationNumber)
        {
            if (iterationNumber == 0 || string.IsNullOrEmpty(footerTemplateName) || !footerCondition)
            {
                return;
            }

            var footerTemplate = GetChildTemplate(footerTemplateName, resolverDelegateModel ?? model, silentlyContinueOnError, customResolverDelegate);
            if (footerTemplate == null)
            {
                return;
            }

            if (customRenderChildTemplateDelegate != null)
            {
                customRenderChildTemplateDelegate.Invoke(templateName, footerTemplate, null, renderAsEnumerable.Value, silentlyContinueOnError, iterationNumber + 1, iterationCount);
            }
            else
            {
                RenderTemplate(footerTemplate, null, iterationNumber + 1, iterationCount, templateName);
            }
        }

        private static string GetTemplateName(string templateName, Func<object, string> customTemplateNameDelegate, string originalTemplateName, object item)
        {
            if (customTemplateNameDelegate == null)
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
            if (returnValue != null && model != null)
            {
                var modelProperty = returnValue.GetType().GetProperty("Model", Constants.BindingFlags);
                modelProperty?.SetValue(returnValue, model);
            }
            return returnValue;
        }

        protected object GetRegisteredObject(List<Tuple<string, Func<object>, Type>> registrations, string objectName, string name, object model = null, bool silentlyContinueOnError = false, Func<string, string, Type, object, bool> customResolverDelegate = null)
        {
            if (objectName == null)
            {
                throw new ArgumentNullException(nameof(objectName));
            }

            var registrationTuples = customResolverDelegate != null
                ? registrations.Where(t => customResolverDelegate(name, t.Item1, t.Item3, model))
                : Resolve(registrations, name, model);

            if (!registrationTuples.Any() && customResolverDelegate == null && string.IsNullOrEmpty(name) && model != null)
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
                if (registrationTuple == null)
                {
                    ReportErrorOnMissingRegistration(objectName, name, model, silentlyContinueOnError);
                    return null;
                }

                var registeredInstance = registrationTuple.Item2();
                if (registeredInstance == null)
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

            Error("Multiple " + objectName.ToLower(CultureInfo.InvariantCulture) + "s found with model type " + (model == null ? "{null}" : model.GetType().FullName) + ": " + string.Join(", ", templateTuplesArray.Select(t => t.Item1)));
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

            var errorMessage = string.IsNullOrEmpty(name) && model != null
                ? "Could not resolve " + objectName.ToLower(CultureInfo.InvariantCulture) + " with model type " + model.GetType().FullName
                : "Could not resolve " + objectName.ToLower(CultureInfo.InvariantCulture) + " with name " + name;

            Error(errorMessage);
        }

        protected void PerformActionOnPlaceholder(string placeholderName, Action<IList<object>> placeholderAction)
        {
            if (placeholderAction == null)
            {
                throw new ArgumentNullException(nameof(placeholderAction));
            }

            if (!PlaceholderChildrenDictionary.ContainsKey(placeholderName))
            {
                PlaceholderChildrenDictionary.Add(placeholderName, new List<object>());
            }

            var childrenList = PlaceholderChildrenDictionary[placeholderName];
            placeholderAction(childrenList);
        }

        protected static object ConvertType(KeyValuePair<string, object> parameter, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var property = type.GetProperty(parameter.Key);

            return property == null
                ? parameter.Value
                : ConvertParameter(parameter, property);
        }

        protected static object ConvertParameter(KeyValuePair<string, object> parameter, PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            return parameter.Value is int && property.PropertyType.IsEnum
                ? Enum.ToObject(property.PropertyType, parameter.Value)
                : Convert.ChangeType(parameter.Value, property.PropertyType);
        }

        private static IEnumerable<Tuple<string, Func<object>, Type>> Resolve(List<Tuple<string, Func<object>, Type>> registrations, string name, object model)
            => string.IsNullOrEmpty(name) && model != null
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
