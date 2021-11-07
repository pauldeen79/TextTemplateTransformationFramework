using System;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Requests
{
    public class ExtractParametersFromTextTemplateRequest<TState>
        where TState : class
    {
        public ExtractParametersFromTextTemplateRequest(TextTemplate textTemplate,
                                                        ITextTemplateProcessorContext<TState> context)
        {
            TextTemplate = textTemplate ?? throw new ArgumentNullException(nameof(textTemplate));
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public TextTemplate TextTemplate { get; }
        public ITextTemplateProcessorContext<TState> Context { get; }
    }
}
