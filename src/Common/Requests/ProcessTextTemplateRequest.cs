using System;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Requests
{
    public class ProcessTextTemplateRequest<TState>
        where TState : class
    {
        public ProcessTextTemplateRequest(TextTemplate textTemplate,
                                          TemplateParameter[] parameters,
                                          ITextTemplateProcessorContext<TState> context)
        {
            TextTemplate = textTemplate ?? throw new ArgumentNullException(nameof(textTemplate));
            Parameters = parameters ?? Array.Empty<TemplateParameter>();
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public TextTemplate TextTemplate { get; }
        public TemplateParameter[] Parameters { get; }
        public ITextTemplateProcessorContext<TState> Context { get; }
    }
}
