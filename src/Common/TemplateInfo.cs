﻿using System;

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

        public TemplateInfo Update(string shortName, TemplateParameter[] parameters)
            => new TemplateInfo(shortName, FileName, AssemblyName, ClassName, Type, parameters);
    }
}
