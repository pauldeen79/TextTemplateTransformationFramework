namespace TextTemplateTransformationFramework.Core.Tests.TemplateRenderers;

public partial class SingleContentTemplateRendererTests
{
    internal SingleContentTemplateRenderer CreateSut() => new();
    protected const string DefaultFilename = "MyFile.txt";
}
