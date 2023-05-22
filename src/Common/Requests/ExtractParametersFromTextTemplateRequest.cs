using System;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Requests
{
    public class ExtractParametersFromTextTemplateRequest<TState>
        where TState : class
    {
        public ExtractParametersFromTextTemplateRequest(ITextTemplateProcessorContext<TState> context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ITextTemplateProcessorContext<TState> Context { get; }
    }
}
