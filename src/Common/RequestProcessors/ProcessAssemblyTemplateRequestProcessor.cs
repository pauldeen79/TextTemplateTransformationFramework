using System;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.Requests;

namespace TextTemplateTransformationFramework.Common.RequestProcessors
{
    public class ProcessAssemblyTemplateRequestProcessor<TState> : IRequestProcessor<ProcessAssemblyTemplateRequest<TState>, ProcessResult>
        where TState : class
    {
        private readonly ITemplateCodeCompiler<TState> _templateCodeCompiler;
        private readonly ITemplateOutputCreator<TState> _templateOutputCreator;
        private readonly ITemplateProcessor<TState> _templateProcessor;

        public ProcessAssemblyTemplateRequestProcessor(ITemplateCodeCompiler<TState> templateCodeCompiler,
                                                       ITemplateOutputCreator<TState> templateOutputCreator, 
                                                       ITemplateProcessor<TState> templateProcessor)
        {
            _templateCodeCompiler = templateCodeCompiler ?? throw new ArgumentNullException(nameof(templateCodeCompiler));
            _templateOutputCreator = templateOutputCreator ?? throw new ArgumentNullException(nameof(templateOutputCreator));
            _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
        }

        public ProcessResult Process(ProcessAssemblyTemplateRequest<TState> request)
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
