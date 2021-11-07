using ScriptCompiler;
using TextTemplateTransformationFramework.Common.Default;

namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface ICodeCompiler<TState>
        where TState : class
    {
        CompilerResults Compile(ITextTemplateProcessorContext<TState> context, TemplateCodeOutput<TState> codeOutput);
    }
}
