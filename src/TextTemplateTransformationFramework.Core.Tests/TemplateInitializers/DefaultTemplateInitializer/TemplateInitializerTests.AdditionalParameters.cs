namespace TemplateFramework.Core.Tests.TemplateInitializers;

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
            sut.Initialize(template, DefaultFilename, TemplateEngineMock.Object, model, null, null);

            // Assert
            template.Model.Should().Be(model);
        }

        [Fact]
        public void Sets_AdditionalParameters_When_Template_Implements_IParameterizedTemplate()
        {
            // Arrange
            var sut = CreateSut();
            var additionalParameters = new { AdditionalParameter = "Hello world!" };
            var template = new TestData.PlainTemplateWithAdditionalParameters();

            // Act
            sut.Initialize(template, DefaultFilename, TemplateEngineMock.Object, default(object?), additionalParameters, null);

            // Assert
            template.AdditionalParameter.Should().Be(additionalParameters.AdditionalParameter);
        }

        [Fact]
        public void Converts_AdditionalParameter_When_Converter_Is_Available()
        {
            // Arrange
            var sut = CreateSut();
            var additionalParameters = new { AdditionalParameter = "?" };
            var template = new TestData.PlainTemplateWithAdditionalParameters();
            object? convertedValue = "Hello world!";
            TemplateParameterConverterMock.Setup(x => x.TryConvert(It.IsAny<object?>(), It.IsAny<Type>(), out convertedValue))
                                          .Returns(true);

            // Act
            sut.Initialize(template, DefaultFilename, TemplateEngineMock.Object, default(object?), additionalParameters, null);

            // Assert
            template.AdditionalParameter.Should().BeEquivalentTo(convertedValue.ToString());
        }

        [Fact]
        public void Throws_On_Non_Existing_AdditionalParameters()
        {
            // Arrange
            var sut = CreateSut();
            var additionalParameters = new { AdditionalParameter = "Hello world!", NonExistingParameter = "Ignored" };
            var template = new TestData.PlainTemplateWithAdditionalParameters();

            // Act & Assert
            sut.Invoking(x => x.Initialize(template, DefaultFilename, TemplateEngineMock.Object, default(object?), additionalParameters, null))
               .Should().Throw<NotSupportedException>().WithMessage("Unsupported template parameter: NonExistingParameter");
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
            sut.Initialize(template, DefaultFilename, TemplateEngineMock.Object, model, additionalParameters, null);

            // Assert
            template.Model.Should().Be(model);
            template.AdditionalParameter.Should().Be(additionalParameters.AdditionalParameter);
        }
    }
}
