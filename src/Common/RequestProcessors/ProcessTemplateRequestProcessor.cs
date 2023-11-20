using System;
using System.Collections.Generic;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Requests;

namespace TextTemplateTransformationFramework.Common.RequestProcessors
{
    public class ProcessTemplateRequestProcessor<TState> : IRequestProcessor<ProcessTemplateRequest<TState>, ProcessResult>
        where TState : class
    {
        private readonly ITemplateInitializer<TState> _templateInitializer;
        private readonly ITemplateRenderer<TState> _templateRenderer;
        private readonly ITemplateCompilerOutputValidator<TState> _templateCompilerOutputValidator;

        public ProcessTemplateRequestProcessor(ITemplateInitializer<TState> templateInitializer,
                                               ITemplateRenderer<TState> templateRenderer,
                                               ITemplateCompilerOutputValidator<TState> templateCompilerOutputValidator)
        {
            _templateInitializer = templateInitializer ?? throw new ArgumentNullException(nameof(templateInitializer));
            _templateRenderer = templateRenderer ?? throw new ArgumentNullException(nameof(templateRenderer));
            _templateCompilerOutputValidator = templateCompilerOutputValidator ?? throw new ArgumentNullException(nameof(templateCompilerOutputValidator));
        }

        public ProcessResult Process(ProcessTemplateRequest<TState> request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            IEnumerable<CompilerError> compilerErrorCollection = null;
            try
            {
                if (!ValidateErrors(request.Context, out compilerErrorCollection))
                {
                    //one or more errors (in parse phase)
                    return ProcessResult.Create
                    (
                        compilerErrorCollection,
                        sourceCode: request.TemplateCompilerOutput.SourceCode,
                        diagnosticDump: request.Context.TextTemplateProcessorContext.Logger.Aggregate(),
                        outputExtension: request.TemplateCompilerOutput.OutputExtension
                    );
                }

                //initialize phase
                InitializeTemplate(request.Context);

                if (!ValidateErrors(request.Context, out compilerErrorCollection))
                {
                    //one or more errors (in initialize phase)
                    return ProcessResult.Create
                    (
                        compilerErrorCollection,
                        sourceCode: request.TemplateCompilerOutput.SourceCode,
                        diagnosticDump: request.Context.TextTemplateProcessorContext.Logger.Aggregate(),
                        outputExtension: request.TemplateCompilerOutput.OutputExtension
                    );
                }
                else
                {
                    //initialize phase ok -> render
                    return RenderTemplate(request.Context);
                }
            }
            catch (Exception ex)
            {
                return ProcessResult.Create
                (
                    compilerErrorCollection ?? request.TemplateCompilerOutput.Errors,
                    null,
                    request.TemplateCompilerOutput.SourceCode,
                    request.Context.TextTemplateProcessorContext.Logger.Aggregate(),
                    outputExtension: request.TemplateCompilerOutput.OutputExtension,
                    ex
                );
            }
        }

        private void InitializeTemplate(ITemplateProcessorContext<TState> context)
            => _templateInitializer.Initialize(context);

        private ProcessResult RenderTemplate(ITemplateProcessorContext<TState> context)
            => _templateRenderer.Render(context);

        private bool ValidateErrors(ITemplateProcessorContext<TState> context, out IEnumerable<CompilerError> compilerErrorCollection)
            => _templateCompilerOutputValidator.Validate(context, out compilerErrorCollection);
    }
}
