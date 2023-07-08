namespace TextTemplateTransformationFramework.Core.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void All_Dependencies_Can_Be_Resolved()
    {
        // Act
        using var provider = new ServiceCollection()
            .AddTemplateFramework()
            .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

        // Assert
        provider.Should().NotBeNull();
    }
}
