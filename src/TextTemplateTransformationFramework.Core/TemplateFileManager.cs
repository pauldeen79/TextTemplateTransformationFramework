using System.Text;
using TextTemplateTransformationFramework.Abstractions;

namespace TextTemplateTransformationFramework.Core
{
    public class TemplateFileManager : ITemplateFileManager
    {
        private readonly Action<StringBuilder> _setStringBuilderDelegate;
        private readonly StringBuilder _originalStringBuilder;

        public TemplateFileManager(Action<StringBuilder> setStringBuilderDelegate,
                                   Func<StringBuilder> getStringBuilderDelegate,
                                   string basePath = "",
        IMultipleContentBuilder? multipleContentBuilder = null)
        {
            if (setStringBuilderDelegate == null)
            {
                throw new ArgumentNullException(nameof(setStringBuilderDelegate));
            }

            if (getStringBuilderDelegate == null)
            {
                throw new ArgumentNullException(nameof(getStringBuilderDelegate));
            }

            _setStringBuilderDelegate = setStringBuilderDelegate;
            _originalStringBuilder = getStringBuilderDelegate();
            MultipleContentBuilder = multipleContentBuilder ?? new MultipleContentBuilder(basePath);
        }

        public IMultipleContentBuilder MultipleContentBuilder { get; }

        public StringBuilder StartNewFile(string fileName = "", bool skipWhenFileExists = false)
        {
            var currentContent = MultipleContentBuilder.AddContent(fileName, skipWhenFileExists, new StringBuilder());
            _setStringBuilderDelegate(currentContent.Builder);
            return currentContent.Builder;
        }

        public void ResetToDefaultOutput()
        {
            _setStringBuilderDelegate(_originalStringBuilder);
        }

        public void Process(bool split = true, bool silentOutput = false)
        {
            ResetToDefaultOutput();

            if (split)
            {
                _originalStringBuilder.Clear();
                if (!silentOutput) _originalStringBuilder.Append(MultipleContentBuilder.ToString());
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
        {
            MultipleContentBuilder.SaveAll();
        }

        public void SaveLastGeneratedFiles(string lastGeneratedFilesPath)
        {
            MultipleContentBuilder.SaveLastGeneratedFiles(lastGeneratedFilesPath);
        }

        public void DeleteLastGeneratedFiles(string lastGeneratedFilesPath, bool recurse = true)
        {
            MultipleContentBuilder.DeleteLastGeneratedFiles(lastGeneratedFilesPath, recurse);
        }
    }
}
