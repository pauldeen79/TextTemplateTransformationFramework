namespace TemplateFramework.Core.Tests.CodeGeneration;

public partial class CodeGenerationSettingsTests
{
    public class Constructor
    {
        [Fact]
        public void Creates_Instance_With_BasePath_And_DryRun()
        {
            // Act
            var sut = new CodeGenerationSettings(TestData.BasePath, true);

            // Assert
            sut.BasePath.Should().Be(TestData.BasePath);
            sut.DryRun.Should().BeTrue();
        }

        [Fact]
        public void Creates_Instance_With_BasePath_And_GenerateMultipleFiles_And_DryRun()
        {
            // Act
            var sut = new CodeGenerationSettings(TestData.BasePath, true, true);

            // Assert
            sut.BasePath.Should().Be(TestData.BasePath);
            sut.GenerateMultipleFiles.Should().BeTrue();
            sut.DryRun.Should().BeTrue();
        }

        [Fact]
        public void Creates_Instance_With_BasePath_And_GeneratedMultipleFIles_And_SkipWhenFileExists_And_DryRun()
        {
            // Act
            var sut = new CodeGenerationSettings(TestData.BasePath, true, true, true);

            // Assert
            sut.BasePath.Should().Be(TestData.BasePath);
            sut.GenerateMultipleFiles.Should().BeTrue();
            sut.SkipWhenFileExists.Should().BeTrue();
            sut.DryRun.Should().BeTrue();
        }

        [Fact]
        public void Throws_On_Null_BasePath()
        {
            this.Invoking(_ => new CodeGenerationSettings(basePath: null!, false))
                .Should().Throw<ArgumentNullException>().WithParameterName("basePath");
        }
    }
}
