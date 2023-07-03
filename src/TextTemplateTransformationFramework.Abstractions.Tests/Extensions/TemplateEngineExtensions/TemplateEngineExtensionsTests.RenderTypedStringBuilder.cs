﻿namespace TextTemplateTransformationFramework.Abstractions.Tests.Extensions;

public partial class TemplateEngineExtensionsTests
{
    public class RenderTyped_StringBuilder : TemplateEngineExtensionsTests
    {
        [Fact]
        public void Template_And_GenerationEnvironment_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateTypedSut();

            // Act
            sut.Object.Render(Template, StringBuilder, Model);

            // Assert
            sut.Verify(x => x.Render(Template, StringBuilder, Model, string.Empty, null), Times.Once);
        }

        [Fact]
        public void Template_GenerationEnvironment_And_DefaultFileName_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateTypedSut();

            // Act
            sut.Object.Render(Template, StringBuilder, Model, DefaultFileName);

            // Assert
            sut.Verify(x => x.Render(Template, StringBuilder, Model, DefaultFileName, null), Times.Once);
        }

        [Fact]
        public void Template_GenerationEnvironment_And_AdditionalParameters_Arguments_Works_Correctly()
        {
            // Arrange
            var sut = CreateTypedSut();

            // Act
            sut.Object.Render(Template, StringBuilder, Model, AdditionalParameters);

            // Assert
            sut.Verify(x => x.Render(Template, StringBuilder, Model, string.Empty, AdditionalParameters), Times.Once);
        }
    }
}
