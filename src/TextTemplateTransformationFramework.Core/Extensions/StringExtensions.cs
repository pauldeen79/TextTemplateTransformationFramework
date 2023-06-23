using System.Text.RegularExpressions;

namespace TextTemplateTransformationFramework.Core.Extensions
{
    public static class StringExtensions
    {
        public static string NormalizeLineEndings(this string instance, TimeSpan? matchTimeout = null)
            => Regex.Replace(instance, @"\r\n|\n\r|\n|\r", Environment.NewLine, RegexOptions.None, matchTimeout ?? TimeSpan.FromMilliseconds(200));
    }
}
