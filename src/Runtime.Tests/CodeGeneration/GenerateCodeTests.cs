using System;
using System.Diagnostics.CodeAnalysis;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using TextTemplateTransformationFramework.Runtime.CodeGeneration;
using Xunit;

namespace TextTemplateTransformationFramework.Runtime.Tests;

[ExcludeFromCodeCoverage]
public class GenerateCodeTests : TestBase
{
    [Fact]
    public void For_Throws_On_Null_Settings()
    {
        this.Invoking(_ => GenerateCode.For(null, new MultipleContentBuilder(), Fixture.Freeze<ICodeGenerationProvider>()))
            .Should().Throw<ArgumentNullException>().WithParameterName("settings");
    }

    [Fact]
    public void For_Should_Not_Throw_On_Null_MultipleContentBuilder()
    {
        // Arrange
        var providerMock = Fixture.Freeze<ICodeGenerationProvider>();
        providerMock.CreateGenerator().Returns(new GenerateCodeTests());

        // Act & Assert
        this.Invoking(_ => GenerateCode.For(new CodeGenerationSettings("UnitTest", true), null, providerMock))
            .Should().NotThrow<ArgumentNullException>();
    }

    [Fact]
    public void For_Throws_On_Null_Provider()
    {
        this.Invoking(_ => GenerateCode.For(new CodeGenerationSettings("UnitTest", true), new MultipleContentBuilder(), null))
            .Should().Throw<ArgumentNullException>().WithParameterName("provider");
    }
}
