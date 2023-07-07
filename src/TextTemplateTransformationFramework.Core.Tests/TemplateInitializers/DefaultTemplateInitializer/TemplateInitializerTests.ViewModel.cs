namespace TextTemplateTransformationFramework.Core.Tests.TemplateInitializers;

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
            sut.Initialize(template, DefaultFilename, TemplateEngineMock.Object, default(object?), null, null);

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
            sut.Initialize(template, DefaultFilename, TemplateEngineMock.Object, default(object?), null, null);

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
            sut.Initialize(template, DefaultFilename, TemplateEngineMock.Object, default(object?), null, null);

            // Assert
            template.ViewModel.Should().BeNull();
        }

        [Fact]
        public void Can_Inject_ViewModel_With_No_Public_Parameterless_Constructor_On_Template_Using_AdditionalParameters()
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

        [Fact]
        public void Can_Inject_Additional_Properties_Of_Same_Type_To_ViewModel()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.TemplateWithViewModel<TestData.ViewModel>(_ => { });

            // Act
            sut.Initialize(template, DefaultFilename, TemplateEngineMock.Object, default(object?), additionalParameters: new { AdditionalParameter = "some value" }, null);

            // Assert
            template.ViewModel.Should().NotBeNull();
            template.ViewModel!.AdditionalParameter.Should().Be("some value");
        }

        [Fact]
        public void Can_Inject_Additional_Properties_Of_Different_Type_To_ViewModel()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.TemplateWithViewModel<TestData.ViewModel>(_ => { });

            // Act
            sut.Initialize(template, DefaultFilename, TemplateEngineMock.Object, default(object?), additionalParameters: new { AdditionalParameter = 1 }, null);

            // Assert
            template.ViewModel.Should().NotBeNull();
            template.ViewModel!.AdditionalParameter.Should().Be("1");
        }

        [Fact]
        public void Can_Inject_Additional_Enum_Property_Using_Int32_Value_To_ViewModel()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.TemplateWithViewModel<TestData.ViewModel>(_ => { });

            // Act
            sut.Initialize(template, DefaultFilename, TemplateEngineMock.Object, default(object?), additionalParameters: new { EnumParameter = (int)TestEnum.SecondValue }, null);

            // Assert
            template.ViewModel.Should().NotBeNull();
            template.ViewModel!.EnumParameter.Should().Be(TestEnum.SecondValue);
        }

        [Fact]
        public void Can_Inject_Null_Value_To_Additional_Property_On_ViewModel()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.TemplateWithViewModel<TestData.ViewModel>(_ => { });

            // Act
            sut.Initialize(template, DefaultFilename, TemplateEngineMock.Object, default(object?), additionalParameters: new { AdditionalParameter = (object?)null }, null);

            // Assert
            template.ViewModel.Should().NotBeNull();
            template.ViewModel!.AdditionalParameter.Should().BeNull();
        }

        [Fact]
        public void Skips_Non_Existing_Additional_Property_On_ViewModel()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.TemplateWithViewModel<TestData.ViewModel>(_ => { });

            // Act
            sut.Initialize(template, DefaultFilename, TemplateEngineMock.Object, default(object?), additionalParameters: new { WrongName = "some value" }, null);

            // Assert
            template.ViewModel.Should().NotBeNull();
            template.ViewModel!.AdditionalParameter.Should().BeEmpty();
        }

        [Fact]
        public void Skips_ReadOnly_Additional_Property_On_ViewModel()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.TemplateWithViewModel<TestData.ViewModel>(_ => { });

            // Act
            sut.Initialize(template, DefaultFilename, TemplateEngineMock.Object, default(object?), additionalParameters: new { ReadOnlyParameter = "Ignored" }, null);

            // Assert
            template.ViewModel.Should().NotBeNull();
            template.ViewModel!.ReadOnlyParameter.Should().Be("Original value");
        }

        [Fact]
        public void Can_Inject_Model_To_ViewModel()
        {
            // Arrange
            var sut = CreateSut();
            var template = new TestData.TemplateWithViewModel<TestData.ViewModelWithModel<string>>(_ => { });
            var model = "Hello world!";

            // Act
            sut.Initialize(template, DefaultFilename, TemplateEngineMock.Object, model, null, null);

            // Assert
            template.ViewModel.Should().NotBeNull();
            template.ViewModel!.Model.Should().Be(model);
        }

        [Fact]
        public void Can_Inject_TemplateContext_To_ViewModel()
        {
            // Arrange
            var sut = CreateSut();
            // Note that this template doesn't have a template context, but it can be injected into the ViewModel :)
            var template = new TestData.TemplateWithViewModel<TestData.ViewModelWithTemplateContext>(_ => { });

            // Act
            sut.Initialize(template, DefaultFilename, TemplateEngineMock.Object, default(object?), null, null);

            // Assert
            template.ViewModel.Should().NotBeNull();
            template.ViewModel!.Context.Should().NotBeNull();
        }
    }
}
