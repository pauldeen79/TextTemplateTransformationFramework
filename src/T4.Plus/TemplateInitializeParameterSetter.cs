using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;

namespace TextTemplateTransformationFramework.T4.Plus
{
    public class TemplateInitializeParameterSetter<TState> : ITemplateInitializeParameterSetter<TState>
        where TState : class
    {
        public void Set(ITemplateProcessorContext<TState> context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            //First, set Session property, if available
            new TextTemplateTransformationFramework.Common.Default.TemplateInitializeParameterSetter<TState>()
                .Set(context);

            if (context.TextTemplateProcessorContext.Parameters is null)
            {
                return;
            }

            //Second, set ViewModel property, if available
            SetViewModel(context);

            //Third, set properties for all parameters that are not set from the session
            //(in the current implementation of the code generator, this is done based on parameter tokens)
            SetProperties(context);
        }

        private static void SetProperties(ITemplateProcessorContext<TState> context)
        {
            var props = TypeDescriptor
                .GetProperties(context.TemplateCompilerOutput.Template)
                .OfType<PropertyDescriptor>()
                .ToArray();

            foreach (var prop in props)
            {
                var parameter = Array.Find(context.TextTemplateProcessorContext.Parameters, p => p.Name == prop.Name);
                if (parameter is null)
                {
                    continue;
                }

                prop.SetValue(context.TemplateCompilerOutput.Template, parameter.ConvertType(prop.PropertyType));
            }
        }

        private static void SetViewModel(ITemplateProcessorContext<TState> context)
        {
            var viewModelProperty = context.TemplateCompilerOutput.Template.GetType().GetProperty("ViewModel");
            if (viewModelProperty is null)
            {
                return;
            }

            var viewModelValue = viewModelProperty.GetValue(context.TemplateCompilerOutput.Template);
            if (viewModelValue is null)
            {
                viewModelValue = Activator.CreateInstance(viewModelProperty.PropertyType);
                viewModelProperty.SetValue(context.TemplateCompilerOutput.Template, viewModelValue);
            }

            var sessionProperty = context.TemplateCompilerOutput.Template.GetType().GetProperty("Session");
            var sessionPropertyValue = sessionProperty is null
                ? null
                : (IDictionary<string, object>)sessionProperty.GetValue(context.TemplateCompilerOutput.Template, null);

            if (sessionPropertyValue is not null)
            {
                var viewModelValueType = viewModelValue.GetType();
                foreach (var kvp in sessionPropertyValue.Where(kvp => kvp.Key != "Model"))
                {
                    var prop = viewModelValueType.GetProperty(kvp.Key);
                    if (prop is not null && prop.GetSetMethod() is null) { continue; }
                    prop?.SetValue(viewModelValue, new TemplateParameter { Name = kvp.Key, Value = kvp.Value }.ConvertType(viewModelValueType));
                }
            }

            foreach (var info in viewModelProperty.PropertyType.GetProperties()
                .Where(p => sessionPropertyValue?.ContainsKey(p.Name) != true
                    && p.CanWrite
                    && p.GetSetMethod() is not null)
                .Select(p => new
                {
                    Property = p,
                    Attributes = p.GetCustomAttributes(typeof(DefaultValueAttribute), true).Cast<DefaultValueAttribute>()
                }
                ).Where(p => p.Attributes?.Any() == true))
            {
                info.Property.SetValue(viewModelValue, new TemplateParameter { Name = info.Property.Name, Value = info.Attributes.First().Value }.ConvertType(viewModelValue.GetType()));
            }
        }
    }
}
