﻿namespace TemplateFramework.Core.Tests;

public partial class TemplateEngineTests
{
    protected StringBuilder StringBuilder { get; } = new();
    protected Mock<IMultipleContentBuilder> MultipleContentBuilderMock { get; } = new();
    protected Mock<IMultipleContentBuilderContainer> MultipleContentBuilderContainerMock { get; } = new();

    protected static readonly ITemplateRenderer[] DefaultTemplateRenderers = new ITemplateRenderer[] { new SingleContentTemplateRenderer(), new MultipleContentTemplateRenderer() };

    protected Mock<ITemplateInitializer> TemplateInitializerMock { get; } = new();
    protected Mock<ITemplateRenderer> TemplateRendererMock { get; } = new();

    protected TemplateEngine CreateSut() => new(TemplateInitializerMock.Object, DefaultTemplateRenderers);
    protected TemplateEngine<string> CreateTypedSut() => new(TemplateInitializerMock.Object, DefaultTemplateRenderers);
}
