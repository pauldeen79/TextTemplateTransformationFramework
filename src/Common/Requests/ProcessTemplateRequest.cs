using System;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Requests
{
    public class ProcessTemplateRequest<TState>
        where TState : class
    {
        public ProcessTemplateRequest(ITemplateProcessorContext<TState> context,
                                      TemplateCompilerOutput<TState> templateCompilerOutput)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            TemplateCompilerOutput = templateCompilerOutput ?? throw new ArgumentNullException(nameof(templateCompilerOutput));
        }

        public ITemplateProcessorContext<TState> Context { get; }
        public TemplateCompilerOutput<TState> TemplateCompilerOutput { get; }
    }
}
