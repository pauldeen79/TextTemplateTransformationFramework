namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface IFileNameProviderContainer
    {
        IFileNameProvider FileNameProvider { get; set; }
    }
}
