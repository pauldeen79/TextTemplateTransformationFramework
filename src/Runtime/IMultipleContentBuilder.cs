using System.Collections.Generic;
using System.Text;

namespace TextTemplateTransformationFramework.Runtime
{
    public interface IMultipleContentBuilder
    {
        string BasePath { get; set; }
        IEnumerable<Content> Contents { get; }
        void SaveAll();
        void SaveLastGeneratedFiles(string lastGeneratedFilesPath);
        void DeleteLastGeneratedFiles(string lastGeneratedFilesPath, bool recurse);
        Content AddContent(string fileName = null, bool skipWhenFileExists = false, StringBuilder builder = null);
    }
}
