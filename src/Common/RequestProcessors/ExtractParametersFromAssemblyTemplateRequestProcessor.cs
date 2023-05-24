using System;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.Requests;

namespace TextTemplateTransformationFramework.Common.RequestProcessors
{
    public class ExtractParametersFromAssemblyTemplateRequestProcessor<TState> : IRequestProcessor<ExtractParametersFromAssemblyTemplateRequest<TState>, ExtractParametersResult>
        where TState : class
    {
        private readonly ITemplateCodeCompiler<TState> _templateCodeCompiler;
        private readonly ITemplateOutputCreator<TState> _templateOutputCreator;
        private readonly ITextTemplateParameterExtractor<TState> _templateParameterExtractor;
        
        public ExtractParametersFromAssemblyTemplateRequestProcessor(ITemplateCodeCompiler<TState> templateCodeCompiler, 
                                                                     ITemplateOutputCreator<TState> templateOutputCreator, 
                                                                     ITextTemplateParameterExtractor<TState> templateParameterExtractor)
        {
            _templateCodeCompiler = templateCodeCompiler ?? throw new ArgumentNullException(nameof(templateCodeCompiler));
            _templateOutputCreator = templateOutputCreator ?? throw new ArgumentNullException(nameof(templateOutputCreator));
            _templateParameterExtractor = templateParameterExtractor ?? throw new ArgumentNullException(nameof(templateParameterExtractor));
        }

        public ExtractParametersResult Process(ExtractParametersFromAssemblyTemplateRequest<TState> request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            TemplateCodeOutput<TState> templateCodeOutput = null;
            var parameters = Enumerable.Empty<TemplateParameter>();
            
            try
            {
                templateCodeOutput = GetTemplateCodeOutput(request.Context);
                var templateCompilerOutput = GetTemplateCompilerOutput(request.Context, templateCodeOutput);
                parameters = _templateParameterExtractor.Extract(request.Context, templateCompilerOutput);

                return ExtractParametersResult.Create
                (
                    parameters,
                    Array.Empty<CompilerError>(),
                    templateCodeOutput.SourceCode,
                    request.Context.Logger.Aggregate()
                );
            }
            catch (Exception exception)
            {
                return ExtractParametersResult.Create
                (
                    parameters,
                    Array.Empty<CompilerError>(),
                    templateCodeOutput?.SourceCode,
                    request.Context.Logger.Aggregate(),
                    exception
                );
            }
        }

        private TemplateCodeOutput<TState> GetTemplateCodeOutput(ITextTemplateProcessorContext<TState> context)
            => _templateOutputCreator.Create(context);

        private TemplateCompilerOutput<TState> GetTemplateCompilerOutput(ITextTemplateProcessorContext<TState> context, TemplateCodeOutput<TState> codeOutput)
            => _templateCodeCompiler.Compile(context, codeOutput);
    }
}
