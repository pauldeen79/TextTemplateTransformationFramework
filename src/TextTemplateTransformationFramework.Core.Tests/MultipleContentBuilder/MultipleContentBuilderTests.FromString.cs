namespace TextTemplateTransformationFramework.Core.Tests;

public partial class MultipleContentBuilderTests
{
    public class FromString : MultipleContentBuilderTests
    {
        [Fact]
        public void Throws_On_Null_Xml()
        {
            // Act & Assert
            this.Invoking(_ => MultipleContentBuilder.FromString(null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("xml");
        }

        [Fact]
        public void Throws_On_Empty_Xml()
        {
            // Act & Assert
            this.Invoking(_ => MultipleContentBuilder.FromString(string.Empty))
                .Should().Throw<ArgumentException>().WithParameterName("xml");
        }

        [Fact]
        public void Throws_On_WhiteSpace_Xml()
        {
            // Act & Assert
            this.Invoking(_ => MultipleContentBuilder.FromString(" "))
                .Should().Throw<ArgumentException>().WithParameterName("xml");
        }

        [Fact]
        public void Returns_New_Instance_When_Xml_Is_Filled()
        {
            // Arrange
            const string xml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TextTemplateTransformationFramework"">
  <BasePath></BasePath>
  <Contents>
    <Contents>
      <FileName>File1.txt</FileName>
      <Lines xmlns:d4p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
        <d4p1:string>Test1</d4p1:string>
        <d4p1:string></d4p1:string>
      </Lines>
      <SkipWhenFileExists>false</SkipWhenFileExists>
    </Contents>
    <Contents>
      <FileName>File2.txt</FileName>
      <Lines xmlns:d4p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
        <d4p1:string>Test2</d4p1:string>
        <d4p1:string></d4p1:string>
      </Lines>
      <SkipWhenFileExists>false</SkipWhenFileExists>
    </Contents>
  </Contents>
</MultipleContents>";

            // Act
            var instance = MultipleContentBuilder.FromString(xml);

            // Assert
            instance.Should().NotBeNull();
            instance.BasePath.Should().BeEmpty();
            instance.Contents.Should().HaveCount(2);
            instance.Contents.First().FileName.Should().Be("File1.txt");
            instance.Contents.Last().FileName.Should().Be("File2.txt");
        }
    }
}
