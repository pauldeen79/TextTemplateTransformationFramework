using System.Collections.Generic;

namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface IArgumentParser
    {
        IEnumerable<string> Parse(string section, string name);
    }
}
