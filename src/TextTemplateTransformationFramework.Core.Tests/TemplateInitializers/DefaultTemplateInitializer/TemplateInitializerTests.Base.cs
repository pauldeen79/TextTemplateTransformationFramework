namespace TextTemplateTransformationFramework.Core.Tests.TemplateInitializers;

public partial class TemplateInitializerTests
{
    protected DefaultTemplateInitializer CreateSut() => new();
    protected Mock<ITemplateEngine> TemplateEngineMock { get; } = new();
    protected const string DefaultFilename = "DefaultFilename.txt";
}
