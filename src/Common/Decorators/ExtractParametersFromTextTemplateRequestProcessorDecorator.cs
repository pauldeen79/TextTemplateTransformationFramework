using System;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Requests;

namespace TextTemplateTransformationFramework.Common.Decorators
{
    public class ExtractParametersFromTextTemplateRequestProcessorDecorator<TState> : IRequestProcessor<ExtractParametersFromTextTemplateRequest<TState>, ExtractParametersResult>
         where TState : class
    {
        private readonly IRequestProcessor<ExtractParametersFromTextTemplateRequest<TState>, ExtractParametersResult> _baseProcessor;
        private readonly IProcessInitializer<ITextTemplateProcessorContext<TState>> _processInitializer;
        private readonly IProcessFinalizer<ITextTemplateProcessorContext<TState>> _processFinalizer;

        public ExtractParametersFromTextTemplateRequestProcessorDecorator(IRequestProcessor<ExtractParametersFromTextTemplateRequest<TState>, ExtractParametersResult> baseProcessor,
                                                                          IProcessInitializer<ITextTemplateProcessorContext<TState>> processInitializer,
                                                                          IProcessFinalizer<ITextTemplateProcessorContext<TState>> processFinalizer)
        {
            _baseProcessor = baseProcessor ?? throw new ArgumentNullException(nameof(baseProcessor));
            _processInitializer = processInitializer ?? throw new ArgumentNullException(nameof(processInitializer));
            _processFinalizer = processFinalizer ?? throw new ArgumentNullException(nameof(processFinalizer));
        }

        public ExtractParametersResult Process(ExtractParametersFromTextTemplateRequest<TState> request)
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
