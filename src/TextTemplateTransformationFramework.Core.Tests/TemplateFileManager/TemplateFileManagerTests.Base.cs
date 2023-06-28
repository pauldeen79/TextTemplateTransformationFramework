namespace TextTemplateTransformationFramework.Core.Tests;

public partial class TemplateFileManagerTests
{
    protected Mock<IMultipleContentBuilder> MultipleContentBuilderMock { get; } = new();

    protected TemplateFileManager CreateSut() => new(MultipleContentBuilderMock.Object, new StringBuilder());
}
