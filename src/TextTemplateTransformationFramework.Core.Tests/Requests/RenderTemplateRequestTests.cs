namespace TemplateFramework.Core.Tests.Requests;

public class RenderTemplateRequestTests
{
    [Fact]
    public void Throws_On_Null_StringBuilder()
    {
        // Act & Assert
        this.Invoking(_ => new RenderTemplateRequest(this, builder: (StringBuilder)null!, string.Empty, null, null))
            .Should().Throw<ArgumentNullException>().WithParameterName("builder");
    }

    [Fact]
    public void Throws_On_Null_MultipleContentBuilder()
    {
        // Act & Assert
        this.Invoking(_ => new RenderTemplateRequest(this, builder: (IMultipleContentBuilder)null!, string.Empty, null, null))
            .Should().Throw<ArgumentNullException>().WithParameterName("builder");
    }

    [Fact]
    public void Throws_On_Null_MultipleContentBuilderContainer()
    {
        // Act & Assert
        this.Invoking(_ => new RenderTemplateRequest(this, builderContainer: null!, string.Empty, null, null))
            .Should().Throw<ArgumentNullException>().WithParameterName("builderContainer");
    }

    [Fact]
    public void Throws_On_Null_Template()
    {
        // Act & Assert
        this.Invoking(_ => new RenderTemplateRequest(template: null!, new StringBuilder(), string.Empty, null, null))
            .Should().Throw<ArgumentNullException>().WithParameterName("template");
    }

    [Fact]
    public void Throws_On_Null_DefaultFileName()
    {
        // Act & Assert
        this.Invoking(_ => new RenderTemplateRequest(this, new StringBuilder(), defaultFilename: null!, null, null))
            .Should().Throw<ArgumentNullException>().WithParameterName("defaultFilename");
    }
}
