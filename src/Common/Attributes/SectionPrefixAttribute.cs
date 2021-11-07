using System;

namespace TextTemplateTransformationFramework.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class SectionPrefixAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SectionPrefixAttribute"/> class.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        public SectionPrefixAttribute(string prefix)
        {
            Prefix = prefix ?? throw new ArgumentNullException(nameof(prefix));
        }

        /// <summary>
        /// Gets the prefix.
        /// </summary>
        /// <value>
        /// The prefix.
        /// </value>
        public string Prefix { get; }
    }
}
