namespace TextTemplateTransformationFramework.Core.Tests;

public partial class TemplateFileManagerTests
{
    protected Mock<IMultipleContentBuilder> MultipleContentBuilderMock { get; } = new();

    protected TemplateFileManager CreateSut() => new(MultipleContentBuilderMock.Object, new StringBuilder());

    protected IContent CreateContent(string fileName, bool skipFileWhenExists, StringBuilder builder)
    {
        var mock = new Mock<IContent>();
        mock.SetupGet(x => x.FileName).Returns(fileName);
        mock.SetupGet(x => x.SkipWhenFileExists).Returns(skipFileWhenExists);
        mock.SetupGet(x => x.Builder).Returns(builder);
        return mock.Object;
    }
}
