using System;
using System.Text.RegularExpressions;

namespace TextTemplateTransformationFramework.Runtime.Extensions
{
    public static class StringExtensions
    {
        public static string NormalizeLineEndings(this string instance)
            => Regex.Replace(instance, @"\r\n|\n\r|\n|\r", Environment.NewLine, RegexOptions.None, TimeSpan.FromMilliseconds(200));
    }
}
