using System;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Requests;

namespace TextTemplateTransformationFramework.Common.Default
{
    public class TemplateProcessor<TState> : ITemplateProcessor<TState>
        where TState : class
    {
        private readonly IRequestProcessor<ProcessTemplateRequest<TState>, ProcessResult> _processTemplateProcessor;

        public TemplateProcessor(IRequestProcessor<ProcessTemplateRequest<TState>, ProcessResult> processTemplateProcessor)
        {
            _processTemplateProcessor = processTemplateProcessor ?? throw new ArgumentNullException(nameof(processTemplateProcessor));
        }

        public ProcessResult Process(ITextTemplateProcessorContext<TState> context, TemplateCompilerOutput<TState> templateCompilerOutput)
            => _processTemplateProcessor.Process(new ProcessTemplateRequest<TState>(new TemplateProcessorContext<TState>(context, templateCompilerOutput), templateCompilerOutput));
    }
}
