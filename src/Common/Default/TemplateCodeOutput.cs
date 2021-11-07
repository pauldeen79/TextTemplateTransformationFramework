using System;
using System.Collections.Generic;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Default
{
    public class TemplateCodeOutput<TState>
        where TState : class
    {
        public TemplateCodeOutput(IEnumerable<ITemplateToken<TState>> sourceTokens,
                                  string sourceCode,
                                  string outputExtension,
                                  string language,
                                  IEnumerable<string> referencedAssemblies,
                                  IEnumerable<string> packageReferences,
                                  string className,
                                  string tempPath,
                                  IEnumerable<CompilerError> errors,
                                  Exception exception = null)
        {
            SourceTokens = sourceTokens?.ToArray() ?? Array.Empty<ITemplateToken<TState>>();
            SourceCode = sourceCode;
            OutputExtension = outputExtension;
            Language = language;
            ReferencedAssemblies = referencedAssemblies?.ToArray() ?? Array.Empty<string>();
            PackageReferences = packageReferences?.ToArray() ?? Array.Empty<string>();
            ClassName = className;
            TempPath = tempPath;
            Errors = errors?.ToArray() ?? Array.Empty<CompilerError>();
            Exception = exception;
        }

        public TemplateCodeOutput(IEnumerable<ITemplateToken<TState>> sourceTokens,
                                  string sourceCode,
                                  TemplateCodeOutput<TState> previousResult)
            : this(sourceTokens,
                   sourceCode,
                   previousResult?.OutputExtension,
                   previousResult?.Language,
                   previousResult?.ReferencedAssemblies,
                   previousResult?.PackageReferences,
                   previousResult?.ClassName,
                   previousResult?.TempPath,
                   previousResult?.Errors,
                   previousResult?.Exception)
        {
        }

        public string Language { get; }

        public string SourceCode { get; }

        public string OutputExtension { get; }

        public IEnumerable<string> ReferencedAssemblies { get; }

        public IEnumerable<string> PackageReferences { get; }

        public IEnumerable<ITemplateToken<TState>> SourceTokens { get; }

        public string ClassName { get; }

        public string TempPath { get; }

        public IEnumerable<CompilerError> Errors { get; }

        public Exception Exception { get; }
    }
}
