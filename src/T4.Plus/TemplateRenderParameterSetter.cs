using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;

namespace TextTemplateTransformationFramework.T4.Plus
{
    public class TemplateRenderParameterSetter<TState> : ITemplateRenderParameterSetter<TState>
        where TState : class
    {
        public void Set(ITemplateProcessorContext<TState> context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            //Set ViewModel property, if available
            var viewModelProperty = context.TemplateCompilerOutput.Template.GetType().GetProperty("ViewModel");
            if (viewModelProperty == null)
            {
                return;
            }
            var viewModelValue = GetViewModelValue(context, viewModelProperty);

            var sessionProperty = context.TemplateCompilerOutput.Template.GetType().GetProperty("Session");
            var sessionPropertyValue = GetSessionPropertyValue(context, sessionProperty);

            var viewModelValueType = viewModelValue.GetType();
            SetPropertyValuesFromSession(viewModelValue, sessionPropertyValue, viewModelValueType);

            SetViewModelDefaultValues(viewModelProperty, viewModelValue, sessionPropertyValue);

            var templateContextProp = viewModelValueType.GetProperty("TemplateContext");
            var templateContextProperty = context.TemplateCompilerOutput.Template.GetType().GetProperty("TemplateContext", Constants.BindingFlags);
            if (templateContextProp != null && templateContextProperty != null)
            {
                templateContextProp.SetValue(viewModelValue, templateContextProperty.GetValue(context.TemplateCompilerOutput.Template));
            }
        }

        private static void SetViewModelDefaultValues(PropertyInfo viewModelProperty, object viewModelValue, IDictionary<string, object> sessionPropertyValue)
        {
            foreach (var info in viewModelProperty.PropertyType.GetProperties()
                .Where(p => sessionPropertyValue?.ContainsKey(p.Name) != true)
                .Select(p => new
                {
                    Property = p,
                    Attributes = p.GetCustomAttributes(typeof(DefaultValueAttribute), true).Cast<DefaultValueAttribute>()
                }
                ).Where(p => p.Attributes?.Any() == true))
            {
                if (info.Property.CanWrite && info.Property.GetSetMethod() != null)
                {
                    info.Property.SetValue(viewModelValue, new TemplateParameter { Name = info.Property.Name, Value = info.Attributes.First().Value }.ConvertType(viewModelValue.GetType()));
                }

                if (sessionPropertyValue != null && !sessionPropertyValue.ContainsKey(info.Property.Name))
                {
                    sessionPropertyValue.Add(info.Property.Name, info.Attributes.First().Value);
                }
            }
        }

        private static void SetPropertyValuesFromSession(object viewModelValue, IDictionary<string, object> sessionPropertyValue, Type viewModelValueType)
        {
            if (sessionPropertyValue == null)
            {
                return;
            }
            
            foreach (var kvp in sessionPropertyValue.Where(kvp => kvp.Key != "Model"))
            {
                var prop = viewModelValueType.GetProperty(kvp.Key);
                if (prop != null && prop.GetSetMethod() == null)
                {
                    continue;
                }
                prop?.SetValue(viewModelValue, new TemplateParameter { Name = kvp.Key, Value = kvp.Value }.ConvertType(viewModelValueType));
            }
        }

        private static IDictionary<string, object> GetSessionPropertyValue(ITemplateProcessorContext<TState> context, PropertyInfo sessionProperty)
            => sessionProperty == null
                ? null
                : (IDictionary<string, object>)sessionProperty.GetValue(context.TemplateCompilerOutput.Template, null);

        private static object GetViewModelValue(ITemplateProcessorContext<TState> context, PropertyInfo viewModelProperty)
        {
            var viewModelValue = viewModelProperty.GetValue(context.TemplateCompilerOutput.Template);
            if (viewModelValue == null)
            {
                viewModelValue = Activator.CreateInstance(viewModelProperty.PropertyType);
                viewModelProperty.SetValue(context.TemplateCompilerOutput.Template, viewModelValue);
            }

            return viewModelValue;
        }
    }
}
