using System;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Requests
{
    public class ProcessAssemblyTemplateRequest<TState>
        where TState : class
    {
        public ProcessAssemblyTemplateRequest(TemplateParameter[] parameters,
                                              ITextTemplateProcessorContext<TState> context)
        {
            Parameters = parameters ?? Array.Empty<TemplateParameter>();
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public TemplateParameter[] Parameters { get; }
        public ITextTemplateProcessorContext<TState> Context { get; }
    }
}
