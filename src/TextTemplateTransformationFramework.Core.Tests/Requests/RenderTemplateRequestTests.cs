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
        public void Constructs_Using_StringBuilder_1()
        {
            // Act
            this.Invoking(_ => new RenderTemplateRequest(this, builder: new StringBuilder(), string.Empty, null, null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_2()
        {
            // Act
            this.Invoking(_ => new RenderTemplateRequest(this, builder: new StringBuilder()))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!, string.Empty))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_3()
        {
            // Act
            this.Invoking(_ => new RenderTemplateRequest(this, builder: new StringBuilder(), string.Empty))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!, string.Empty, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_4()
        {
            // Act
            this.Invoking(_ => new RenderTemplateRequest(this, builder: new StringBuilder(), string.Empty, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_5()
        {
            // Act
            this.Invoking(_ => new RenderTemplateRequest(this, builder: new StringBuilder(), additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!, string.Empty, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_6()
        {
            // Act
            this.Invoking(_ => new RenderTemplateRequest(this, builder: new StringBuilder(), string.Empty, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_7()
        {
            // Act
            this.Invoking(_ => new RenderTemplateRequest(this, builder: new StringBuilder(), context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!, string.Empty, null, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: new Mock<IMultipleContentBuilder>().Object, string.Empty, null, null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: new Mock<IMultipleContentBuilder>().Object))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!, string.Empty))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: new Mock<IMultipleContentBuilder>().Object, string.Empty))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!, string.Empty, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: new Mock<IMultipleContentBuilder>().Object, string.Empty, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: new Mock<IMultipleContentBuilder>().Object, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!, string.Empty, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: new Mock<IMultipleContentBuilder>().Object, string.Empty, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builder: new Mock<IMultipleContentBuilder>().Object, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: null!, string.Empty, null, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilderContainer_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: new Mock<IMultipleContentBuilderContainer>().Object, string.Empty, null, null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilderContainer_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: new Mock<IMultipleContentBuilderContainer>().Object))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: null!, string.Empty))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilderContainer_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: new Mock<IMultipleContentBuilderContainer>().Object, string.Empty))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: null!, string.Empty, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilderContainer_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: new Mock<IMultipleContentBuilderContainer>().Object, string.Empty, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: null!, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilderContainer_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: new Mock<IMultipleContentBuilderContainer>().Object, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: null!, string.Empty, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilderContainer_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: new Mock<IMultipleContentBuilderContainer>().Object, string.Empty, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: null!, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilderContainer_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: new Mock<IMultipleContentBuilderContainer>().Object, context: null))
                .Should().NotThrow();
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
        public void Constructs_Using_StringBuilder_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: new StringBuilder(), null, string.Empty, null, null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (StringBuilder)null!, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: new StringBuilder(), null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (StringBuilder)null!, null, string.Empty))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: new StringBuilder(), null, string.Empty))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (StringBuilder)null!, null, string.Empty, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: new StringBuilder(), null, string.Empty, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (StringBuilder)null!, null, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: new StringBuilder(), null, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (StringBuilder)null!, null, string.Empty, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: new StringBuilder(), null, string.Empty, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_StringBuilder_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (StringBuilder)null!, null, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_StringBuilder_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: new StringBuilder(), null, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (IMultipleContentBuilder)null!, null, string.Empty, null, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: new Mock<IMultipleContentBuilder>().Object, null, string.Empty, null, null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (IMultipleContentBuilder)null!, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: new Mock<IMultipleContentBuilder>().Object, null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (IMultipleContentBuilder)null!, null, string.Empty))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: new Mock<IMultipleContentBuilder>().Object, null, string.Empty))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (IMultipleContentBuilder)null!, null, string.Empty, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: new Mock<IMultipleContentBuilder>().Object, null, string.Empty, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (IMultipleContentBuilder)null!, null, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: new Mock<IMultipleContentBuilder>().Object, null, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (IMultipleContentBuilder)null!, null, string.Empty, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: new Mock<IMultipleContentBuilder>().Object, null, string.Empty, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilder_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: (IMultipleContentBuilder)null!, null, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builder");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilder_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builder: new Mock<IMultipleContentBuilder>().Object, null, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builderContainer: null!, null, string.Empty, null, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilderContainer_1()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builderContainer: new Mock<IMultipleContentBuilderContainer>().Object, null, string.Empty, null, null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builderContainer: null!, null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilderContainer_2()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builderContainer: new Mock<IMultipleContentBuilderContainer>().Object, null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builderContainer: null!, null, string.Empty))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilderContainer_3()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builderContainer: new Mock<IMultipleContentBuilderContainer>().Object, null, string.Empty))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builderContainer: null!, null, string.Empty, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilderContainer_4()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builderContainer: new Mock<IMultipleContentBuilderContainer>().Object, null, string.Empty, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builderContainer: null!, null, additionalParameters: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilderContainer_5()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builderContainer: new Mock<IMultipleContentBuilderContainer>().Object, null, additionalParameters: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builderContainer: null!, null, string.Empty, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilderContainer_6()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builderContainer: new Mock<IMultipleContentBuilderContainer>().Object, null, string.Empty, context: null))
                .Should().NotThrow();
        }

        [Fact]
        public void Throws_On_Null_MultipleContentBuilderContainer_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builderContainer: null!, null, context: null))
                .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
        }

        [Fact]
        public void Constructs_Using_MultipleContentBuilderContainer_7()
        {
            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(this, builderContainer: new Mock<IMultipleContentBuilderContainer>().Object, null, context: null))
                .Should().NotThrow();
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

        [Fact]
        public void Constructs_Using_Request()
        {
            // Arrange
            var requestMock = new Mock<IRenderTemplateRequest>();
            requestMock.SetupGet(x => x.DefaultFilename).Returns(string.Empty);
            requestMock.SetupGet(x => x.GenerationEnvironment).Returns(new Mock<IGenerationEnvironment>().Object);
            requestMock.SetupGet(x => x.Template).Returns(this);

            // Act & Assert
            this.Invoking(_ => new RenderTemplateRequest<object?>(request: requestMock.Object))
                .Should().NotThrow();
        }
    }
}
