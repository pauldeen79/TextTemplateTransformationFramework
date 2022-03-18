namespace TextTemplateTransformationFramework.Common.Cmd.Contracts
{
    public interface IUserInput
    {
        string GetValue(TemplateParameter parameter);
    }
}
