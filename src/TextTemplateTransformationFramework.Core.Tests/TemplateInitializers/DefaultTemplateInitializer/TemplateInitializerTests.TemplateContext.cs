namespace TextTemplateTransformationFramework.Core.Tests.TemplateInitializers;

public partial class TemplateInitializerTests
{
    public class TemplateContext : TemplateInitializerTests
    {
        [Fact]
        public void Sets_TemplateContext_On_Template_When_Possible()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.PlainTemplateWithTemplateContext(_ => "Hello world!");
            var context = new Core.TemplateContext(template);

            // Act
            sut.Initialize(template, DefaultFilename, default(object?), null, context);

            // Assert
            template.Context.Should().BeSameAs(context);
        }

        [Fact]
        public void Initializes_New_TemplateContext_Without_Model_When_Not_Provided()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.PlainTemplateWithTemplateContext(_ => "Hello world!");

            // Act
            sut.Initialize(template, DefaultFilename, default(object?), null, context: null);

            // Assert
            template.Context.Should().NotBeNull();
            template.Context.Template.Should().BeSameAs(template);
            template.Context.Model.Should().BeNull();
            template.Context.IsRootContext.Should().BeTrue();
            template.Context.ParentContext.Should().BeNull();
        }

        [Fact]
        public void Initializes_New_TemplateContext_With_Model_When_Not_Provided()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.PlainTemplateWithTemplateContext(_ => "Hello world!"); // note that this template type does not implement IModelContainer<T>, so the model property will not be set. But it will be available in the TemplateContext (untyped)
            var model = "Hello world!";

            // Act
            sut.Initialize(template, DefaultFilename, model, null, context: null);

            // Assert
            template.Context.Should().NotBeNull();
            template.Context.Template.Should().BeSameAs(template);
            template.Context.Model.Should().Be(model);
            template.Context.IsRootContext.Should().BeTrue();
            template.Context.ParentContext.Should().BeNull();
        }
    }
}
