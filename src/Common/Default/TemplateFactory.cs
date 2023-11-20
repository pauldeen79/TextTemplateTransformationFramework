using ScriptCompiler;
using System;
using System.Linq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Extensions;
using Utilities.Extensions;

namespace TextTemplateTransformationFramework.Common.Default
{
    public class TemplateFactory<TState> : ITemplateFactory<TState>
        where TState : class
    {
        public object Create(ITextTemplateProcessorContext<TState> context, TemplateCodeOutput<TState> codeOutput, CompilerResults result)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (codeOutput == null)
            {
                throw new ArgumentNullException(nameof(codeOutput));
            }

            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            return result.CompiledAssembly.CreateInstance(codeOutput.ClassName)
                ?? Activator.CreateInstance(GetType(codeOutput, result));
        }

        private static Type GetType(TemplateCodeOutput<TState> codeOutput, CompilerResults result)
            => result.CompiledAssembly.GetExportedTypes()
                .First(t => t.FullName.WithoutGenerics() == codeOutput.ClassName)
                .GetModelType(typeof(TState));
    }
}
