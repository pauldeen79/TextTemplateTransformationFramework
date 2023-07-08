namespace TextTemplateTransformationFramework.Core.Tests;

public class IntegrationTests
{
    [Fact]
    public void Can_Render_MultipleContentBuilderTemplate_With_ChildTemplate_And_TemplateContext()
    {
        // Arrange
        var childTemplateCreatorMock = new Mock<IChildTemplateCreator>();
        childTemplateCreatorMock.Setup(x => x.SupportsName(It.IsAny<string>()))
                                .Returns<string>(name => name == "MyChildTemplate");
        childTemplateCreatorMock.Setup(x => x.CreateByName(It.IsAny<string>()))
                                .Returns<string>(name => name == "MyChildTemplate"
                                    ? new TestData.PlainTemplateWithTemplateContext(context => "Context IsRootContext: " + context.IsRootContext)
                                    : throw new NotSupportedException("What are you doing?!"));
        using var provider = new ServiceCollection()
            .AddTemplateFramework()
            .AddSingleton(_ => childTemplateCreatorMock.Object)
            .BuildServiceProvider();
        var sut = provider.GetRequiredService<ITemplateEngine>();

        var childTemplateFactory = provider.GetRequiredService<IChildTemplateFactory>();
        var template = new TestData.MultipleContentBuilderTemplateWithTemplateContextAndTemplateEngine(childTemplateFactory, (builder, context, engine, factory) =>
        {
            var childTemplate = factory.CreateByName("MyChildTemplate");
            engine.Render(childTemplate, builder, context.CreateChildContext(childTemplate));
        });
        var fileSystemMock = new Mock<IFileSystem>();
        var generationEnvironment = new MultipleContentBuilder(fileSystemMock.Object, Encoding.UTF8, TestData.BasePath);

        // Act
        sut.Render(template, generationEnvironment);

        // Assert
        generationEnvironment.Contents.Should().ContainSingle();
        generationEnvironment.Contents.Single().Builder.ToString().Should().Be("Context IsRootContext: False");
    }
}
