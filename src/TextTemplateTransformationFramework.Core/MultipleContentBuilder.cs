namespace TextTemplateTransformationFramework.Core;

public class MultipleContentBuilder : IMultipleContentBuilder
{
    private readonly IFileSystem _fileSystem;
    private readonly Encoding _encoding;
    private readonly List<IContent> _contentList;

    public MultipleContentBuilder(string basePath = "") : this(new FileSystem(), Encoding.UTF8, basePath)
    {
    }

    public MultipleContentBuilder(Encoding encoding, string basePath = "") : this(new FileSystem(), encoding, basePath)
    {
    }

    internal MultipleContentBuilder(IFileSystem fileSystem, Encoding encoding, string basePath = "")
    {
        Guard.IsNotNull(fileSystem);
        Guard.IsNotNull(encoding);
        Guard.IsNotNull(basePath);
        
        _fileSystem = fileSystem;
        _encoding = encoding;
        _contentList = new List<IContent>();
        BasePath = basePath;
    }

    public string BasePath { get; set; }

    public void SaveAll()
    {
        foreach (var content in _contentList)
        {
            var path = string.IsNullOrEmpty(BasePath) || Path.IsPathRooted(content.FileName)
                ? content.FileName
                : Path.Combine(BasePath, content.FileName);

            if (string.IsNullOrEmpty(path))
            {
                throw new InvalidOperationException("Path could not be determined");
            }

            if (content.SkipWhenFileExists && _fileSystem.FileExists(path))
            {
                continue;
            }

            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !_fileSystem.DirectoryExists(dir))
            {
                _fileSystem.CreateDirectory(dir);
            }

            var contents = content.Builder.ToString().NormalizeLineEndings();
            Retry(() => _fileSystem.WriteAllText(path, contents, _encoding));
        }
    }

    public void SaveLastGeneratedFiles(string lastGeneratedFilesPath)
    {
        var fullPath = string.IsNullOrEmpty(BasePath) || Path.IsPathRooted(lastGeneratedFilesPath)
            ? lastGeneratedFilesPath
            : Path.Combine(BasePath, lastGeneratedFilesPath);

        if (string.IsNullOrEmpty(fullPath))
        {
            throw new InvalidOperationException("Full path could not be determined");
        }

        var dir = Path.GetDirectoryName(fullPath);
        if (string.IsNullOrEmpty(dir))
        {
            throw new InvalidOperationException("Directory could not be determined");
        }

        if (!_fileSystem.DirectoryExists(dir))
        {
            _fileSystem.CreateDirectory(dir);
        }

        if (!fullPath.Contains('*'))
        {
            _fileSystem.WriteAllLines(fullPath, _contentList.OrderBy(c => c.FileName).Select(c => c.FileName), _encoding);
        }
    }

    public void DeleteLastGeneratedFiles(string lastGeneratedFilesPath, bool recurse)
    {
        Guard.IsNotNull(lastGeneratedFilesPath);

        var basePath = BasePath;
        if (lastGeneratedFilesPath.Contains('\\'))
        {
            var lastSlash = lastGeneratedFilesPath.LastIndexOf("\\");

            basePath = $"{basePath}\\{lastGeneratedFilesPath.Substring(0, lastSlash)}";
            lastGeneratedFilesPath = lastGeneratedFilesPath.Substring(lastSlash + 1);
        }
        var fullPath = GetFullPath(lastGeneratedFilesPath, basePath);

        if (!_fileSystem.FileExists(fullPath))
        {
            if (fullPath.Contains('*')
                && !string.IsNullOrEmpty(basePath)
                && _fileSystem.DirectoryExists(basePath))
            {
                foreach (var filename in GetFiles(basePath, lastGeneratedFilesPath, recurse))
                {
                    _fileSystem.FileDelete(filename);
                }
            }
            // No previously generated files to delete
            return;
        }

        foreach (var fileName in _fileSystem.ReadAllLines(fullPath, _encoding))
        {
            var fileFullPath = string.IsNullOrEmpty(basePath) || Path.IsPathRooted(fileName)
                ? fileName
                : Path.Combine(basePath, fileName);

            if (_fileSystem.FileExists(fileFullPath))
            {
                _fileSystem.FileDelete(fileFullPath);
            }
        }
    }

    public IContent AddContent(string fileName = "", bool skipWhenFileExists = false, StringBuilder? builder = null)
    {
        Guard.IsNotNull(fileName);

        var content = builder is null
            ? new Content()
            : new Content(builder);

        content.FileName = fileName;
        content.SkipWhenFileExists = skipWhenFileExists;

        _contentList.Add(content);

        return content;
    }

    public IEnumerable<IContent> Contents => _contentList.AsReadOnly();

    public static MultipleContentBuilder FromString(string xml)
    {
        Guard.IsNotNull(xml);

        var result = new MultipleContentBuilder();

        MultipleContents? mc;

        // Cope with CA2202 analysis warning by using this unusual statement with try.finally instead of nested usings
        var stringReader = new StringReader(xml);
        try
        {
            using (var reader = XmlReader.Create(stringReader))
            {
                // Set reference to null because the reader will destroy it
                stringReader = null;

                var serializer = new DataContractSerializer(typeof(MultipleContents));
                mc = serializer.ReadObject(reader) as MultipleContents;
            }
        }
        finally
        {
            stringReader?.Dispose();
        }

        if (mc is null)
        {
            throw new InvalidOperationException("Xml could not be parsed to MultipleContents");
        }

        result.BasePath = mc.BasePath;
        foreach (var item in mc.Contents)
        {
            var c = result.AddContent(item.FileName, item.SkipWhenFileExists);
            foreach (var line in item.Lines)
            {
                c.Builder.AppendLine(line);
            }
        }

        return result;
    }

    public override string ToString()
    {
        var mc = new MultipleContents
        {
            BasePath = BasePath,
            Contents = _contentList.Select(c => new Contents
            {
                FileName = c.FileName,
                Lines = c.Builder.ToString().NormalizeLineEndings().Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList(),
                SkipWhenFileExists = c.SkipWhenFileExists
            }).ToList()
        };

        var serializer = new DataContractSerializer(typeof(MultipleContents));
        var sb = new StringBuilder();

        using (var writer = XmlWriter.Create(sb, new XmlWriterSettings { Indent = true }))
        {
            serializer.WriteObject(writer, mc);
        }

        return sb.ToString();
    }

    private static void Retry(Action action)
    {
        for (int i = 1; i < 3; i++)
        {
            try
            {
                action();
                return;
            }
            catch (IOException x) when (x.Message.Contains("because it is being used by another process"))
            {
                Thread.Sleep(i * 500);
            }
        }
    }

    private string[] GetFiles(string basePath, string lastGeneratedFilesPath, bool recurse)
    {
        if (!_fileSystem.DirectoryExists(basePath))
        {
            return Array.Empty<string>();
        }

        return _fileSystem.GetFiles(basePath, lastGeneratedFilesPath, recurse);
    }

    private static string GetFullPath(string lastGeneratedFilesPath, string basePath)
        => string.IsNullOrEmpty(basePath) || Path.IsPathRooted(lastGeneratedFilesPath)
            ? lastGeneratedFilesPath
            : Path.Combine(basePath, lastGeneratedFilesPath);
}
