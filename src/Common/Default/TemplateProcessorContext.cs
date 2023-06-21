using System;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Default
{
    public class TemplateProcessorContext<TState> : ContextBase, ITemplateProcessorContext<TState>
        where TState : class
    {
        public TemplateProcessorContext(ITextTemplateProcessorContext<TState> textTemplateProcessorContext,
                                        TemplateCompilerOutput<TState> templateCompilerOutput)
        {
            TextTemplateProcessorContext = textTemplateProcessorContext ?? throw new ArgumentNullException(nameof(textTemplateProcessorContext));
            TemplateCompilerOutput = templateCompilerOutput ?? throw new ArgumentNullException(nameof(templateCompilerOutput));
        }

        public ITextTemplateProcessorContext<TState> TextTemplateProcessorContext { get; }
        public TemplateCompilerOutput<TState> TemplateCompilerOutput { get; }
    }
}
