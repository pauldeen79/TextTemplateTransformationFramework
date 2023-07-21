namespace TemplateFramework.Core.Tests.Requests;

public class RenderTemplateRequestTests
{
    [Fact]
    public void Throws_On_Null_StringBuilder()
    {
        // Act & Assert
        this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!))
            .Should().Throw<ArgumentNullException>().WithParameterName("builder");
    }

    [Fact]
    public void Throws_On_Null_MultipleContentBuilder()
    {
        // Act & Assert
        this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!))
            .Should().Throw<ArgumentNullException>().WithParameterName("builder");
    }

    [Fact]
    public void Throws_On_Null_MultipleContentBuilderContainer()
    {
        // Act & Assert
        this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: null!))
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
