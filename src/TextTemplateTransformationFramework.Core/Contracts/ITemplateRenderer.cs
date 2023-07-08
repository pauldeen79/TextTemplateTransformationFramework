namespace TemplateFramework.Core.Contracts;

public interface ITemplateRenderer
{
    bool Supports(object generationEnvironment);
    void Render(object template, object generationEnvironment, string defaultFilename);
}
