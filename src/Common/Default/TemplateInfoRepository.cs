using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Default
{
    public class TemplateInfoRepository : ITemplateInfoRepository
    {
        private static readonly JsonSerializerSettings _settings = new()
        {
            Converters = new[] { new StringEnumConverter() },
            TypeNameHandling = TypeNameHandling.Auto
        };
        
        private readonly IFileContentsProvider _fileContentsProvider;

        public TemplateInfoRepository(IFileContentsProvider fileContentsProvider)
        {
            _fileContentsProvider = fileContentsProvider ?? throw new ArgumentNullException(nameof(fileContentsProvider));
        }

        public void Add(TemplateInfo templateInfo)
        {
            if (templateInfo == null)
            {
                throw new ArgumentNullException(nameof(templateInfo));
            }

            var templates = GetTemplates().ToList();

            var existingTemplate = FindTemplate(templateInfo, templates);
            if (existingTemplate != null)
            {
                throw new ArgumentOutOfRangeException(nameof(templateInfo), "Template already exists");
            }

            templates.Add(templateInfo);
            Save(templates);
        }

        public IEnumerable<TemplateInfo> GetTemplates()
        {
            var fileName = GetFileName();
            if (!_fileContentsProvider.FileExists(fileName))
            {
                return Enumerable.Empty<TemplateInfo>();
            }

            return JsonConvert.DeserializeObject<TemplateInfoConfig>(_fileContentsProvider.GetFileContents(fileName), _settings).Templates;
        }

        public void Remove(TemplateInfo templateInfo)
        {
            if (templateInfo == null)
            {
                throw new ArgumentNullException(nameof(templateInfo));
            }

            var templates = GetTemplates().ToList();
            var templateToRemove = FindTemplate(templateInfo, templates);
            if (templateToRemove == null)
            {
                throw new ArgumentOutOfRangeException(nameof(templateInfo), "Template was not found");
            }

            templates.Remove(templateToRemove);
            Save(templates);
        }

        private static string GetFileName() => Path.Combine(GetDirectory(), "templates.config");

        private static string GetDirectory() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "T4Plus");

        private static TemplateInfo FindTemplate(TemplateInfo templateInfo, List<TemplateInfo> templates)
            => templates.Find(x =>
            x.AssemblyName?.Equals(templateInfo.AssemblyName, StringComparison.OrdinalIgnoreCase) == true
            && x.ClassName?.Equals(templateInfo.ClassName, StringComparison.OrdinalIgnoreCase) == true
            && x.FileName?.Equals(templateInfo.FileName, StringComparison.OrdinalIgnoreCase) == true
            && x.Type == templateInfo.Type);

        private void Save(List<TemplateInfo> templates)
        {
            var directory = GetDirectory();
            if (!_fileContentsProvider.DirectoryExists(directory))
            {
                _fileContentsProvider.CreateDirectory(directory);
            }
            _fileContentsProvider.WriteFileContents(GetFileName(), JsonConvert.SerializeObject(new TemplateInfoConfig { Templates = templates }, _settings));
        }
    }

    public class TemplateInfoConfig
    {
        public List<TemplateInfo> Templates { get; set; }
    }
}
