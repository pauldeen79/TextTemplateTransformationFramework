namespace TemplateFramework.Abstractions.Tests.Extensions;

public partial class TemplateEngineExtensionsTests
{
    protected Mock<ITemplateEngine> CreateSut() => new();
    protected Mock<ITemplateEngine<string>> CreateTypedSut() => new();
    protected object Template { get; } = new();
    protected const string Model = "Hello world";
    protected object AdditionalParameters { get; } = new();
    protected StringBuilder StringBuilder { get; } = new();
    protected Mock<IMultipleContentBuilder> MultipleContentBuilderMock { get; } = new();
    protected Mock<IMultipleContentBuilderContainer> MultipleContentBuilderContainerMock { get; } = new();
    protected Mock<ITemplateContext> TemplateContextMock { get; } = new();
    protected const string DefaultFilename = "Default.txt";
}
