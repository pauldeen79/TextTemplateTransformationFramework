using System;
using System.Linq;
using ScriptCompiler;
using ScriptCompiler.Extensions;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateTokens;
using TextTemplateTransformationFramework.Common.Extensions;

namespace TextTemplateTransformationFramework.Common.Default
{
    public class TemplateCodeCompiler<TState> : ITemplateCodeCompiler<TState>
        where TState : class
    {
        private readonly ICodeCompiler<TState> _codeCompiler;
        private readonly ITemplateFactory<TState> _templateFactory;

        public TemplateCodeCompiler(ICodeCompiler<TState> codeCompiler, ITemplateFactory<TState> templateFactory)
        {
            _codeCompiler = codeCompiler ?? throw new ArgumentNullException(nameof(codeCompiler));
            _templateFactory = templateFactory ?? throw new ArgumentNullException(nameof(templateFactory));
        }

        public TemplateCompilerOutput<TState> Compile(ITextTemplateProcessorContext<TState> context, TemplateCodeOutput<TState> codeOutput)
        {
            if (codeOutput == null)
            {
                throw new ArgumentNullException(nameof(codeOutput));
            }

            var result = DoCompile(context, codeOutput);
            var template = result?.Errors.HasErrors() != false
                ? null
                : CreateTemplate(context, codeOutput, result);
            var errors = result?.Errors == null
                ? null
                : result.Errors.Select(CreateCompilerError(codeOutput)).ToList();

            if (errors != null && errors.HasErrors())
            {
                //when compilation fails, add parse-time errors.
                errors.AddRange
                (
                    codeOutput
                        .SourceTokens
                        .GetTemplateTokensFromSections<TState, IMessageToken<TState>>()
                        .Select(e => e.ToCompilerError())
                        .ToArray()
                );
            }

            return TemplateCompilerOutput.Create
            (
                result?.CompiledAssembly,
                template,
                errors,
                codeOutput.SourceCode,
                codeOutput.OutputExtension,
                codeOutput.SourceTokens,
                codeOutput.Errors
            );
        }

        private static Func<Microsoft.CodeAnalysis.Diagnostic, CompilerError> CreateCompilerError(TemplateCodeOutput<TState> codeOutput)
            => d =>
                new CompilerError
                (
                    d.Location.GetLineSpan().StartLinePosition.Line,
                    d.Id,
                    d.GetMessage(),
                    codeOutput.SourceTokens.GetTemplateTokensFromSections().FirstOrDefault()?.FileName ?? "Unknown.tt",
                    d.Severity == Microsoft.CodeAnalysis.DiagnosticSeverity.Warning,
                    d.Location.GetLineSpan().StartLinePosition.Character
                );

        private CompilerResults DoCompile(ITextTemplateProcessorContext<TState> context,
                                        TemplateCodeOutput<TState> codeOutput)
            => _codeCompiler.Compile(context, codeOutput);

        private object CreateTemplate(ITextTemplateProcessorContext<TState> context,
                                      TemplateCodeOutput<TState> codeOutput,
                                      CompilerResults result)
            => _templateFactory.Create(context, codeOutput, result);
    }
}
