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
        private static readonly JsonSerializerSettings _settings = new() { Converters = new[] { new StringEnumConverter() } };

        public void Add(TemplateInfo templateInfo)
        {
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
            if (!File.Exists(fileName))
            {
                return Enumerable.Empty<TemplateInfo>();
            }

            return JsonConvert.DeserializeObject<TemplateInfoConfig>(File.ReadAllText(fileName), _settings).Templates;
        }

        public void Remove(TemplateInfo templateInfo)
        {
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

        private static void Save(List<TemplateInfo> templates)
        {
            var directory = GetDirectory();
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.WriteAllText(GetFileName(), JsonConvert.SerializeObject(new TemplateInfoConfig { Templates = templates }, _settings));
        }
    }

    public class TemplateInfoConfig
    {
        public List<TemplateInfo> Templates { get; set; }
    }
}
