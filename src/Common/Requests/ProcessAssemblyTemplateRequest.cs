using System;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Requests
{
    public class ProcessAssemblyTemplateRequest<TState>
        where TState : class
    {
        public ProcessAssemblyTemplateRequest(AssemblyTemplate assemblyTemplate,
                                              TemplateParameter[] parameters,
                                              ITextTemplateProcessorContext<TState> context)
        {
            AssemblyTemplate = assemblyTemplate ?? throw new ArgumentNullException(nameof(assemblyTemplate));
            Parameters = parameters ?? Array.Empty<TemplateParameter>();
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public AssemblyTemplate AssemblyTemplate { get; }
        public TemplateParameter[] Parameters { get; }
        public ITextTemplateProcessorContext<TState> Context { get; }
    }
}
