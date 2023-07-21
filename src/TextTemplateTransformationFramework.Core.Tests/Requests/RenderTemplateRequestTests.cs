namespace TemplateFramework.Core.Tests.Requests;

public class RenderTemplateRequestTests
{
    public class UnTyped
    {
        [Fact]
        public void Throws_On_Null_StringBuilder_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!, string.Empty, null, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!, string.Empty))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!, string.Empty, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!, string.Empty, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!, string.Empty, null, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!, string.Empty))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!, string.Empty, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!, string.Empty, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: null!, string.Empty, null, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: null!, string.Empty))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: null!, string.Empty, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: null!, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: null!, string.Empty, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: null!, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Throws_On_Null_Template()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(template: null!, new StringBuilder()))
                .Should().Throw<ArgumentNullException>().WithParameterName("template");
        }

        [Fact]
        public void Throws_On_Null_DefaultFileName()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, new StringBuilder(), defaultFilename: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("defaultFilename");
        }
    }

    public class Typed
    {
        [Fact]
        public void Throws_On_Null_StringBuilder_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (StringBuilder)null!, null, string.Empty, null, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (StringBuilder)null!, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (StringBuilder)null!, null, string.Empty))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (StringBuilder)null!, null, string.Empty, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (StringBuilder)null!, null, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (StringBuilder)null!, null, string.Empty, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (StringBuilder)null!, null, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (IMultipleContentBuilder)null!, null, string.Empty, null, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (IMultipleContentBuilder)null!, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (IMultipleContentBuilder)null!, null, string.Empty))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (IMultipleContentBuilder)null!, null, string.Empty, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (IMultipleContentBuilder)null!, null, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (IMultipleContentBuilder)null!, null, string.Empty, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (IMultipleContentBuilder)null!, null, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builderContainer: null!, null, string.Empty, null, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builderContainer: null!, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builderContainer: null!, null, string.Empty))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builderContainer: null!, null, string.Empty, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builderContainer: null!, null, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builderContainer: null!, null, string.Empty, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builderContainer: null!, null, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Throws_On_Null_Template()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(template: null!, new StringBuilder(), null))
                .Should().Throw<ArgumentNullException>().WithParameterName("template");
        }

        [Fact]
        public void Throws_On_Null_DefaultFileName()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, new StringBuilder(), null, defaultFilename: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("defaultFilename");
        }

        [Fact]
        public void Throws_On_Null_Request()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(request: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("request");
        }
    }
}
