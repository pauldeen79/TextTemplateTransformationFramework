namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface ITextTemplateProcessor
    {
        ExtractParametersResult ExtractParameters(TextTemplate textTemplate);
        ProcessResult PreProcess(TextTemplate textTemplate, TemplateParameter[] parameters);
        ProcessResult Process(TextTemplate textTemplate, TemplateParameter[] parameters);
    }
}