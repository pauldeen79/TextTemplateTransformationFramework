namespace TextTemplateTransformationFramework.Abstractions
{
    public interface ITemplateFileManager : IMultipleContentBuilderContainer
    {
        StringBuilder StartNewFile(string fileName, bool skipWhenFileExists);
        void ResetToDefaultOutput();
        void Process(bool split, bool silentOutput);
        void SaveAll();
        void SaveLastGeneratedFiles(string lastGeneratedFilesPath);
        void DeleteLastGeneratedFiles(string lastGeneratedFilesPath, bool recurse);
    }
}
