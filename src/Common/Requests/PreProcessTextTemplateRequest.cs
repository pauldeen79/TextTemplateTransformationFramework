using System;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Requests
{
    public class PreProcessTextTemplateRequest<TState>
        where TState : class
    {
        public PreProcessTextTemplateRequest(TextTemplate textTemplate,
                                             TemplateParameter[] parameters,
                                             ITextTemplateProcessorContext<TState> context)
        {
            TextTemplate = textTemplate ?? throw new ArgumentNullException(nameof(textTemplate));
            Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public TextTemplate TextTemplate { get; }
        public TemplateParameter[] Parameters { get; }
        public ITextTemplateProcessorContext<TState> Context { get; }
    }
}
