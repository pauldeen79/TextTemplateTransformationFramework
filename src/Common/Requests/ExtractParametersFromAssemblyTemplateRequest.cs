using System;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Requests
{
    public class ExtractParametersFromAssemblyTemplateRequest<TState>
        where TState : class
    {
        public ExtractParametersFromAssemblyTemplateRequest(ITextTemplateProcessorContext<TState> context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ITextTemplateProcessorContext<TState> Context { get; }
    }
}
