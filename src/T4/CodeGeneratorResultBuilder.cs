using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextTemplateTransformationFramework.Common;

namespace TextTemplateTransformationFramework.T4
{
    public sealed class CodeGeneratorResultBuilder
    {
        public StringBuilder SourceCodeStringBuilder { get; }
        public string Language { get; set;  }
        public List<CompilerError> Errors { get; }

        public CodeGeneratorResultBuilder(StringBuilder sourceCodeStringBuilder, IEnumerable<CompilerError> errors)
        {
            SourceCodeStringBuilder = sourceCodeStringBuilder;
            Errors = new List<CompilerError>(errors ?? Enumerable.Empty<CompilerError>());
        }

        public CodeGeneratorResult Build()
            => new CodeGeneratorResult(SourceCodeStringBuilder.ToString(), Language, Errors);

        public CodeGeneratorResultBuilder WithLanguage(string codeDomLanguage)
        {
            Language = codeDomLanguage;
            return this;
        }
    }
}
