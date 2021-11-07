using System.Collections.Generic;
using TextTemplateTransformationFramework.Common;

namespace TextTemplateTransformationFramework.T4.Contracts
{
    public interface ITokenArgumentParser<T>
        where T : class
    {
        IEnumerable<string> ParseArgument(SectionContext<T> context, string argumentName);
    }
}
