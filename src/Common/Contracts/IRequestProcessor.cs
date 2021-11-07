namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface IRequestProcessor<in TRequest, out TResponse>
    {
        TResponse Process(TRequest request);
    }
}
