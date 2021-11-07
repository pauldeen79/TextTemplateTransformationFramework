using System;
using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.Common.Extensions
{
    public static class TemplateProcessorExtensions
    {
        public static ProcessResult PreProcess(this ITextTemplateProcessor instance, TextTemplate textTemplate) => instance.PreProcess(textTemplate, Array.Empty<TemplateParameter>());
        public static ProcessResult PreProcess(this ITextTemplateProcessor instance, string textTemplateText, string filename = "unknown.tt") => instance.PreProcess(new TextTemplate(textTemplateText, filename), Array.Empty<TemplateParameter>());
        public static ProcessResult PreProcess(this ITextTemplateProcessor instance, string textTemplateText, TemplateParameter[] parameters, string filename = "unknown.tt") => instance.PreProcess(new TextTemplate(textTemplateText, filename), parameters);
        public static ProcessResult Process(this ITextTemplateProcessor instance, TextTemplate textTemplate) => instance.Process(textTemplate, Array.Empty<TemplateParameter>());
        public static ProcessResult Process(this ITextTemplateProcessor instance, string textTemplateText, string filename = "unknown.tt") => instance.Process(new TextTemplate(textTemplateText, filename), Array.Empty<TemplateParameter>());
        public static ProcessResult Process(this ITextTemplateProcessor instance, string textTemplateText, TemplateParameter[] parameters, string filename = "unknown.tt") => instance.Process(new TextTemplate(textTemplateText, filename), parameters);
        public static ExtractParametersResult ExtractParameters(this ITextTemplateProcessor instance, string textTemplateText, string filename = "unknown.tt") => instance.ExtractParameters(new TextTemplate(textTemplateText, filename));
    }
}
