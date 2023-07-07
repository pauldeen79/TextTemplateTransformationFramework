namespace TextTemplateTransformationFramework.Core.Tests;

public class IntegrationTests
{
    [Fact]
    public void Can_Render_MultipleContentBuilderTemplate_With_ChildTemplate_And_TemplateContext()
    {
        // Arrange
        var sut = new TemplateEngine();

        // For now, we setup the child template creators using a mock implementation.
        // When using DI, you have to register your own template creators as IChildTemplateCreator, and the child template factory as IChildTemplateFactory, and then resolve the IChildTemplateFactory.
        var childTemplateCreatorMock = new Mock<IChildTemplateCreator>();
        childTemplateCreatorMock.Setup(x => x.SupportsName(It.IsAny<string>()))
                                .Returns<string>(name => name == "MyChildTemplate");
        childTemplateCreatorMock.Setup(x => x.CreateByName(It.IsAny<string>()))
                                .Returns<string>(name => name == "MyChildTemplate"
                                    ? new TestData.PlainTemplateWithTemplateContext(context => "Context IsRootContext: " + context.IsRootContext)
                                    : throw new NotSupportedException("What are you doing?!"));

        var childTemplateFactory = new ChildTemplateFactory(new[] { childTemplateCreatorMock.Object });
        var template = new TestData.MultipleContentBuilderTemplateWithTemplateContextAndTemplateEngine(childTemplateFactory, (builder, context, engine, factory) =>
        {
            var childTemplate = factory.CreateByName("MyChildTemplate");
            engine.Render(childTemplate, builder, context.CreateChildContext(childTemplate));
        });
        var generationEnvironment = new MultipleContentBuilder(new Mock<IFileSystem>().Object, Encoding.UTF8, TestData.BasePath);

        // Act
        sut.Render(template, generationEnvironment);

        // Assert
        generationEnvironment.Contents.Should().ContainSingle();
        generationEnvironment.Contents.Single().Builder.ToString().Should().Be("Context IsRootContext: False");
    }
}
