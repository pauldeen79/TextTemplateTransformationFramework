using System.Collections.Generic;

namespace TextTemplateTransformationFramework.Common.Contracts
{
    /// <summary>
    /// Allows to communicate with the tokenparser from parsing components such as section context.
    /// </summary>
    public interface ITokenParserCallback<TState> : IDictionary<string, object>
        where TState : class
    {
        /// <summary>
        /// Gets the section arguments from the specified section context by argument name.
        /// </summary>
        /// <param name="context">The section context to get the section argument from.</param>
        /// <param name="name">The name of the argument to get.</param>
        /// <returns>The values of the section argument.</returns>
        IEnumerable<string> GetSectionArguments(SectionContext<TState> context, string name);

        /// <summary>
        /// Parses a (sub) template.
        /// </summary>
        /// <remarks>Can be used to include a text template from another file or other external resource.</remarks>
        /// <param name="context">The context that holds text template contents to parse.</param>
        /// <returns>The template tokens that are produced (parsed) from the specified text template.</returns>
        IEnumerable<ITemplateToken<TState>> Parse(ITextTemplateProcessorContext<TState> context);

        /// <summary>
        /// Determines whether the token parser is processing a child template.
        /// </summary>
        bool IsChildTemplate { get; }

        /// <summary>
        /// Determines whether the section context is currently processing a directive with the specified name.
        /// </summary>
        /// <param name="context">The current section context.</param>
        /// <param name="name">The directive name.</param>
        /// <returns>true when the section context is currently processing a directive with the specified name, otherwise false.</returns>
        bool SectionIsDirectiveWithName(SectionContext<TState> context, string name);

        /// <summary>
        /// Determines whether the section context is currently processing a section that starts with the specified prefix.
        /// </summary>
        /// <param name="context">The current section context.</param>
        /// <param name="prefix">The prefix of the section.</param>
        /// <returns>true when the section context is currently processing a section that starts with the specified prefix, otherwise false.</returns>
        bool SectionStartsWithPrefix(SectionContext<TState> context, string prefix);
    }
}
