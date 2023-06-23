using System.Text;

namespace TextTemplateTransformationFramework.Abstractions
{
    public interface ITemplateFileManager
    {
        IMultipleContentBuilder MultipleContentBuilder { get; }
        StringBuilder StartNewFile(string fileName = "", bool skipWhenFileExists = false);
        void ResetToDefaultOutput();
        void Process(bool split = true, bool silentOutput = false);
        void SaveAll();
        void SaveLastGeneratedFiles(string lastGeneratedFilesPath);
        void DeleteLastGeneratedFiles(string lastGeneratedFilesPath, bool recurse = true);
    }
}
