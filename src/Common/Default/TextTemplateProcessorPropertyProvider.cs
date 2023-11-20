using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Default
{
    public class TextTemplateProcessorPropertyProvider<TState> : ITextTemplateProcessorPropertyProvider<TState>
        where TState : class
    {
        public IEnumerable<PropertyDescriptor> Get(ITextTemplateProcessorContext<TState> context,
                                                   TemplateCompilerOutput<TState> templateCompilerOutput,
                                                   Type templateType)
        {
            if (templateCompilerOutput is null)
            {
                throw new ArgumentNullException(nameof(templateCompilerOutput));
            }

            return TypeDescriptor
                .GetProperties(templateCompilerOutput.Template)
                .Cast<PropertyDescriptor>()
                .Where(p => p.ComponentType == templateType);
        }
    }
}
