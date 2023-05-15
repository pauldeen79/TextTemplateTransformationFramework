using FluentAssertions;
using TextTemplateTransformationFramework.Runtime.CodeGeneration;
using Xunit;

namespace TextTemplateTransformationFramework.Runtime.Tests.CodeGeneration
{
    public class CodeGenerationSettingsTests
    {
        [Fact]
        public void Can_Generate_CodeGenerationSettings_With_BasePath_And_DryRun()
        {
            // Act
            var settings = new CodeGenerationSettings("BasePath", true);

            // Assert
            settings.BasePath.Should().Be("BasePath");
            settings.GenerateMultipleFiles.Should().BeFalse();
            settings.SkipWhenFileExists.Should().BeFalse();
            settings.DryRun.Should().BeTrue();
        }

        [Fact]
        public void Can_Generate_CodeGenerationSettings_With_BasePath_And_GenerateMultipleFiles_And_DryRun()
        {
            // Act
            var settings = new CodeGenerationSettings("BasePath", true, true);

            // Assert
            settings.BasePath.Should().Be("BasePath");
            settings.GenerateMultipleFiles.Should().BeTrue();
            settings.SkipWhenFileExists.Should().BeFalse();
            settings.DryRun.Should().BeTrue();
        }

        [Fact]
        public void Can_Generate_CodeGenerationSettings_With_BasePath_And_GenerateMultipleFiles_And_SkipWhenFileExists_And_DryRun()
        {
            // Act
            var settings = new CodeGenerationSettings("BasePath", true, true, true);

            // Assert
            settings.BasePath.Should().Be("BasePath");
            settings.GenerateMultipleFiles.Should().BeTrue();
            settings.SkipWhenFileExists.Should().BeTrue();
            settings.DryRun.Should().BeTrue();
        }
    }
}
