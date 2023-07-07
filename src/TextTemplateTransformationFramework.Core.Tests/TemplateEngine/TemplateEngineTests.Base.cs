namespace TextTemplateTransformationFramework.Core.Tests;

public partial class TemplateEngineTests
{
    protected StringBuilder StringBuilder { get; } = new();
    protected Mock<IMultipleContentBuilder> MultipleContentBuilderMock { get; } = new();
    protected Mock<IMultipleContentBuilderContainer> MultipleContentBuilderContainerMock { get; } = new();

    internal static readonly ITemplateRenderer[] DefaultTemplateRenderers = new ITemplateRenderer[] { new SingleContentTemplateRenderer(), new MultipleContentTemplateRenderer() };

    internal Mock<ITemplateInitializer> TemplateInitializerMock { get; } = new();
    internal Mock<ITemplateRenderer> TemplateRendererMock { get; } = new();

    protected TemplateEngine CreateSut() => new();
    protected TemplateEngine<string> CreateTypedSut() => new();
}
