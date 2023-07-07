namespace TextTemplateTransformationFramework.Core.Tests;

public class IntegrationTests
{
    [Fact]
    public void Can_Render_MultipleContentBuilderTemplate_With_ChildTemplate_And_TemplateContext()
    {
        // Arrange
        var sut = new TemplateEngine();
        var template = new TestData.MultipleContentBuilderTemplateWithTemplateContextAndTemplateEngine((builder, context, engine) =>
        {
            var childTemplate = new TestData.PlainTemplateWithTemplateContext(context => "Context IsRootContext: " + context.IsRootContext);
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
