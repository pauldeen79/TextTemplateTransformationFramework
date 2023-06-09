using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using TextTemplateTransformationFramework.Runtime.CodeGeneration;
using Xunit;

namespace TextTemplateTransformationFramework.Runtime.Tests;

[ExcludeFromCodeCoverage]
public class GenerateCodeTests
{
    [Fact]
    public void For_Throws_On_Null_Settings()
    {
        this.Invoking(_ => GenerateCode.For(null, new MultipleContentBuilder(), new Mock<ICodeGenerationProvider>().Object))
            .Should().Throw<ArgumentNullException>().WithParameterName("settings");
    }

    [Fact]
    public void For_Should_Not_Throw_On_Null_MultipleContentBuilder()
    {
        // Arrange
        var providerMock = new Mock<ICodeGenerationProvider>();
        providerMock.Setup(x => x.CreateGenerator()).Returns(new GenerateCodeTests());

        // Act & Assert
        this.Invoking(_ => GenerateCode.For(new CodeGenerationSettings("UnitTest", true), null, providerMock.Object))
            .Should().NotThrow<ArgumentNullException>();
    }

    [Fact]
    public void For_Throws_On_Null_Provider()
    {
        this.Invoking(_ => GenerateCode.For(new CodeGenerationSettings("UnitTest", true), new MultipleContentBuilder(), null))
            .Should().Throw<ArgumentNullException>().WithParameterName("provider");
    }
}
