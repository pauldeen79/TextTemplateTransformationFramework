namespace TextTemplateTransformationFramework.Core.Tests;

public partial class ChildTemplateFactoryTests
{
    protected ChildTemplateFactory CreateSut() => new(new[] { ChildTemplateCreatorMock.Object });
    protected Mock<IChildTemplateCreator> ChildTemplateCreatorMock { get; } = new();
}
