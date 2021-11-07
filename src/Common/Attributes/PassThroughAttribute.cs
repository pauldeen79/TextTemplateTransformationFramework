using System;

namespace TextTemplateTransformationFramework.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class PassThroughAttribute : Attribute
    {
    }
}
