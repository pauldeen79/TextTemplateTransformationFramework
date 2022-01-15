using System;
using System.Collections.Generic;

namespace TextTemplateTransformationFramework.Common
{
    public class CodeGeneratorResult
    {
        public CodeGeneratorResult(string sourceCode, string language, IEnumerable<CompilerError> errors)
        {
            SourceCode = sourceCode ?? throw new ArgumentNullException(nameof(sourceCode));
            Language = language ?? throw new ArgumentNullException(nameof(language));
            Errors = errors ?? throw new ArgumentNullException(nameof(errors));
        }

        public string SourceCode { get; }
        public string Language { get; }
        public IEnumerable<CompilerError> Errors { get; }
    }
}
