using System.Text;
using CommunityToolkit.Diagnostics;
using TextTemplateTransformationFramework.Abstractions;

namespace TextTemplateTransformationFramework.Core
{
    public class TemplateFileManager : ITemplateFileManager
    {
        private readonly StringBuilder _originalStringBuilder;

        public TemplateFileManager(StringBuilder stringBuilder, string basePath = "")
            : this (new MultipleContentBuilder(basePath), stringBuilder)
        {
        }

        public TemplateFileManager(IMultipleContentBuilder multipleContentBuilder, StringBuilder stringBuilder)
        {
            Guard.IsNotNull(multipleContentBuilder);
            Guard.IsNotNull(stringBuilder);

            MultipleContentBuilder = multipleContentBuilder;
            _originalStringBuilder = stringBuilder;
        }

        public IMultipleContentBuilder MultipleContentBuilder { get; }
        public StringBuilder GenerationEnvironment { get; private set; } = new();

        public StringBuilder StartNewFile(string fileName = "", bool skipWhenFileExists = false)
        {
            Guard.IsNotNull(fileName);

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

        public void SaveAll() => MultipleContentBuilder.SaveAll();

        public void SaveLastGeneratedFiles(string lastGeneratedFilesPath)
        {
            Guard.IsNotNull(lastGeneratedFilesPath);
            MultipleContentBuilder.SaveLastGeneratedFiles(lastGeneratedFilesPath);
        }

        public void DeleteLastGeneratedFiles(string lastGeneratedFilesPath, bool recurse = true)
        {
            Guard.IsNotNull(lastGeneratedFilesPath);
            MultipleContentBuilder.DeleteLastGeneratedFiles(lastGeneratedFilesPath, recurse);
        }
    }
}
