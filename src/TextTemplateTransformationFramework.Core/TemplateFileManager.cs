namespace TextTemplateTransformationFramework.Core;

public class TemplateFileManager : ITemplateFileManager
{
    private readonly IIndentedStringBuilder _originalStringBuilder;

    public TemplateFileManager(IIndentedStringBuilder stringBuilder, string basePath = "")
        : this (new MultipleContentBuilder(basePath), stringBuilder)
    {
    }

    internal TemplateFileManager(IMultipleContentBuilder multipleContentBuilder, IIndentedStringBuilder stringBuilder)
    {
        Guard.IsNotNull(stringBuilder);

        MultipleContentBuilder = multipleContentBuilder;
        _originalStringBuilder = stringBuilder;
    }

    public IMultipleContentBuilder MultipleContentBuilder { get; }
    public IIndentedStringBuilder GenerationEnvironment { get; private set; } = new IndentedStringBuilder();

    public IIndentedStringBuilder StartNewFile(string fileName = "", bool skipWhenFileExists = false)
    {
        Guard.IsNotNull(fileName);

        var currentContent = MultipleContentBuilder.AddContent(fileName, skipWhenFileExists, new IndentedStringBuilder());
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
                _originalStringBuilder.Append(MultipleContentBuilder.ToString()!);
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
        Guard.IsNotNullOrWhiteSpace(lastGeneratedFilesPath);
        MultipleContentBuilder.SaveLastGeneratedFiles(lastGeneratedFilesPath);
    }

    public void DeleteLastGeneratedFiles(string lastGeneratedFilesPath, bool recurse = true)
    {
        Guard.IsNotNullOrWhiteSpace(lastGeneratedFilesPath);
        MultipleContentBuilder.DeleteLastGeneratedFiles(lastGeneratedFilesPath, recurse);
    }
}
