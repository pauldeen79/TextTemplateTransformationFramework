using System;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Requests
{
    public class ExtractParametersFromAssemblyTemplateRequest<TState>
        where TState : class
    {
        public ExtractParametersFromAssemblyTemplateRequest(AssemblyTemplate asemblyTemplate,
                                                            ITextTemplateProcessorContext<TState> context)
        {
            AssemblyTemplate = asemblyTemplate ?? throw new ArgumentNullException(nameof(asemblyTemplate));
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public AssemblyTemplate AssemblyTemplate { get; }
        public ITextTemplateProcessorContext<TState> Context { get; }
    }
}
