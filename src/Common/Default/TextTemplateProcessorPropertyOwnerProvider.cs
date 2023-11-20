using System;
using System.ComponentModel;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Default
{
    public class TextTemplateProcessorPropertyOwnerProvider<TState> : ITextTemplateProcessorPropertyOwnerProvider<TState>
        where TState : class
    {
        public object Get(ITextTemplateProcessorContext<TState> context,
                          PropertyDescriptor property,
                          TemplateCompilerOutput<TState> templateCompilerOutput)
        {
            if (templateCompilerOutput is null)
            {
                throw new ArgumentNullException(nameof(templateCompilerOutput));
            }

            return templateCompilerOutput.Template;
        }
    }
}
