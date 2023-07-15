namespace TemplateFramework.Core.Tests.TemplateInitializers;

public partial class TemplateInitializerTests
{
    public class ViewModel : TemplateInitializerTests
    {
        [Fact]
        public void Can_Inject_ViewModel_On_Template_Using_AdditionalParameters()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.TemplateWithViewModel<TestData.NonConstructableViewModel>(_ => { });
            var viewModel = new TestData.NonConstructableViewModel("Some value");

            // Act
            sut.Initialize(template, DefaultFilename, TemplateEngineMock.Object, default(object?), additionalParameters: new { ViewModel = viewModel }, null);

            // Assert
            template.ViewModel.Should().BeSameAs(viewModel);
        }
    }
}
