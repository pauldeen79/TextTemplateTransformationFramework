using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common
{
    public static class TemplateCompilerOutput
    {
        public static TemplateCompilerOutput<TState> Create<TState>
        (
            Assembly assembly,
            object template,
            IEnumerable<CompilerError> errors,
            string sourceCode,
            string outputExtension,
            IEnumerable<ITemplateToken<TState>> sourceTokens,
            IEnumerable<CompilerError> previousPhaseErrors = null
        ) where TState : class
        => new TemplateCompilerOutput<TState>
        (
            assembly,
            template,
            Combine(previousPhaseErrors, errors),
            sourceCode,
            outputExtension,
            sourceTokens
        );

        private static IEnumerable<CompilerError> Combine(IEnumerable<CompilerError> previousPhaseErrors, IEnumerable<CompilerError> errors)
        {
            var result = new List<CompilerError>();
            if (previousPhaseErrors != null)
            {
                result.AddRange(previousPhaseErrors);
            }
            if (errors != null)
            {
                result.AddRange(errors);
            }

            return result;
        }

    }

    public sealed class TemplateCompilerOutput<TState>
        where TState : class
    {
        internal TemplateCompilerOutput
        (
            Assembly assembly,
            object template,
            IEnumerable<CompilerError> errors,
            string sourceCode,
            string outputExtension,
            IEnumerable<ITemplateToken<TState>> sourceTokens
        )
        {
            Assembly = assembly;
            Template = template;
            Errors = errors;
            SourceCode = sourceCode;
            OutputExtension = outputExtension;
            SourceTokens = sourceTokens.ToArray();
        }

        public Assembly Assembly { get; }
        public object Template { get; }
        public IEnumerable<CompilerError> Errors { get; }
        public IEnumerable<ITemplateToken<TState>> SourceTokens { get; }
        public string SourceCode { get; }
        public string OutputExtension { get; }
    }
}
