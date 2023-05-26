namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface ITextTemplateProcessor
    {
        ExtractParametersResult ExtractParameters(TextTemplate textTemplate);
        ExtractParametersResult ExtractParameters(AssemblyTemplate assemblyTemplate);
        ProcessResult PreProcess(TextTemplate textTemplate, TemplateParameter[] parameters);
        ProcessResult Process(TextTemplate textTemplate, TemplateParameter[] parameters);
        ProcessResult Process(AssemblyTemplate assemblyTemplate, TemplateParameter[] parameters);
    }
}
