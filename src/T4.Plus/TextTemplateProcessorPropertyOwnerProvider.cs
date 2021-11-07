using System;
using System.ComponentModel;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Plus
{
    public class TextTemplateProcessorPropertyOwnerProvider<TState> : ITextTemplateProcessorPropertyOwnerProvider<TState>
        where TState : class
    {
        public object Get(ITextTemplateProcessorContext<TState> context,
                          PropertyDescriptor property,
                          TemplateCompilerOutput<TState> templateCompilerOutput)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (templateCompilerOutput == null)
            {
                throw new ArgumentNullException(nameof(templateCompilerOutput));
            }

            return property.ComponentType == templateCompilerOutput.Template.GetType()
                ? templateCompilerOutput.Template
                : templateCompilerOutput.Template.GetType().GetProperty("ViewModel").GetValue(templateCompilerOutput.Template);
        }
    }
}
