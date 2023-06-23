using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using TextTemplateTransformationFramework.Abstractions;
using TextTemplateTransformationFramework.Core.Extensions;

namespace TextTemplateTransformationFramework.Core
{
    public class MultipleContentBuilder : IMultipleContentBuilder
    {
        private readonly List<IContent> _contentList;

        public MultipleContentBuilder(string basePath = "")
        {
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

                if (content.SkipWhenFileExists && File.Exists(path))
                {
                    continue;
                }

                var dir = Path.GetDirectoryName(path);
                if (string.IsNullOrEmpty(dir))
                {
                    throw new InvalidOperationException("Directory could not be determined");
                }

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var contents = content.Builder.ToString().NormalizeLineEndings();
                Retry(() => File.WriteAllText(path, contents, Encoding.UTF8));
            }
        }

        public void SaveLastGeneratedFiles(string lastGeneratedFilesPath)
        {
            var fullPath = string.IsNullOrEmpty(BasePath) || Path.IsPathRooted(lastGeneratedFilesPath)
                ? lastGeneratedFilesPath
                : Path.Combine(BasePath, lastGeneratedFilesPath);

            var dir = Path.GetDirectoryName(fullPath);
            if (string.IsNullOrEmpty(dir))
            {
                throw new InvalidOperationException("Directory could not be determined");
            }

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (fullPath != null && !fullPath.Contains('*'))
            {
                File.WriteAllLines(fullPath, _contentList.OrderBy(c => c.FileName).Select(c => c.FileName));
            }
        }

        public void DeleteLastGeneratedFiles(string lastGeneratedFilesPath, bool recurse)
        {
            if (lastGeneratedFilesPath == null)
            {
                throw new ArgumentNullException(nameof(lastGeneratedFilesPath));
            }

            var basePath = BasePath;
            if (lastGeneratedFilesPath.Contains('\\'))
            {
                var lastSlash = lastGeneratedFilesPath.LastIndexOf("\\");

                basePath = $"{basePath}\\{lastGeneratedFilesPath.Substring(0, lastSlash)}";
                lastGeneratedFilesPath = lastGeneratedFilesPath.Substring(lastSlash + 1);
            }
            var fullPath = GetFullPath(lastGeneratedFilesPath, basePath);

            if (!File.Exists(fullPath))
            {
                if (fullPath?.Contains('*') == true
                    && !string.IsNullOrEmpty(basePath)
                    && Directory.Exists(basePath))
                {
                    foreach (var filename in GetFiles(basePath, lastGeneratedFilesPath, recurse))
                    {
                        File.Delete(filename);
                    }
                }
                // No previously generated files to delete
                return;
            }

            foreach (var fileName in File.ReadAllLines(fullPath))
            {
                var fileFullPath = string.IsNullOrEmpty(basePath) || Path.IsPathRooted(fileName)
                    ? fileName
                    : Path.Combine(basePath, fileName);

                if (File.Exists(fileFullPath))
                {
                    File.Delete(fileFullPath);
                }
            }
        }

        private static string[] GetFiles(string basePath, string lastGeneratedFilesPath, bool recurse)
        {
            try
            {
                return Directory.GetFiles(basePath, lastGeneratedFilesPath, GetSearchOption(recurse));
            }
            catch (DirectoryNotFoundException)
            {
                return Array.Empty<string>();
            }
        }

        private static string? GetFullPath(string lastGeneratedFilesPath, string basePath)
        {
            return string.IsNullOrEmpty(basePath) || Path.IsPathRooted(lastGeneratedFilesPath)
                ? lastGeneratedFilesPath
                : Path.Combine(basePath, lastGeneratedFilesPath);
        }

        public IContent AddContent(string fileName = "", bool skipWhenFileExists = false, StringBuilder? builder = null)
        {
            var content = builder == null
            ? new Content
            {
                FileName = fileName,
                SkipWhenFileExists = skipWhenFileExists
            }
            : new Content(builder)
            {
                FileName = fileName,
                SkipWhenFileExists = skipWhenFileExists
            };
            _contentList.Add(content);
            return content;
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

        public static MultipleContentBuilder FromString(string xml)
        {
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

            if (mc == null)
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

        public IEnumerable<IContent> Contents { get { return _contentList.AsReadOnly(); } }

        private static SearchOption GetSearchOption(bool recurse)
            => recurse
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;

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
    }
}
