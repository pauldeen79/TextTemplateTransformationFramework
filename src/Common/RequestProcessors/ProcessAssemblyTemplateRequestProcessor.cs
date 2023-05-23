using System;
using System.Linq;
using System.Runtime.Loader;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.Requests;

namespace TextTemplateTransformationFramework.Common.RequestProcessors
{
    public class ProcessAssemblyTemplateRequestProcessor<TState> : IRequestProcessor<ProcessAssemblyTemplateRequest<TState>, ProcessResult>
        where TState : class
    {
        private readonly ITemplateProcessor<TState> _templateProcessor;
        private readonly IAssemblyService _assemblyService;

        public ProcessAssemblyTemplateRequestProcessor(ITemplateProcessor<TState> templateProcessor,
                                                       IAssemblyService assemblyService)
        {
            _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
            _assemblyService = assemblyService ?? throw new ArgumentNullException(nameof(assemblyService));
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
                templateCodeOutput = GetTemplateCodeOutput();
                templateCompilerOutput = GetTemplateCompilerOutput(request.AssemblyTemplate);
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

        private TemplateCodeOutput<TState> GetTemplateCodeOutput()
            => new TemplateCodeOutput<TState>(Enumerable.Empty<ITemplateToken<TState>>(), new CodeGeneratorResult(string.Empty, "C#", Enumerable.Empty<CompilerError>()), string.Empty, Enumerable.Empty<string>(), Enumerable.Empty<string>(), string.Empty, string.Empty);

        private TemplateCompilerOutput<TState> GetTemplateCompilerOutput(AssemblyTemplate template)
        {
            var assembly = _assemblyService.LoadAssembly(template.AssemblyName, AssemblyLoadContext.Default);
            return new TemplateCompilerOutput<TState>(assembly, assembly.GetExportedTypes().FirstOrDefault(x => x.FullName == template.ClassName), Enumerable.Empty<CompilerError>(), string.Empty, string.Empty, Enumerable.Empty<ITemplateToken<TState>>());
        }

        private ProcessResult RenderTemplate(ITextTemplateProcessorContext<TState> context, TemplateCompilerOutput<TState> templateCompilerOutput)
            => _templateProcessor.Process(context, templateCompilerOutput);
    }
}
