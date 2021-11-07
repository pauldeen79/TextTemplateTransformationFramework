using System;

namespace TextTemplateTransformationFramework.Common.Attributes
{
    /// <summary>
    /// Allows multiple token mappers within the same assembly to process as one group, allowing to either enable or disable pass-through.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class GroupedTokenMapperAttribute : Attribute
    {
        public GroupedTokenMapperAttribute(Type type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public Type Type { get; }
    }
}
