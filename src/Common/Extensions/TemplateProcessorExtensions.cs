using System;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Extensions
{
    public static class TemplateProcessorExtensions
    {
        public static ProcessResult PreProcess(this ITextTemplateProcessor instance, TextTemplate textTemplate) => instance.PreProcess(textTemplate, Array.Empty<TemplateParameter>());
        public static ProcessResult Process(this ITextTemplateProcessor instance, TextTemplate textTemplate) => instance.Process(textTemplate, Array.Empty<TemplateParameter>());
        public static ExtractParametersResult ExtractParameters(this ITextTemplateProcessor instance, string assemblyName, string className, string usePath) => instance.ExtractParameters(new AssemblyTemplate(assemblyName, className, usePath));
    }
}
