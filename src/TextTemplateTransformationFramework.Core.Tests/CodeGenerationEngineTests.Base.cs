namespace TextTemplateTransformationFramework.Core.Tests;

public partial class CodeGenerationEngineTests
{
    protected Mock<ITemplateRenderer> TemplateRendererMock { get; } = new();
    protected Mock<ITemplateFileManager> TemplateFileManagerMock { get; } = new();

    protected CodeGenerationEngine CreateSut() => new(TemplateRendererMock.Object, TemplateFileManagerMock.Object);
}
