﻿namespace TextTemplateTransformationFramework.Core.Tests;

public partial class TemplateContextTests
{
    public class RootContext
    {
        [Fact]
        public void Returns_Same_Instance_When_There_Is_No_ParentContext()
        {
            // Arrange
            var sut = new TemplateContext(template: this, parentContext: null);

            // Act
            var result = sut.RootContext;

            // Assert
            result.RootContext.Should().BeSameAs(sut);
        }

        [Fact]
        public void Returns_RootContext_One_Level_Deep()
        {
            // Arrange
            var parentTemplateContext = new TemplateContext(template: this, parentContext: null);
            var sut = new TemplateContext(template: this, parentContext: parentTemplateContext);

            // Act
            var result = sut.RootContext;

            // Assert
            result.RootContext.Should().BeSameAs(parentTemplateContext);
        }

        [Fact]
        public void Returns_RootContext_Two_Levels_Deep()
        {
            // Arrange
            var rootTemplateContext = new TemplateContext(template: this, parentContext: null);
            var parentTemplateContext = new TemplateContext(template: this, parentContext: rootTemplateContext);
            var sut = new TemplateContext(template: this, parentContext: parentTemplateContext);

            // Act
            var result = sut.RootContext;

            // Assert
            result.RootContext.Should().BeSameAs(rootTemplateContext);
        }
    }
}
