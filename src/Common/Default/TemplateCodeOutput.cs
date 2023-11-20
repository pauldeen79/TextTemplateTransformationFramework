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
                                  CodeGeneratorResult codeGeneratorResult,
                                  string outputExtension,
                                  IEnumerable<string> referencedAssemblies,
                                  IEnumerable<string> packageReferences,
                                  string className,
                                  string tempPath)
        {
            if (codeGeneratorResult is null)
            {
                throw new ArgumentNullException(nameof(codeGeneratorResult));
            }
            SourceTokens = sourceTokens?.ToArray() ?? Array.Empty<ITemplateToken<TState>>();
            SourceCode = codeGeneratorResult.SourceCode;
            OutputExtension = outputExtension;
            Language = codeGeneratorResult.Language;
            ReferencedAssemblies = referencedAssemblies?.ToArray() ?? Array.Empty<string>();
            PackageReferences = packageReferences?.ToArray() ?? Array.Empty<string>();
            ClassName = className;
            TempPath = tempPath;
            Errors = codeGeneratorResult.Errors;
        }

        public TemplateCodeOutput(IEnumerable<ITemplateToken<TState>> sourceTokens,
                                  string sourceCode,
                                  TemplateCodeOutput<TState> previousResult)
            : this(sourceTokens,
                   new CodeGeneratorResult(sourceCode, previousResult?.Language ?? "C#", previousResult?.Errors ?? Enumerable.Empty<CompilerError>()),
                   previousResult?.OutputExtension,
                   previousResult?.ReferencedAssemblies,
                   previousResult?.PackageReferences,
                   previousResult?.ClassName,
                   previousResult?.TempPath)
        {
            if (previousResult is null)
            {
                throw new ArgumentNullException(nameof(previousResult));
            }
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
    }
}
