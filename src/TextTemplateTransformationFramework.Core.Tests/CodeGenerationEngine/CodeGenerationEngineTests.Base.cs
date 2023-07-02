namespace TextTemplateTransformationFramework.Core.Tests;

public abstract partial class CodeGenerationEngineTests
{
    protected Mock<ITemplateEngine> TemplateEngineMock { get; } = new();
    protected Mock<ITemplateEngine<string>> TypedTemplateEngineMock { get; } = new();
    protected Mock<ITemplateFileManager> TemplateFileManagerMock { get; } = new();
    protected Mock<ITemplateFileManagerFactory> TemplateFileManagerFactoryMock { get; }

    protected CodeGenerationEngineTests()
    {
        TemplateFileManagerFactoryMock = new Mock<ITemplateFileManagerFactory>();
        TemplateFileManagerFactoryMock.Setup(x => x.Create(It.IsAny<string>())).Returns(TemplateFileManagerMock.Object);
    }

    protected CodeGenerationEngine CreateSut() => new(TemplateEngineMock.Object, TemplateFileManagerFactoryMock.Object, string.Empty);
    protected CodeGenerationEngine<string> CreateTypedSut() => new(TypedTemplateEngineMock.Object, TemplateFileManagerFactoryMock.Object);
}
