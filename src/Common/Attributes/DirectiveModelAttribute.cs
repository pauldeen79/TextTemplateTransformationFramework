using System;

namespace TextTemplateTransformationFramework.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class DirectiveModelAttribute : Attribute
    {
        public DirectiveModelAttribute(Type type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public Type Type { get; }
    }
}
