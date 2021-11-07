using System;

namespace TextTemplateTransformationFramework.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class DirectivePrefixAttribute : Attribute
    {
        public DirectivePrefixAttribute(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }
    }
}
