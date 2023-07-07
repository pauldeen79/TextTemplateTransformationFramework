namespace TextTemplateTransformationFramework.Abstractions;

public interface ITemplateEngineContainer
{
    ITemplateEngine TemplateEngine { get; set; }
}
