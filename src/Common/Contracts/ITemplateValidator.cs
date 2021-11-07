namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface ITemplateValidator
    {
        ProcessResult Validate(object template);
    }
}
