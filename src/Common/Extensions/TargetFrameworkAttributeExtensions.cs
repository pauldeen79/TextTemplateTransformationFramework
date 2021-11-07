using System.Runtime.Versioning;

namespace TextTemplateTransformationFramework.Common.Extensions
{
    public static class TargetFrameworkAttributeExtensions
    {
        public static string GetFrameworkName(this TargetFrameworkAttribute instance)
            => instance?.FrameworkName ?? ".NETFramework,Version=v4.8";
    }
}
