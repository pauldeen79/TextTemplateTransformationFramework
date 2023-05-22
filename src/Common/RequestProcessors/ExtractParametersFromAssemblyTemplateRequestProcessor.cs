using System;
using System.Linq;
using System.Runtime.Loader;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.Requests;

namespace TextTemplateTransformationFramework.Common.RequestProcessors
{
    public class ExtractParametersFromAssemblyTemplateRequestProcessor<TState> : IRequestProcessor<ExtractParametersFromAssemblyTemplateRequest<TState>, ExtractParametersResult>
        where TState : class
    {
        private readonly ITextTemplateParameterExtractor<TState> _templateParameterExtractor;
        private readonly IAssemblyService _assemblyService;

        public ExtractParametersFromAssemblyTemplateRequestProcessor(ITextTemplateParameterExtractor<TState> templateParameterExtractor,
                                                                     IAssemblyService assemblyService)
        {
            _templateParameterExtractor = templateParameterExtractor ?? throw new ArgumentNullException(nameof(templateParameterExtractor));
            _assemblyService = assemblyService ?? throw new ArgumentNullException(nameof(assemblyService));
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
                templateCodeOutput = GetTemplateCodeOutput();
                var templateCompilerOutput = GetTemplateCompilerOutput(request.Context.AssemblyTemplate);
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

        private TemplateCodeOutput<TState> GetTemplateCodeOutput()
            => new TemplateCodeOutput<TState>(Enumerable.Empty<ITemplateToken<TState>>(), new CodeGeneratorResult(string.Empty, "C#", Enumerable.Empty<CompilerError>()), string.Empty, Enumerable.Empty<string>(), Enumerable.Empty<string>(), string.Empty, string.Empty);

        private TemplateCompilerOutput<TState> GetTemplateCompilerOutput(AssemblyTemplate template)
        {
            var assembly = _assemblyService.LoadAssembly(template.AssemblyName, AssemblyLoadContext.Default);
            return new TemplateCompilerOutput<TState>(assembly, assembly.GetExportedTypes().FirstOrDefault(x => x.FullName == template.ClassName), Enumerable.Empty<CompilerError>(), string.Empty, string.Empty, Enumerable.Empty<ITemplateToken<TState>>());
        }
    }
}
