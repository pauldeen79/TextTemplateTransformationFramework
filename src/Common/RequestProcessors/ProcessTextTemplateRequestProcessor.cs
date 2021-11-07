using System;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.Requests;

namespace TextTemplateTransformationFramework.Common.RequestProcessors
{
    public class ProcessTextTemplateRequestProcessor<TState> : IRequestProcessor<ProcessTextTemplateRequest<TState>, ProcessResult>
        where TState : class
    {
        private readonly ITemplateOutputCreator<TState> _templateOutputCreator;
        private readonly ITemplateCodeCompiler<TState> _templateCodeCompiler;
        private readonly ITemplateProcessor<TState> _templateProcessor;

        public ProcessTextTemplateRequestProcessor(ITemplateOutputCreator<TState> templateOutputCreator,
                                                   ITemplateCodeCompiler<TState> templateCodeCompiler,
                                                   ITemplateProcessor<TState> templateProcessor)
        {
            _templateOutputCreator = templateOutputCreator ?? throw new ArgumentNullException(nameof(templateOutputCreator));
            _templateCodeCompiler = templateCodeCompiler ?? throw new ArgumentNullException(nameof(templateCodeCompiler));
            _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
        }

        public ProcessResult Process(ProcessTextTemplateRequest<TState> request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            TemplateCodeOutput<TState> templateCodeOutput = null;
            TemplateCompilerOutput<TState> templateCompilerOutput = null;

            try
            {
                templateCodeOutput = GetTemplateCodeOutput(request.Context);
                templateCompilerOutput = GetTemplateCompilerOutput(request.Context, templateCodeOutput);
                return RenderTemplate(request.Context, templateCompilerOutput);
            }
            catch (Exception exception)
            {
                return ProcessResult.Create
                (
                    templateCompilerOutput?.Errors,
                    output: null,
                    templateCodeOutput?.SourceCode,
                    request.Context.Logger.Aggregate(),
                    templateCompilerOutput?.OutputExtension,
                    exception
                );
            }
        }

        private TemplateCodeOutput<TState> GetTemplateCodeOutput(ITextTemplateProcessorContext<TState> context)
            => _templateOutputCreator.Create(context);

        private TemplateCompilerOutput<TState> GetTemplateCompilerOutput(ITextTemplateProcessorContext<TState> context, TemplateCodeOutput<TState> codeOutput)
            => _templateCodeCompiler.Compile(context, codeOutput);

        private ProcessResult RenderTemplate(ITextTemplateProcessorContext<TState> context, TemplateCompilerOutput<TState> templateCompilerOutput)
            => _templateProcessor.Process(context, templateCompilerOutput);
    }
}
