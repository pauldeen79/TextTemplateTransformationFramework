namespace TextTemplateTransformationFramework.Core.Tests.TemplateRenderers;

public partial class MultipleContentTemplateRendererTests
{
    public class Render : MultipleContentTemplateRendererTests
    {
        [Fact]
        public void Throws_When_GenerationEnvironment_Is_Null()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Render(new TestData.Template(_ => { }), generationEnvironment: null!, DefaultFilename))
               .Should().Throw<ArgumentException>().WithParameterName("generationEnvironment");
        }

        [Fact]
        public void Throws_When_GenerationEnvironemnt_Is_Not_Supported()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Render(new TestData.Template(_ => { }), generationEnvironment: this, DefaultFilename))
               .Should().Throw<ArgumentException>().WithParameterName("generationEnvironment");
        }

        [Fact]
        public void Throws_When_MultipleContentBuilderContainer_Returns_Null_MultipleContentBuilder()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Render(new TestData.Template(_ => { }), generationEnvironment: new Mock<IMultipleContentBuilderContainer>().Object, DefaultFilename))
               .Should().Throw<ArgumentException>().WithParameterName("generationEnvironment");
        }

        [Fact]
        public void Renders_MultipleContentBuilderTemplate_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var template = new Mock<IMultipleContentBuilderTemplate>();
            var generationEnvironment = new Mock<IMultipleContentBuilder>();

            // Act
            sut.Render(template.Object, generationEnvironment.Object, DefaultFilename);

            // Assert
            template.Verify(x => x.Render(It.IsAny<IMultipleContentBuilder>()), Times.Once);
        }

        [Fact]
        public void Renders_To_MultipleContentBuilder_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.TextTransformTemplate(() => "Hello world!");
            var generationEnvironment = new Mock<IMultipleContentBuilder>();
            var contentBuilderMock = new Mock<IContentBuilder>();
            generationEnvironment.Setup(x => x.AddContent(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<StringBuilder?>()))
                                 .Returns<string, bool, StringBuilder?>((filename, skipWhenFileExists, b) =>
                                 {
                                     contentBuilderMock.SetupGet(x => x.Builder).Returns(b ?? new StringBuilder());

                                     return contentBuilderMock.Object;
                                 });

            // Act
            sut.Render(template, generationEnvironment.Object, DefaultFilename);

            // Assert
            contentBuilderMock.Object.Builder.Should().NotBeNull();
            contentBuilderMock.Object.Builder.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void Renders_To_MultipleContentBuilderContainer_Correctly()
        {
            var sut = CreateSut();
            var template = new TestData.TextTransformTemplate(() => "Hello world!");
            var generationEnvironment = new Mock<IMultipleContentBuilderContainer>();
            var multipleContentBuilderMock = new Mock<IMultipleContentBuilder>();
            generationEnvironment.SetupGet(x => x.MultipleContentBuilder).Returns(multipleContentBuilderMock.Object);
            var contentBuilderMock = new Mock<IContentBuilder>();
            multipleContentBuilderMock.Setup(x => x.AddContent(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<StringBuilder?>()))
                                      .Returns<string, bool, StringBuilder?>((filename, skipWhenFileExists, b) =>
                                      {
                                          contentBuilderMock.SetupGet(x => x.Builder).Returns(b ?? new StringBuilder());
                                      
                                          return contentBuilderMock.Object;
                                      });

            // Act
            sut.Render(template, generationEnvironment.Object, DefaultFilename);

            // Assert
            contentBuilderMock.Object.Builder.Should().NotBeNull();
            contentBuilderMock.Object.Builder.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void Unpacks_MultipleContentBuilder_Result_From_Single_Template_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var contents = new MultipleContentBuilder();
            contents.AddContent("MyFile.txt").Builder.Append("Hello world!");
            var template = new TestData.TextTransformTemplate(() => contents.ToString());
            var generationEnvironment = new Mock<IMultipleContentBuilder>();
            var contentBuilderMock = new Mock<IContentBuilder>();
            generationEnvironment.Setup(x => x.AddContent(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<StringBuilder?>()))
                                 .Returns<string, bool, StringBuilder?>((filename, skipWhenFileExists, b) =>
                                 {
                                     contentBuilderMock.SetupGet(x => x.Builder).Returns(b ?? new StringBuilder());

                                     return contentBuilderMock.Object;
                                 });

            // Act
            sut.Render(template, generationEnvironment.Object, DefaultFilename);

            // Assert
            contentBuilderMock.Object.Builder.Should().NotBeNull();
            contentBuilderMock.Object.Builder.ToString().Should().Be("Hello world!" + Environment.NewLine);
        }
    }
}
