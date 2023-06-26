namespace TextTemplateTransformationFramework.Abstractions;

public interface ITemplateContextContainer
{
    ITemplateContext Context { get; set; }
}
