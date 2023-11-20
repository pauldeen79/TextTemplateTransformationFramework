using ScriptCompiler;
using System;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;

namespace TextTemplateTransformationFramework.T4.Plus
{
    public class TemplateFactory<TState> : ITemplateFactory<TState>
        where TState : class
    {
        private readonly ITemplateFactory<TState> _baseFactory;

        public TemplateFactory(ITemplateFactory<TState> baseFactory)
        {
            _baseFactory = baseFactory ?? throw new ArgumentNullException(nameof(baseFactory));
        }

        public object Create(ITextTemplateProcessorContext<TState> context,
                             TemplateCodeOutput<TState> codeOutput,
                             CompilerResults result)
        {
            if (codeOutput is null)
            {
                throw new ArgumentNullException(nameof(codeOutput));
            }

            if (result is null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            var compositionRoot = result.CompiledAssembly.CreateInstance(codeOutput.ClassName + "CompositionRoot");

            return compositionRoot is null
                ? _baseFactory.Create(context, codeOutput, result)
                : compositionRoot.GetType().GetMethod("ResolveTemplate").Invoke(compositionRoot, Array.Empty<object>());
        }
    }
}
