﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace TextTemplateTransformationFramework.Runtime
{
    public class T4PlusComposableGeneratedTemplateBaseCompositionRoot<T> : T4PlusComposableGeneratedTemplateBaseCompositionRoot
        where T : T4PlusComposableGeneratedTemplateBase, new()
    {
        public T4PlusComposableGeneratedTemplateBaseCompositionRoot(Func<Tuple<string, Func<object>, Type>, Tuple<string, Func<object>, Type>> childTemplateModifierDelegate = null,
                                                                    Func<Tuple<string, Func<object>, Type>, Tuple<string, Func<object>, Type>> viewModelModifierDelegate = null,
                                                                    Action<string, Func<object>, Type> registerChildTemplateDelegate = null,
                                                                    Action<string, Func<object>, Type> registerViewModelDelegate = null)
            : base(childTemplateModifierDelegate, viewModelModifierDelegate, registerChildTemplateDelegate, registerViewModelDelegate)
        {
        }

        public T ResolveTemplate()
        {
            return new T
            {
                GetChildTemplateDelegate = GetChildTemplate,
                GetViewModelDelegate = GetViewModel
            };
        }
    }

    public class T4PlusComposableGeneratedTemplateBaseCompositionRoot
    {
        public T4PlusComposableGeneratedTemplateBaseCompositionRoot(Func<Tuple<string, Func<object>, Type>, Tuple<string, Func<object>, Type>> childTemplateModifierDelegate = null,
                                                                    Func<Tuple<string, Func<object>, Type>, Tuple<string, Func<object>, Type>> viewModelModifierDelegate = null,
                                                                    Action<string, Func<object>, Type> registerChildTemplateDelegate = null,
                                                                    Action<string, Func<object>, Type> registerViewModelDelegate = null)
        {
            ChildTemplates = new List<Tuple<string, Func<object>, Type>>();
            ViewModels = new List<Tuple<string, Func<object>, Type>>();
            ChildTemplateModifierDelegate = childTemplateModifierDelegate;
            ViewModelModifierDelegate = viewModelModifierDelegate;
            RegisterChildTemplateDelegate = registerChildTemplateDelegate;
            RegisterViewModelDelegate = registerViewModelDelegate;
        }

        #region Child templates
        protected List<Tuple<string, Func<object>, Type>> ChildTemplates { get; }
        protected List<Tuple<string, Func<object>, Type>> ViewModels { get; }
        protected Func<Tuple<string, Func<object>, Type>, Tuple<string, Func<object>, Type>> ChildTemplateModifierDelegate { get; }
        protected Func<Tuple<string, Func<object>, Type>, Tuple<string, Func<object>, Type>> ViewModelModifierDelegate { get; }
        protected Action<string, Func<object>, Type> RegisterChildTemplateDelegate { get; }
        protected Action<string, Func<object>, Type> RegisterViewModelDelegate { get; }

        public void RegisterChildTemplate(string templateName, Func<object> templateDelegate, Type modelType = null)
        {
            var registration = new Tuple<string, Func<object>, Type>(templateName, templateDelegate, modelType);
            if (ChildTemplateModifierDelegate != null)
            {
                var newRegistration = ChildTemplateModifierDelegate(registration);
                if (newRegistration != null)
                {
                    if (RegisterChildTemplateDelegate != null)
                    {
                        RegisterChildTemplateDelegate(templateName, newRegistration.Item2, modelType);
                    }
                    else
                    {
                        ChildTemplates.Add(new Tuple<string, Func<object>, Type>(templateName, newRegistration.Item2, modelType));
                    }
                    return;
                }
            }

            if (RegisterChildTemplateDelegate != null)
            {
                RegisterChildTemplateDelegate(registration.Item1, registration.Item2, registration.Item3);
            }
            else
            {
                ChildTemplates.Add(registration);
            }
        }

        public void RegisterViewModel(string viewModelName, Func<object> viewModelDelegate, Type modelType = null)
        {
            var registration = new Tuple<string, Func<object>, Type>(viewModelName, viewModelDelegate, modelType);
            if (ViewModelModifierDelegate != null)
            {
                var newRegistration = ViewModelModifierDelegate(registration);
                if (newRegistration != null)
                {
                    if (RegisterChildTemplateDelegate != null)
                    {
                        RegisterViewModelDelegate(viewModelName, newRegistration.Item2, modelType);
                    }
                    else
                    {
                        ViewModels.Add(new Tuple<string, Func<object>, Type>(viewModelName, newRegistration.Item2, modelType));
                    }
                    return;
                }
            }

            if (RegisterChildTemplateDelegate != null)
            {
                RegisterViewModelDelegate(registration.Item1, registration.Item2, registration.Item3);
            }
            else
            {
                ViewModels.Add(registration);
            }
        }

        protected virtual object GetChildTemplate(string templateName, object model, bool silentlyContinueOnError, Func<string, string, Type, object, bool> customResolverDelegate, Action<string> errorDelegate)
        {
            return GetRegisteredObject(ChildTemplates, "Child template", templateName, model, silentlyContinueOnError, customResolverDelegate, errorDelegate);
        }

        protected virtual object GetViewModel(string viewModelName, object model, bool silentlyContinueOnError, Func<string, string, Type, object, bool> customResolverDelegate, Action<string> errorDelegate)
        {
            var returnValue = GetRegisteredObject(ViewModels, "View model", viewModelName, model, silentlyContinueOnError, customResolverDelegate, errorDelegate);
            if (returnValue != null && model != null)
            {
                var modelProperty = returnValue.GetType().GetProperty("Model", Constants.BindingFlags);
                modelProperty?.SetValue(returnValue, model);
            }
            return returnValue;
        }

        protected virtual object GetRegisteredObject(List<Tuple<string, Func<object>, Type>> registrations, string objectName, string name, object model, bool silentlyContinueOnError, Func<string, string, Type, object, bool> customResolverDelegate, Action<string> errorDelegate)
        {
            if (errorDelegate == null)
            {
                throw new ArgumentNullException(nameof(errorDelegate));
            }
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
                if (!silentlyContinueOnError)
                {
                    errorDelegate("Multiple " + objectName.ToLower(CultureInfo.InvariantCulture) + "s found with model type " + (model == null ? "{null}" : model.GetType().FullName) + ": " + string.Join(", ", templateTuplesArray.Select(t => t.Item1)));
                }
                return null;
            }
            else
            {
                var registrationTuple = templateTuplesArray.Length == 1 ? templateTuplesArray[0] : null;
                if (registrationTuple == null)
                {
                    if (string.IsNullOrEmpty(name) && model != null)
                    {
                        if (!silentlyContinueOnError)
                        {
                            errorDelegate("Could not resolve " + objectName.ToLower(CultureInfo.InvariantCulture) + " with model type " + model.GetType().FullName);
                        }
                    }
                    else
                    {
                        if (!silentlyContinueOnError)
                        {
                            errorDelegate("Could not resolve " + objectName.ToLower(CultureInfo.InvariantCulture) + " with name " + name);
                        }
                    }

                    return null;
                }

                var registeredInstance = registrationTuple.Item2();
                if (registeredInstance == null)
                {
                    if (!silentlyContinueOnError)
                    {
                        errorDelegate(objectName + " [" + registrationTuple.Item1 + "] did not instanciate");
                    }
                    return null;
                }

                return registeredInstance;
            }
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

    public class T4PlusComposableGeneratedTemplateBase : T4GeneratedTemplateBase
    {
        #region Child templates
        public virtual void RenderTemplate(object template, object model, int? iterationNumber = null, int? iterationCount = null, string resolveTemplateName = null)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            var templateType = template.GetType();

            var childRootTemplateProperty = templateType.GetProperty("RootTemplate");
            if (childRootTemplateProperty?.GetSetMethod() != null)
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
                    if (iterationNumber == 0 && !string.IsNullOrEmpty(headerTemplateName) && headerCondition)
                    {
                        RenderHeaderTemplate(templateName, model, renderAsEnumerable, silentlyContinueOnError, headerTemplateName, customResolverDelegate, resolverDelegateModel, customRenderChildTemplateDelegate, iterationCount, iterationNumber);
                    }
                    var template = GetChildTemplate(templateName, resolverDelegateModel ?? item, silentlyContinueOnError, customResolverDelegate);
                    if (template != null)
                    {
                        if (customRenderChildTemplateDelegate != null)
                        {
                            customRenderChildTemplateDelegate.Invoke(templateName, template, item, renderAsEnumerable.Value, silentlyContinueOnError, iterationNumber, iterationCount);
                        }
                        else
                        {
                            RenderTemplate(template, item, iterationNumber, iterationCount, templateName);
                        }
                    }
                    if (iterationNumber + 1 < iterationCount && !string.IsNullOrEmpty(separatorTemplateName))
                    {
                        RenderSeparatorTemplate(templateName, model, renderAsEnumerable, silentlyContinueOnError, separatorTemplateName, customResolverDelegate, resolverDelegateModel, customRenderChildTemplateDelegate, iterationCount, iterationNumber, item);
                    }
                    iterationNumber++;
                }
                if (iterationNumber > 0 && !string.IsNullOrEmpty(footerTemplateName) && footerCondition)
                {
                    RenderFooterTemplate(templateName, model, renderAsEnumerable, silentlyContinueOnError, footerTemplateName, customResolverDelegate, resolverDelegateModel, customRenderChildTemplateDelegate, iterationCount, iterationNumber);
                }
            }
            else
            {
                templateName = GetTemplateName(templateName, customTemplateNameDelegate, templateName, model);
                var template = GetChildTemplate(templateName, resolverDelegateModel ?? model, silentlyContinueOnError, customResolverDelegate);
                if (template != null)
                {
                    if (customRenderChildTemplateDelegate != null)
                    {
                        customRenderChildTemplateDelegate.Invoke(templateName, template, model, renderAsEnumerable.Value, silentlyContinueOnError, null, null);
                    }
                    else
                    {
                        RenderTemplate(template, model, null, null, templateName);
                    }
                }
            }
        }

        private static string GetTemplateName(string templateName, Func<object, string> customTemplateNameDelegate, string originalTemplateName, object item)
        {
            if (customTemplateNameDelegate != null)
            {
                var customTemplateName = customTemplateNameDelegate(item);
                if (!string.IsNullOrEmpty(customTemplateName))
                {
                    templateName = customTemplateName;
                }
                else
                {
                    templateName = originalTemplateName;
                }
            }

            return templateName;
        }

        private void RenderHeaderTemplate(string templateName, object model, bool? renderAsEnumerable, bool silentlyContinueOnError, string headerTemplateName, Func<string, string, Type, object, bool> customResolverDelegate, object resolverDelegateModel, Action<string, object, object, bool, bool, int?, int?> customRenderChildTemplateDelegate, int iterationCount, int iterationNumber)
        {
            var headerTemplate = GetChildTemplate(headerTemplateName, resolverDelegateModel ?? model, silentlyContinueOnError, customResolverDelegate);
            if (headerTemplate != null)
            {
                if (customRenderChildTemplateDelegate != null)
                {
                    customRenderChildTemplateDelegate.Invoke(templateName, headerTemplate, null, renderAsEnumerable.Value, silentlyContinueOnError, iterationNumber + 1, iterationCount);
                }
                else
                {
                    RenderTemplate(headerTemplate, null, iterationNumber - 1, iterationCount, templateName);
                }
            }
        }

        private void RenderSeparatorTemplate(string templateName, object model, bool? renderAsEnumerable, bool silentlyContinueOnError, string separatorTemplateName, Func<string, string, Type, object, bool> customResolverDelegate, object resolverDelegateModel, Action<string, object, object, bool, bool, int?, int?> customRenderChildTemplateDelegate, int iterationCount, int iterationNumber, object item)
        {
            var separatorTemplate = GetChildTemplate(separatorTemplateName, resolverDelegateModel ?? model, silentlyContinueOnError, customResolverDelegate);
            if (separatorTemplate != null)
            {
                if (customRenderChildTemplateDelegate != null)
                {
                    customRenderChildTemplateDelegate.Invoke(templateName, separatorTemplate, item, renderAsEnumerable.Value, silentlyContinueOnError, iterationNumber, iterationCount);
                }
                else
                {
                    RenderTemplate(separatorTemplate, item, iterationNumber, iterationCount, templateName);
                }
            }
        }

        private void RenderFooterTemplate(string templateName, object model, bool? renderAsEnumerable, bool silentlyContinueOnError, string footerTemplateName, Func<string, string, Type, object, bool> customResolverDelegate, object resolverDelegateModel, Action<string, object, object, bool, bool, int?, int?> customRenderChildTemplateDelegate, int iterationCount, int iterationNumber)
        {
            var footerTemplate = GetChildTemplate(footerTemplateName, resolverDelegateModel ?? model, silentlyContinueOnError, customResolverDelegate);
            if (footerTemplate != null)
            {
                if (customRenderChildTemplateDelegate != null)
                {
                    customRenderChildTemplateDelegate.Invoke(templateName, footerTemplate, null, renderAsEnumerable.Value, silentlyContinueOnError, iterationNumber + 1, iterationCount);
                }
                else
                {
                    RenderTemplate(footerTemplate, null, iterationNumber + 1, iterationCount, templateName);
                }
            }
        }

        public Func<string, object, bool, Func<string, string, Type, object, bool>, Action<string>, object> GetChildTemplateDelegate { get; set; }

        protected virtual object GetChildTemplate(string templateName, object model = null, bool silentlyContinueOnError = false, Func<string, string, Type, object, bool> customResolverDelegate = null)
        {
            return GetChildTemplateDelegate?.Invoke(templateName, model, silentlyContinueOnError, customResolverDelegate, errorMessage => Error(errorMessage));
        }

        public Func<string, object, bool, Func<string, string, Type, object, bool>, Action<string>, object> GetViewModelDelegate { get; set; }

        protected virtual object GetViewModel(string viewModelName, object model = null, bool silentlyContinueOnError = false, Func<string, string, Type, object, bool> customResolverDelegate = null)
        {
            return GetViewModelDelegate?.Invoke(viewModelName, model, silentlyContinueOnError, customResolverDelegate, errorMessage => Error(errorMessage));
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
