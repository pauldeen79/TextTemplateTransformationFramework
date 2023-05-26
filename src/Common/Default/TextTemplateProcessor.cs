using System;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Requests;

namespace TextTemplateTransformationFramework.Common.Default
{
    public class TextTemplateProcessor<TState> : ITextTemplateProcessor
        where TState : class
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IRequestProcessor<ProcessTextTemplateRequest<TState>, ProcessResult> _processTextTemplateProcessor;
        private readonly IRequestProcessor<PreProcessTextTemplateRequest<TState>, ProcessResult> _preProcessTextTemplateProcessor;
        private readonly IRequestProcessor<ExtractParametersFromTextTemplateRequest<TState>, ExtractParametersResult> _extractParametersFromTextTemplateProcessor;

        public TextTemplateProcessor(ILoggerFactory loggerFactory,
                                     IRequestProcessor<ProcessTextTemplateRequest<TState>, ProcessResult> processTextTemplateProcessor,
                                     IRequestProcessor<PreProcessTextTemplateRequest<TState>, ProcessResult> preProcessTextTemplateProcessor,
                                     IRequestProcessor<ExtractParametersFromTextTemplateRequest<TState>, ExtractParametersResult> extractParametersFromTextTemplateProcessor)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _processTextTemplateProcessor = processTextTemplateProcessor ?? throw new ArgumentNullException(nameof(processTextTemplateProcessor));
            _preProcessTextTemplateProcessor = preProcessTextTemplateProcessor ?? throw new ArgumentNullException(nameof(preProcessTextTemplateProcessor));
            _extractParametersFromTextTemplateProcessor = extractParametersFromTextTemplateProcessor ?? throw new ArgumentNullException(nameof(extractParametersFromTextTemplateProcessor));
        }

        public ProcessResult Process(TextTemplate textTemplate, TemplateParameter[] parameters)
            => _processTextTemplateProcessor.Process
            (
                new ProcessTextTemplateRequest<TState>
                (
                    parameters,
                    CreateContext(textTemplate, parameters)
                )
            );

        public ProcessResult Process(AssemblyTemplate assemblyTemplate, TemplateParameter[] parameters)
            => _processTextTemplateProcessor.Process
            (
                new ProcessTextTemplateRequest<TState>
                (
                    parameters,
                    CreateContext(assemblyTemplate, parameters)
                )
            );

        public ProcessResult PreProcess(TextTemplate textTemplate, TemplateParameter[] parameters)
            => _preProcessTextTemplateProcessor.Process
            (
                new PreProcessTextTemplateRequest<TState>
                (
                    parameters,
                    CreateContext(textTemplate, parameters)
                )
            );

        public ExtractParametersResult ExtractParameters(TextTemplate textTemplate)
            => _extractParametersFromTextTemplateProcessor.Process
            (
                new ExtractParametersFromTextTemplateRequest<TState>
                (
                    CreateContext(textTemplate, Array.Empty<TemplateParameter>())
                )
            );

        public ExtractParametersResult ExtractParameters(AssemblyTemplate assemblyTemplate)
            => _extractParametersFromTextTemplateProcessor.Process
            (
                new ExtractParametersFromTextTemplateRequest<TState>
                (
                    CreateContext(assemblyTemplate, Array.Empty<TemplateParameter>())
                )
            );

        private ITextTemplateProcessorContext<TState> CreateContext(TextTemplate textTemplate, TemplateParameter[] parameters)
            => new TextTemplateProcessorContext<TState>
            (
                textTemplate,
                parameters,
                _loggerFactory.Create(),
                null
            );

        private ITextTemplateProcessorContext<TState> CreateContext(AssemblyTemplate assemblyTemplate, TemplateParameter[] parameters)
            => new TextTemplateProcessorContext<TState>
            (
                assemblyTemplate,
                parameters,
                _loggerFactory.Create(),
                null
            );
    }
}
