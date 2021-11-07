using System.Collections.Generic;
using System.Globalization;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4
{
    public class ArgumentParser : IArgumentParser
    {
        public IEnumerable<string> Parse(string section, string name)
        {
            if (section != null && name != null)
            {
                var continueAt = -1;
                var @break = false;
                while (!@break)
                {
                    var start = section.ToLower(CultureInfo.InvariantCulture).IndexOf(" " + name.ToLower(CultureInfo.InvariantCulture) + "=\"", continueAt + 1);
                    if (start == -1)
                    {
                        break;
                    }
                    continueAt = -1;
                    while (true)
                    {
                        var end = section.IndexOf("\"", continueAt > -1
                            ? continueAt
                            : start + name.Length + 3);

                        if (end > 0 && section.Substring(end - 1, 1) == "\\" && end + 1 < section.TrimEnd().Length)
                        {
                            continueAt = end + 1;
                            continue;
                        }

                        if (end == -1)
                        {
                            @break = true;
                            break;
                        }

                        var value = section
                            .Substring(start + name.Length + 3, end - start - name.Length - 3)
                            .Replace("\\\"", "\"");

                        yield return value;
                        continueAt = end;
                        break;
                    }
                }
            }
        }
    }
}
