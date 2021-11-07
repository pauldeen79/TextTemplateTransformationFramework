using System.Collections.Generic;

namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface IFileNameProvider
    {
        IEnumerable<string> GetFiles(string path, string searchPattern, bool recurse);
    }
}
