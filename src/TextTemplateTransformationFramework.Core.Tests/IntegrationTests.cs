namespace TemplateFramework.Core.Tests;

public class IntegrationTests
{
    [Fact]
    public void Can_Render_MultipleContentBuilderTemplate_With_ChildTemplate_And_TemplateContext()
    {
        // Arrange
        var templateCreatorMock = new Mock<ITemplateCreator>();
        templateCreatorMock.Setup(x => x.SupportsName(It.IsAny<string>()))
                           .Returns<string>(name => name == "MyTemplate");
        templateCreatorMock.Setup(x => x.CreateByName(It.IsAny<string>()))
                           .Returns<string>(name => name == "MyTemplate"
                                ? new TestData.PlainTemplateWithTemplateContext(context => "Context IsRootContext: " + context.IsRootContext)
                                : throw new NotSupportedException("What are you doing?!"));
        using var provider = new ServiceCollection()
            .AddTemplateFramework()
            .AddSingleton(_ => templateCreatorMock.Object)
            .BuildServiceProvider();
        var sut = provider.GetRequiredService<ITemplateEngine>();

        var templateFactory = provider.GetRequiredService<ITemplateFactory>();
        var template = new TestData.MultipleContentBuilderTemplateWithTemplateContextAndTemplateEngine(templateFactory, (builder, context, engine, factory) =>
        {
            var childTemplate = factory.CreateByName("MyTemplate");
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
