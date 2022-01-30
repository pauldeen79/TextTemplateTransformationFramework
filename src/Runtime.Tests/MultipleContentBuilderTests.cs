using System.IO;
using FluentAssertions;
using Xunit;

namespace TextTemplateTransformationFramework.Runtime.Tests
{
    public class MultipleContentBuilderTests
    {
        [Fact]
        public void SaveAll_Saves_All_Contents()
        {
            // Arrange
            var basePath = Path.Combine(Path.GetTempPath(), nameof(SaveAll_Saves_All_Contents));
            if (Directory.Exists(basePath))
            {
                Directory.Delete(basePath, true);
            }
            var sut = new MultipleContentBuilder(basePath);
            var c1 = sut.AddContent("File1.txt");
            c1.Builder.AppendLine("Test1");
            var c2 = sut.AddContent("File2.txt");
            c2.Builder.AppendLine("Test2");

            // Act
            sut.SaveAll();

            // Assert
            File.Exists(Path.Combine(basePath, "File1.txt")).Should().BeTrue();
            File.Exists(Path.Combine(basePath, "File2.txt")).Should().BeTrue();
        }
    }
}
