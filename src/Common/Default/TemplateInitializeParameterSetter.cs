using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;

namespace TextTemplateTransformationFramework.Common.Default
{
    public class TemplateInitializeParameterSetter<TState> : ITemplateInitializeParameterSetter<TState>
        where TState : class
    {
        public void Set(ITemplateProcessorContext<TState> context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var type = context.TemplateCompilerOutput.Template.GetType();
            var sessionProperty = type.GetProperty("Session");

            var sessionPropertyValue = sessionProperty == null
                ? null
                : (IDictionary<string, object>)sessionProperty.GetValue(context.TemplateCompilerOutput.Template, null);

            if (sessionProperty != null && sessionPropertyValue == null)
            {
                var dict = new Dictionary<string, object>();
                sessionProperty.SetValue(context.TemplateCompilerOutput.Template, dict, null);
                sessionPropertyValue = dict;
            }

            foreach (var info in type.GetProperties().Select(p => new { Property = p, Attributes = p.GetCustomAttributes(typeof(DefaultValueAttribute), true).Cast<DefaultValueAttribute>() }).Where(p => p.Attributes?.Any() == true))
            {
                if (info.Property.CanWrite && info.Property.GetSetMethod() != null)
                {
                    info.Property.SetValue(context.TemplateCompilerOutput.Template, info.Attributes.First().Value.ConvertValue(info.Property.PropertyType));
                }

                if (sessionPropertyValue?.ContainsKey(info.Property.Name) == false)
                {
                    sessionPropertyValue.Add(info.Property.Name, info.Attributes.First().Value.ConvertValue(info.Property.PropertyType));
                }
            }

            if (context.TextTemplateProcessorContext.Parameters == null)
            {
                return;
            }

            sessionProperty?.SetValue(context.TemplateCompilerOutput.Template, context.TextTemplateProcessorContext.Parameters.ToDictionary(p => p.Name, p => p.ConvertType(type)), null);
        }
    }
}
