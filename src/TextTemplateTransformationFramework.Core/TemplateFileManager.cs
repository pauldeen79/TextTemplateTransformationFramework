using System.Text;
using TextTemplateTransformationFramework.Abstractions;

namespace TextTemplateTransformationFramework.Core
{
    public class TemplateFileManager : ITemplateFileManager
    {
        private readonly StringBuilder _originalStringBuilder;

        //TODO: Review if we need a default c'tor, or refactor to use DI
        public TemplateFileManager(StringBuilder stringBuilder, string basePath = "")
            : this (new MultipleContentBuilder(basePath), stringBuilder)
        {
        }

        public TemplateFileManager(IMultipleContentBuilder multipleContentBuilder, StringBuilder stringBuilder)
        {
            MultipleContentBuilder = multipleContentBuilder;
            _originalStringBuilder = stringBuilder;
        }

        public IMultipleContentBuilder MultipleContentBuilder { get; }
        public StringBuilder GenerationEnvironment { get; private set; } = new();

        public StringBuilder StartNewFile(string fileName = "", bool skipWhenFileExists = false)
        {
            var currentContent = MultipleContentBuilder.AddContent(fileName, skipWhenFileExists, new StringBuilder());
            GenerationEnvironment = currentContent.Builder;
            return currentContent.Builder;
        }

        public void ResetToDefaultOutput() => GenerationEnvironment = _originalStringBuilder;

        public void Process(bool split = true, bool silentOutput = false)
        {
            ResetToDefaultOutput();

            if (split)
            {
                _originalStringBuilder.Clear();
                if (!silentOutput)
                {
                    _originalStringBuilder.Append(MultipleContentBuilder.ToString());
                }
            }
            else if (!silentOutput)
            {
                foreach (var item in MultipleContentBuilder.Contents)
                {
                    _originalStringBuilder.Append(item.Builder);
                }
            }
        }

        public void SaveAll()
            => MultipleContentBuilder.SaveAll();

        public void SaveLastGeneratedFiles(string lastGeneratedFilesPath)
            => MultipleContentBuilder.SaveLastGeneratedFiles(lastGeneratedFilesPath);

        public void DeleteLastGeneratedFiles(string lastGeneratedFilesPath, bool recurse = true)
            => MultipleContentBuilder.DeleteLastGeneratedFiles(lastGeneratedFilesPath, recurse);
    }
}
