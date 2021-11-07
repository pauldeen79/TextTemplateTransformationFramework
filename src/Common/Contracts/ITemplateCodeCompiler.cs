using TextTemplateTransformationFramework.Common.Default;

namespace TextTemplateTransformationFramework.Common.Contracts
{
    /// <summary>
    /// Compiles template code output into a template instance.
    /// </summary>
    public interface ITemplateCodeCompiler<TState>
        where TState : class
    {
        /// <summary>
        /// Compiles the specified template code output into a template instance.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="codeOutput">The template code output to compile.</param>
        /// <returns>Template compiler output that contains the template instance, or compiler errors.</returns>
        TemplateCompilerOutput<TState> Compile(ITextTemplateProcessorContext<TState> context, TemplateCodeOutput<TState> codeOutput);
    }
}
