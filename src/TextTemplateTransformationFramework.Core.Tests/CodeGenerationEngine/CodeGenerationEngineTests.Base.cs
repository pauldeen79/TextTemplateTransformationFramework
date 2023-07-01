namespace TextTemplateTransformationFramework.Core.Tests;

public abstract partial class CodeGenerationEngineTests
{
    protected Mock<ITemplateRenderer> TemplateRendererMock { get; } = new();
    protected Mock<ITemplateRenderer<string>> TypedTemplateRendererMock { get; } = new();
    protected Mock<ITemplateFileManager> TemplateFileManagerMock { get; } = new();
    protected Mock<ITemplateFileManagerFactory> TemplateFileManagerFactoryMock { get; }

    protected CodeGenerationEngineTests()
    {
        TemplateFileManagerFactoryMock = new Mock<ITemplateFileManagerFactory>();
        TemplateFileManagerFactoryMock.Setup(x => x.Create(It.IsAny<string>())).Returns(TemplateFileManagerMock.Object);
    }

    protected CodeGenerationEngine CreateSut() => new(TemplateRendererMock.Object, TemplateFileManagerFactoryMock.Object, string.Empty);
    protected CodeGenerationEngine<string> CreateTypedSut() => new(TypedTemplateRendererMock.Object, TemplateFileManagerFactoryMock.Object);
}
