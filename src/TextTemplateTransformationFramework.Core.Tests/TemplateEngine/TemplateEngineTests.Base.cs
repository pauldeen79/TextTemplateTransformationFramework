namespace TextTemplateTransformationFramework.Core.Tests;

public partial class TemplateEngineTests
{
    protected StringBuilder StringBuilder { get; } = new();
    protected Mock<IMultipleContentBuilder> MultipleContentBuilderMock { get; } = new();
    protected Mock<IMultipleContentBuilderContainer> MultipleContentBuilderContainerMock { get; } = new();

    protected TemplateEngine CreateSut() => new();
    protected TemplateEngine<string> CreateTypedSut() => new();
}
