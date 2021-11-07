using System.Collections.Generic;
using System.Text;
using TextTemplateTransformationFramework.Common;

namespace TextTemplateTransformationFramework.T4
{
    public sealed class CodeGeneratorResult
    {
        public StringBuilder SourceCodeStringBuilder { get; }
        public IEnumerable<CompilerError> Errors { get; }

        public CodeGeneratorResult(StringBuilder sourceCodeStringBuilder, IEnumerable<CompilerError> errors)
        {
            SourceCodeStringBuilder = sourceCodeStringBuilder;
            Errors = errors;
        }
    }
}
