namespace TemplateFramework.Abstractions.Tests.Extensions;

public partial class TemplateEngineExtensionsTests
{
    protected Mock<ITemplateEngine> CreateSut() => new();
    protected object Template { get; } = new();
    protected object Model { get; } = new object();
    protected object AdditionalParameters { get; } = new();
    protected StringBuilder StringBuilder { get; } = new();
    protected Mock<IMultipleContentBuilder> MultipleContentBuilderMock { get; } = new();
    protected Mock<IMultipleContentBuilderContainer> MultipleContentBuilderContainerMock { get; } = new();
    protected Mock<ITemplateContext> TemplateContextMock { get; } = new();
    protected const string DefaultFilename = "Default.txt";
}
