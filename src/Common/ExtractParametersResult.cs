using System;
using System.Collections.Generic;
using System.Linq;

namespace TextTemplateTransformationFramework.Common
{
    [Serializable]
    public sealed class ExtractParametersResult
    {
        private ExtractParametersResult
        (
            TemplateParameter[] parameters,
            CompilerError[] compilerErrors,
            string sourceCode,
            string diagnosticDump,
            string exception
        )
        {
            Parameters = parameters;
            CompilerErrors = compilerErrors;
            SourceCode = sourceCode;
            DiagnosticDump = diagnosticDump;
            Exception = exception;
        }

        public TemplateParameter[] Parameters { get; }
        public string SourceCode { get; }
        public string DiagnosticDump { get; }
        public CompilerError[] CompilerErrors { get; }
        public string Exception { get; }

        public static ExtractParametersResult Create
        (
            IEnumerable<TemplateParameter> parameters,
            CompilerError[] compilerErrors,
            string sourceCode,
            string diagnosticDump,
            Exception exception = null
        ) => new ExtractParametersResult(parameters.ToArray(),
                                         compilerErrors,
                                         sourceCode,
                                         diagnosticDump,
                                         exception?.ToString());
    }
}
