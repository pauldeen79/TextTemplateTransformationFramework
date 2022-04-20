using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using TextTemplateTransformationFramework.Runtime.Extensions;

namespace TextTemplateTransformationFramework.Runtime
{
    public class MultipleContentBuilder : IMultipleContentBuilder
    {
        private readonly List<Content> _contentList;

        public MultipleContentBuilder(string basePath = null)
        {
            _contentList = new List<Content>();
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

                var dir = Path.GetDirectoryName(path);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var contents = content.Builder.ToString().NormalizeLineEndings();
                File.WriteAllText(path, contents, Encoding.UTF8);
            }
        }

        public void SaveLastGeneratedFiles(string lastGeneratedFilesPath)
        {
            var fullPath = string.IsNullOrEmpty(BasePath) || Path.IsPathRooted(lastGeneratedFilesPath)
                ? lastGeneratedFilesPath
                : Path.Combine(BasePath, lastGeneratedFilesPath);

            var dir = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (fullPath != null && !fullPath.Contains("*"))
            {
                File.WriteAllLines(fullPath, _contentList.OrderBy(c => c.FileName).Select(c => c.FileName));
            }
        }

        public void DeleteLastGeneratedFiles(string lastGeneratedFilesPath, bool recurse)
        {
            var basePath = BasePath;
            if (lastGeneratedFilesPath?.Contains("\\") == true)
            {
                var lastSlash = lastGeneratedFilesPath.LastIndexOf("\\");

                basePath = basePath + "\\" + lastGeneratedFilesPath.Substring(0, lastSlash);
                lastGeneratedFilesPath = lastGeneratedFilesPath.Substring(lastSlash + 1);
            }
            var fullPath = GetFullPath(lastGeneratedFilesPath, basePath);

            if (!File.Exists(fullPath))
            {
                if (fullPath?.Contains("*") == true
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

        private static string GetFullPath(string lastGeneratedFilesPath, string basePath)
        {
            return string.IsNullOrEmpty(basePath) || Path.IsPathRooted(lastGeneratedFilesPath)
                            ? lastGeneratedFilesPath
                            : Path.Combine(basePath, lastGeneratedFilesPath);
        }

        public Content AddContent(string fileName = null, bool skipWhenFileExists = false, StringBuilder builder = null)
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

            MultipleContents mc;

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

        public IEnumerable<Content> Contents { get { return _contentList.AsReadOnly(); } }

        private static SearchOption GetSearchOption(bool recurse)
            => recurse
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;
    }
}
