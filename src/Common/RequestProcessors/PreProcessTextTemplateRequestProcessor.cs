using System;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.Requests;

namespace TextTemplateTransformationFramework.Common.RequestProcessors
{
    public class PreProcessTextTemplateRequestProcessor<TState> : IRequestProcessor<PreProcessTextTemplateRequest<TState>, ProcessResult>
        where TState : class
    {
        private readonly ITemplateOutputCreator<TState> _templateOutputCreator;

        public PreProcessTextTemplateRequestProcessor(ITemplateOutputCreator<TState> templateOutputCreator)
        {
            _templateOutputCreator = templateOutputCreator ?? throw new ArgumentNullException(nameof(templateOutputCreator));
        }

        public ProcessResult Process(PreProcessTextTemplateRequest<TState> request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            TemplateCodeOutput<TState> templateCodeOutput = null;

            try
            {
                templateCodeOutput = GetTemplateCodeOutput(request.Context);

                return ProcessResult.Create
                (
                    templateCodeOutput.Errors,
                    output: null,
                    templateCodeOutput.SourceCode,
                    request.Context.Logger.Aggregate(),
                    templateCodeOutput.OutputExtension,
                    templateCodeOutput.Exception
                );
            }
            catch (Exception exception)
            {
                return ProcessResult.Create
                (
                    errors: null,
                    output: null,
                    templateCodeOutput?.SourceCode,
                    request.Context.Logger.Aggregate(),
                    templateCodeOutput?.OutputExtension,
                    exception
                );
            }
        }

        private TemplateCodeOutput<TState> GetTemplateCodeOutput(ITextTemplateProcessorContext<TState> context)
            => _templateOutputCreator.Create(context);
    }
}
