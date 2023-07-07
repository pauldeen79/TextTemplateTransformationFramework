namespace TextTemplateTransformationFramework.Core.Tests;

public partial class TemplateInitializerTests
{
    public class AdditionalParameters : TemplateInitializerTests
    {
        [Fact]
        public void Sets_Model_When_Possible()
        {
            // Arrange
            var sut = CreateSut();
            var model = "Hello world!";
            var template = new TestData.TemplateWithModel<string>(_ => { });

            // Act
            sut.Initialize(template, DefaultFilename, model, null, null);

            // Assert
            template.Model.Should().Be(model);
        }

        [Fact]
        public void Sets_AdditionalParameters_When_On_Public_Property()
        {
            // Arrange
            var sut = CreateSut();
            var additionalParameters = new { AdditionalParameter = "Hello world!" };
            var template = new TestData.PlainTemplateWithAdditionalParameters();

            // Act
            sut.Initialize(template, DefaultFilename, default(object?), additionalParameters, null);

            // Assert
            template.AdditionalParameter.Should().Be(additionalParameters.AdditionalParameter);
        }

        [Fact]
        public void Sets_AdditionalParameters_When_On_Internal_Property()
        {
            // Arrange
            var sut = CreateSut();
            var additionalParameters = new { InternalParameter = "Hello world!" };
            var template = new TestData.PlainTemplateWithAdditionalParameters();

            // Act
            sut.Initialize(template, DefaultFilename, default(object?), additionalParameters, null);

            // Assert
            template.InternalParameter.Should().Be(additionalParameters.InternalParameter);
        }

        [Fact]
        public void Skips_Non_Existing_AdditionalParameters()
        {
            // Arrange
            var sut = CreateSut();
            var additionalParameters = new { AdditionalParameter = "Hello world!", NonExistingParameter = "Ignored" };
            var template = new TestData.PlainTemplateWithAdditionalParameters();

            // Act
            sut.Initialize(template, DefaultFilename, default(object?), additionalParameters, null);

            // Assert
            template.AdditionalParameter.Should().Be(additionalParameters.AdditionalParameter);
        }

        [Fact]
        public void Skips_Model_AdditionalParameter()
        {
            // Arrange
            var sut = CreateSut();
            var model = "Hello world!";
            var additionalParameters = new { AdditionalParameter = "Hello world!", Model = "Ignored" };
            var template = new TestData.PlainTemplateWithModelAndAdditionalParameters<string>();

            // Act
            sut.Initialize(template, DefaultFilename, model, additionalParameters, null);

            // Assert
            template.Model.Should().Be(model);
            template.AdditionalParameter.Should().Be(additionalParameters.AdditionalParameter);
        }
    }
}
