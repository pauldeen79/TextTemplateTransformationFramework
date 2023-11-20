using System;
using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common.Extensions;

namespace TextTemplateTransformationFramework.Common
{
    public sealed class ProcessResult
    {
        private ProcessResult(CompilerError[] compilerErrors,
                              string output,
                              string sourceCode,
                              string diagnosticDump,
                              string outputExtension,
                              string exception)
        {
            CompilerErrors = compilerErrors;
            Output = output;
            SourceCode = sourceCode;
            DiagnosticDump = diagnosticDump;
            OutputExtension = outputExtension;
            Exception = exception;
        }

        public CompilerError[] CompilerErrors { get; }
        public string Output { get; }
        public string SourceCode { get; }
        public string DiagnosticDump { get; }
        public string OutputExtension { get; }
        public string Exception { get; }

        public override string ToString() =>
            !string.IsNullOrEmpty(Exception)
                ? Exception
                : Output;

        public static ProcessResult Create(IEnumerable<CompilerError> errors,
                                           string output = null,
                                           string sourceCode = null,
                                           string diagnosticDump = null,
                                           string outputExtension = null,
                                           Exception exception = null)
            => new ProcessResult
            (
                (errors ?? Array.Empty<CompilerError>())
                    .ToArray(),
                string.IsNullOrEmpty(output)
                    &&
                    (
                        (errors ?? Array.Empty<CompilerError>()).HasErrors()
                        || exception is not null
                    )
                        ? "ErrorGeneratingOutput"
                        : output,
                sourceCode,
                diagnosticDump,
                outputExtension,
                exception?.ToString()
            );
    }
}
