using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus
{
    public class TextTemplateProcessorPropertyProvider<TState> : ITextTemplateProcessorPropertyProvider<TState>
        where TState : class
    {
        public IEnumerable<PropertyDescriptor> Get(ITextTemplateProcessorContext<TState> context,
                                                   TemplateCompilerOutput<TState> templateCompilerOutput,
                                                   Type templateType)
        {
            if (templateCompilerOutput == null)
            {
                throw new ArgumentNullException(nameof(templateCompilerOutput));
            }

            return TypeDescriptor
                .GetProperties(templateCompilerOutput.Template)
                .Cast<PropertyDescriptor>()
                .Where(p => p.Name != "ViewModel" && p.ComponentType == templateType)
                .Concat(GetViewModelProperties(templateCompilerOutput));
        }

        private IEnumerable<PropertyDescriptor> GetViewModelProperties(TemplateCompilerOutput<TState> templateCompilerOutput)
        {
            var viewModelProperty = templateCompilerOutput.Template.GetType().GetProperty("ViewModel");

            if (viewModelProperty == null)
            {
                return Enumerable.Empty<PropertyDescriptor>();
            }

            var viewModelPropertyValue = viewModelProperty.GetValue(templateCompilerOutput.Template);

            if (viewModelPropertyValue == null)
            {
                viewModelPropertyValue = Activator.CreateInstance(viewModelProperty.PropertyType);
                viewModelProperty.SetValue(templateCompilerOutput.Template, viewModelPropertyValue);
            }

            return TypeDescriptor
                .GetProperties(viewModelPropertyValue)
                .Cast<PropertyDescriptor>()
                .Where(p => p.IsBrowsable && p.ComponentType == viewModelPropertyValue.GetType());
        }
    }
}
