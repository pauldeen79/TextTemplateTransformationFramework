using System.Collections.Generic;
using TextTemplateTransformationFramework.Common.Default;

namespace TextTemplateTransformationFramework.Common.Contracts
{
    /// <summary>
    /// Translates template tokens into template code output.
    /// </summary>
    public interface ITokenProcessor<TState>
        where TState : class
    {
        /// <summary>
        /// Process the specified tokens, and produce compiled template code output.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="tokens">The template tokens to process.</param>
        /// <returns>Compiled template code output.</returns>
        TemplateCodeOutput<TState> Process(ITextTemplateProcessorContext<TState> context, IEnumerable<ITemplateToken<TState>> tokens);
    }
}
