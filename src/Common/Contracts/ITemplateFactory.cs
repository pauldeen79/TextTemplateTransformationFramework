using ScriptCompiler;
using TextTemplateTransformationFramework.Common.Default;

namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface ITemplateFactory<TState>
        where TState : class
    {
        object Create(ITextTemplateProcessorContext<TState> context, TemplateCodeOutput<TState> codeOutput, CompilerResults result);
    }
}
