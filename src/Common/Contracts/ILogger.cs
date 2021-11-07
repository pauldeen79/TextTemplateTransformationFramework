namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface ILogger
    {
        void Log(string message);
        string Aggregate();
    }
}
