namespace TextTemplateTransformationFramework.Core.Tests;

public partial class TemplateInitializerTests
{
    public class ViewModel : TemplateInitializerTests
    {
        [Fact]
        public void Sets_ViewModel_On_Template_When_Possible()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.TemplateWithViewModel<TestData.ViewModel>(_ => { });

            // Act
            sut.Initialize(template, DefaultFilename, default(object?), null, null);

            // Assert
            template.ViewModel.Should().NotBeNull();
        }

        [Fact]
        public void Does_Not_Overwrite_Existing_ViewModel_On_Template_When_Already_Initialized()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.TemplateWithViewModel<TestData.ViewModel>(_ => { });
            var viewModel = new TestData.ViewModel();
            template.ViewModel = viewModel;

            // Act
            sut.Initialize(template, DefaultFilename, default(object?), null, null);

            // Assert
            template.ViewModel.Should().BeSameAs(viewModel);
        }

        [Fact]
        public void Does_Not_Set_ViewModel_On_Template_When_Not_Already_Initialized_And_No_Public_Parameterless_Constructor_Available()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.TemplateWithViewModel<TestData.NonConstructableViewModel>(_ => { });

            // Act
            sut.Initialize(template, DefaultFilename, default(object?), null, null);

            // Assert
            template.ViewModel.Should().BeNull();
        }
    }
}
