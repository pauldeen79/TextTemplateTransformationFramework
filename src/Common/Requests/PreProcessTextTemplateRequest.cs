using System;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Requests
{
    public class PreProcessTextTemplateRequest<TState>
        where TState : class
    {
        public PreProcessTextTemplateRequest(TemplateParameter[] parameters,
                                             ITextTemplateProcessorContext<TState> context)
        {
            Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public TemplateParameter[] Parameters { get; }
        public ITextTemplateProcessorContext<TState> Context { get; }
    }
}
