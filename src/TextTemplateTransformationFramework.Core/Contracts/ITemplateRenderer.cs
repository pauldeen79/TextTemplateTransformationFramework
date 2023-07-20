namespace TemplateFramework.Core.Contracts;

public interface ITemplateRenderer
{
    bool Supports(IGenerationEnvironment generationEnvironment);
    void Render(object template, IGenerationEnvironment generationEnvironment, string defaultFilename);
}
