using System;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Requests;

namespace TextTemplateTransformationFramework.Common.Decorators
{
    public class ProcessTemplateRequestProcessorDecorator<TState> : IRequestProcessor<ProcessTemplateRequest<TState>, ProcessResult>
        where TState : class
    {
        private readonly IRequestProcessor<ProcessTemplateRequest<TState>, ProcessResult> _baseProcessor;
        private readonly IProcessInitializer<ITemplateProcessorContext<TState>> _processInitializer;
        private readonly IProcessFinalizer<ITemplateProcessorContext<TState>> _processFinalizer;

        public ProcessTemplateRequestProcessorDecorator(IRequestProcessor<ProcessTemplateRequest<TState>, ProcessResult> baseProcessor,
                                                        IProcessInitializer<ITemplateProcessorContext<TState>> processInitializer,
                                                        IProcessFinalizer<ITemplateProcessorContext<TState>> processFinalizer)
        {
            _baseProcessor = baseProcessor ?? throw new ArgumentNullException(nameof(baseProcessor));
            _processInitializer = processInitializer ?? throw new ArgumentNullException(nameof(processInitializer));
            _processFinalizer = processFinalizer ?? throw new ArgumentNullException(nameof(processFinalizer));
        }

        public ProcessResult Process(ProcessTemplateRequest<TState> request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            _processInitializer.Initialize(request.Context);
            try
            {
                return _baseProcessor.Process(request);
            }
            finally
            {
                _processFinalizer.Finalize(request.Context);
            }
        }
    }
}
