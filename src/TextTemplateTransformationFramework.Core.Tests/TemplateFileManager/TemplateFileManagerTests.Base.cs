﻿namespace TextTemplateTransformationFramework.Core.Tests;

public partial class TemplateFileManagerTests
{
    protected Mock<IMultipleContentBuilder> MultipleContentBuilderMock { get; } = new();
    protected Mock<IFileSystem> FileSystemMock { get; } = new();
    protected StringBuilder StringBuilder { get; } = new();

    protected TemplateFileManager CreateSut() => new(MultipleContentBuilderMock.Object, StringBuilder);

    protected TemplateFileManager CreateSutWithRealMultipleContentBuilder() => new(new MultipleContentBuilder(FileSystemMock.Object, Encoding.UTF8), StringBuilder);


    protected IContentBuilder CreateContentBuilder(string fileName, bool skipFileWhenExists, StringBuilder builder)
    {
        var mock = new Mock<IContentBuilder>();
        mock.SetupGet(x => x.FileName).Returns(fileName);
        mock.SetupGet(x => x.SkipWhenFileExists).Returns(skipFileWhenExists);
        mock.SetupGet(x => x.Builder).Returns(builder);
        return mock.Object;
    }
}
