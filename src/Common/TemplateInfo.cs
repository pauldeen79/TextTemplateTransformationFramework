using System;

namespace TextTemplateTransformationFramework.Common
{
    public record TemplateInfo
    {
        public string ShortName { get; }
        public string FileName { get; }
        public string AssemblyName { get; }
        public string ClassName { get; }
        public TemplateType Type { get; }
        public TemplateParameter[] Parameters { get; }

        public TemplateInfo(string shortName, string fileName, string assemblyName, string className, TemplateType type, TemplateParameter[] parameters)
        {
            ShortName = shortName ?? throw new ArgumentNullException(nameof(shortName));
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            AssemblyName = assemblyName ?? throw new ArgumentNullException(nameof(assemblyName));
            ClassName = className ?? throw new ArgumentNullException(nameof(className));
            Type = type;
            Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        }

        public static TemplateInfo Text(string shortName, string fileName, TemplateParameter[] parameters)
            => new TemplateInfo(shortName, fileName, null, null, TemplateType.TextTemplate, parameters);

        public static TemplateInfo Assembly(string shortName, string assemblyName, string className, TemplateParameter[] parameters)
            => new TemplateInfo(shortName, null, assemblyName, className, TemplateType.AssemblyTemplate, parameters);
    }
}
