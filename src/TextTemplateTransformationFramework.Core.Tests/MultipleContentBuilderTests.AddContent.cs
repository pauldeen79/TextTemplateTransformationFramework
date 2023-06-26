namespace TextTemplateTransformationFramework.Core.Tests;

public partial class MultipleContentBuilderTests
{
    public class AddContent : MultipleContentBuilderTests
    {
        [Fact]
        public void Throws_On_Null_FileName()
        {
            // Arrange
            var sut = CreateSut(TestData.BasePath);

            // Act & Assert
            sut.Invoking(x => x.AddContent(null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("fileName");
        }

        [Fact]
        public void Uses_StringBuilder_When_Supplied()
        {
            // Arrange
            var sut = CreateSut(TestData.BasePath);
            var builder = new IndentedStringBuilder("ExistingContent");

            // Act
            var result = sut.AddContent("File.txt", builder: builder);

            // Assert
            result.Builder.ToString().Should().Be("ExistingContent");
        }

        [Fact]
        public void Creates_New_StringBulder_When_Not_Supplied()
        {
            // Arrange
            var sut = CreateSut(TestData.BasePath);

            // Act
            var result = sut.AddContent("File.txt");

            // Assert
            result.Builder.ToString().Should().BeEmpty();
        }

        [Fact]
        public void Sets_FileName_Correctly()
        {
            // Arrange
            var sut = CreateSut(TestData.BasePath);

            // Act
            var result = sut.AddContent("File.txt");

            // Assert
            result.FileName.Should().Be("File.txt");
        }

        [Fact]
        public void Sets_SkipWhenFileExists_Correctly()
        {
            // Arrange
            var sut = CreateSut(TestData.BasePath);

            // Act
            var result = sut.AddContent("File.txt", skipWhenFileExists: true);

            // Assert
            result.SkipWhenFileExists.Should().BeTrue();
        }

        [Fact]
        public void Adds_New_File_To_Content_List()
        {
            // Arrange
            var sut = CreateSut(TestData.BasePath);

            // Act
            _ = sut.AddContent("File.txt");

            // Assert
            sut.Contents.Should().HaveCount(3); //two are added from initialization in CreateSut, one is added using AddContent here
            sut.Contents.Should().Contain(x => x.FileName == "File.txt");
        }
    }
}
