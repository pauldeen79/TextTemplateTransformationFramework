namespace TextTemplateTransformationFramework.Core.Tests
{
    public partial class MultipleContentBuilderTests
    {
        public class SaveAll : MultipleContentBuilderTests
        {
            [Fact]
            public void Saves_All_Contents()
            {
                var sut = new MultipleContentBuilder(FileSystemMock.Object, Encoding.UTF8, TestData.BasePath);
                var c1 = sut.AddContent("File1.txt");
                c1.Builder.AppendLine("Test1");
                var c2 = sut.AddContent("File2.txt");
                c2.Builder.AppendLine("Test2");

                // Act
                sut.SaveAll();

                // Assert
                FileSystemMock.Verify(x => x.WriteAllText(Path.Combine(TestData.BasePath, "File1.txt"), "Test1" + Environment.NewLine, Encoding.UTF8), Times.Once);
                FileSystemMock.Verify(x => x.WriteAllText(Path.Combine(TestData.BasePath, "File2.txt"), "Test2" + Environment.NewLine, Encoding.UTF8), Times.Once);
            }
        }
    }
}
