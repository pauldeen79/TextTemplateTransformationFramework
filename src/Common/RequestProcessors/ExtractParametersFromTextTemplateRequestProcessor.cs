using System;
using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.Requests;

namespace TextTemplateTransformationFramework.Common.RequestProcessors
{
    public class ExtractParametersFromTextTemplateRequestProcessor<TState> : IRequestProcessor<ExtractParametersFromTextTemplateRequest<TState>, ExtractParametersResult>
        where TState : class
    {
        private readonly ITemplateCodeCompiler<TState> _templateCodeCompiler;
        private readonly ITextTemplateParameterExtractor<TState> _templateParameterExtractor;
        private readonly ITemplateOutputCreator<TState> _templateOutputCreator;

        public ExtractParametersFromTextTemplateRequestProcessor(ITemplateCodeCompiler<TState> templateCodeCompiler,
                                                                 ITextTemplateParameterExtractor<TState> templateParameterExtractor,
                                                                 ITemplateOutputCreator<TState> templateOutputCreator)
        {
            _templateCodeCompiler = templateCodeCompiler ?? throw new ArgumentNullException(nameof(templateCodeCompiler));
            _templateParameterExtractor = templateParameterExtractor ?? throw new ArgumentNullException(nameof(templateParameterExtractor));
            _templateOutputCreator = templateOutputCreator ?? throw new ArgumentNullException(nameof(templateOutputCreator));
        }
        public ExtractParametersResult Process(ExtractParametersFromTextTemplateRequest<TState> request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            TemplateCodeOutput<TState> templateCodeOutput = null;
            var compilerErrors = Array.Empty<CompilerError>();
            var parameters = Enumerable.Empty<TemplateParameter>();
            
            try
            {
                templateCodeOutput = GetTemplateCodeOutput(request.Context);
                var templateCompilerOutput = GetTemplateCompilerOutput(request.Context, templateCodeOutput);
                compilerErrors = GetCompilerErrors(templateCompilerOutput).ToArray();
                parameters = _templateParameterExtractor.Extract(request.Context, templateCompilerOutput);

                return ExtractParametersResult.Create
                (
                    parameters,
                    compilerErrors,
                    templateCodeOutput.SourceCode,
                    request.Context.Logger.Aggregate()
                );
            }
            catch (Exception exception)
            {
                return ExtractParametersResult.Create
                (
                    parameters,
                    compilerErrors,
                    templateCodeOutput?.SourceCode,
                    request.Context.Logger.Aggregate(),
                    exception
                );
            }
        }

        private TemplateCodeOutput<TState> GetTemplateCodeOutput(ITextTemplateProcessorContext<TState> context)
            => _templateOutputCreator.Create(context);

        private IEnumerable<CompilerError> GetCompilerErrors(TemplateCompilerOutput<TState> templateCompilerOutput)
            => templateCompilerOutput.Errors;

        private TemplateCompilerOutput<TState> GetTemplateCompilerOutput(ITextTemplateProcessorContext<TState> context, TemplateCodeOutput<TState> codeOutput)
            => _templateCodeCompiler.Compile(context, codeOutput);
    }
}
