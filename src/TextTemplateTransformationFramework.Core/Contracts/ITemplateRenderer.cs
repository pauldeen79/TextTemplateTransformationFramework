namespace TextTemplateTransformationFramework.Core.Contracts;

internal interface ITemplateRenderer
{
    bool Supports(object generationEnvironment);
    void Render(object template, object generationEnvironment, string defaultFilename);
}
