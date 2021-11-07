using System.Collections.Generic;

namespace TextTemplateTransformationFramework.Common.Contracts
{
    /// <summary>
    /// Translates a text template into template tokens.
    /// </summary>
    public interface ITextTemplateTokenParser<TState>
        where TState : class
    {
        /// <summary>
        /// Parse the specified text template into template tokens.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Parsed tokens.</returns>
        IEnumerable<ITemplateToken<TState>> Parse(ITextTemplateProcessorContext<TState> context);
    }
}
