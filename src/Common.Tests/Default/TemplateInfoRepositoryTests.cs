using System;
using System.Linq;
using FluentAssertions;
using Moq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.Default
{
    public class TemplateInfoRepositoryTests : TestBase
    {
        private readonly Mock<IFileContentsProvider> _fileContentsProviderMock = new();
        private string _contents = "";

        public TemplateInfoRepositoryTests()
        {
            _fileContentsProviderMock.Setup(x => x.DirectoryExists(It.IsAny<string>())).Returns(true);
            _fileContentsProviderMock.Setup(x => x.FileExists(It.IsAny<string>())).Returns(() => !string.IsNullOrEmpty(_contents));
            _fileContentsProviderMock.Setup(x => x.GetFileContents(It.IsAny<string>())).Returns(() => _contents);
            _fileContentsProviderMock.Setup(x => x.WriteFileContents(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>((path, contents) => _contents = contents);
        }

        [Fact]
        public void Ctor_Throws_On_Null_Arguments()
        {
            ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(typeof(TemplateInfoRepository));
        }

        [Fact]
        public void Add_Throws_On_Null_Template()
        {
            // Arrange
            var sut = new TemplateInfoRepository(_fileContentsProviderMock.Object);

            // Act
            sut.Invoking(x => x.Add(null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Update_Throws_On_Null_Template()
        {
            // Arrange
            var sut = new TemplateInfoRepository(_fileContentsProviderMock.Object);

            // Act
            sut.Invoking(x => x.Update(null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Remove_Throws_On_Null_Template()
        {
            // Arrange
            var sut = new TemplateInfoRepository(_fileContentsProviderMock.Object);

            // Act
            sut.Invoking(x => x.Remove(null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Add_Throws_On_Duplicate_Template()
        {
            // Arrange
            var sut = new TemplateInfoRepository(_fileContentsProviderMock.Object);
            var templateInfo = new TemplateInfo("MyTemplate", "my.template", "", "", TemplateType.TextTemplate, new[] { new TemplateParameter { Name = "MyParameter", Value = "123" } });
            sut.Add(templateInfo);

            // Act & Assert
            sut.Invoking(x => x.Add(templateInfo)).Should().Throw<ArgumentOutOfRangeException>().WithMessage("Template already exists (Parameter 'templateInfo')");
        }

        [Fact]
        public void Update_Throws_On_Non_Existing_Template()
        {
            // Arrange
            var sut = new TemplateInfoRepository(_fileContentsProviderMock.Object);

            // Act
            sut.Invoking(x => x.Update(new TemplateInfo("", "", "", "", TemplateType.TextTemplate, Array.Empty<TemplateParameter>())))
               .Should().Throw<ArgumentOutOfRangeException>().WithMessage("Template was not found (Parameter 'templateInfo')");
        }

        [Fact]
        public void Remove_Throws_On_Non_Existing_Template()
        {
            // Arrange
            var sut = new TemplateInfoRepository(_fileContentsProviderMock.Object);

            // Act
            sut.Invoking(x => x.Remove(new TemplateInfo("", "", "", "", TemplateType.TextTemplate, Array.Empty<TemplateParameter>())))
               .Should().Throw<ArgumentOutOfRangeException>().WithMessage("Template was not found (Parameter 'templateInfo')");
        }

        [Fact]
        public void Add_Adds_The_Specified_Template_Corectly()
        {
            // Arrange
            var sut = new TemplateInfoRepository(_fileContentsProviderMock.Object);
            var templateInfo = new TemplateInfo("MyTemplate", "my.template", "", "", TemplateType.TextTemplate, new[] { new TemplateParameter { Name = "MyParameter", Value = "123" } });

            // Act
            sut.Add(templateInfo);

            // Assert
            _contents.Should().StartWith(@"{""Templates"":[{""ShortName"":""MyTemplate"",""FileName"":""my.template"",""AssemblyName"":"""",""ClassName"":"""",""Type"":""TextTemplate"",""Parameters"":[{""Name"":""MyParameter"",""Type"":null,""EditorAttributeEditorTypeName"":null,""EditorAttributeEditorBaseTypeName"":null,""TypeConverterTypeName"":null,""Value"":""123"",""OmitValueAssignment"":false,""Description"":null,""DisplayName"":null,""Browsable"":false,""ReadOnly"":false,""PossibleValues"":null}]}]}");
        }

        [Fact]
        public void Add_Creates_Directory_When_It_Does_Not_Exist_Yet()
        {
            // Arrange
            _fileContentsProviderMock.Setup(x => x.DirectoryExists(It.IsAny<string>())).Returns(false);
            var sut = new TemplateInfoRepository(_fileContentsProviderMock.Object);
            var templateInfo = new TemplateInfo("MyTemplate", "my.template", "", "", TemplateType.TextTemplate, new[] { new TemplateParameter { Name = "MyParameter", Value = "123" } });

            // Act
            sut.Add(templateInfo);

            // Assert
            _fileContentsProviderMock.Verify(x => x.CreateDirectory(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Update_Updates_The_Specified_Template_Correctly()
        {
            // Arrange
            var sut = new TemplateInfoRepository(_fileContentsProviderMock.Object);
            var templateInfo = new TemplateInfo("MyTemplate", "my.template", "", "", TemplateType.TextTemplate, new[] { new TemplateParameter { Name = "MyParameter", Value = "123" } });
            sut.Add(templateInfo);

            // Act
            sut.Update(templateInfo.Update("updated", Array.Empty<TemplateParameter>()));

            // Assert
            _contents.Should().StartWith(@"{""Templates"":[{""ShortName"":""updated"",""FileName"":""my.template"",""AssemblyName"":"""",""ClassName"":"""",""Type"":""TextTemplate"",""Parameters"":[]}]}");
        }

        [Fact]
        public void Remove_Removes_The_Specified_Template_Correctly()
        {
            // Arrange
            var sut = new TemplateInfoRepository(_fileContentsProviderMock.Object);
            var templateInfo = new TemplateInfo("MyTemplate", "my.template", "", "", TemplateType.TextTemplate, new[] { new TemplateParameter { Name = "MyParameter", Value = "123" } });
            sut.Add(templateInfo);

            // Act
            sut.Remove(templateInfo);

            // Assert
            _contents.Should().StartWith(@"{""Templates"":[]}");
        }

        [Fact]
        public void GetTemplates_Returns_Empty_Result_When_Contents_Is_Empty()
        {
            // Arrange
            var sut = new TemplateInfoRepository(_fileContentsProviderMock.Object);

            // Act
            var actual = sut.GetTemplates().ToArray();

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public void GetTemplates_Returns_Correct_Result_When_Contents_Is_Not_Empty()
        {
            // Arrange
            var sut = new TemplateInfoRepository(_fileContentsProviderMock.Object);
            var templateInfo = new TemplateInfo("MyTemplate", "my.template", "", "", TemplateType.TextTemplate, new[] { new TemplateParameter { Name = "MyParameter", Value = "123" } });
            sut.Add(templateInfo);

            // Act
            var actual = sut.GetTemplates().ToArray();

            // Assert
            actual.Should().BeEquivalentTo(new[] { templateInfo });
        }

        [Fact]
        public void FindByShortName_Throws_On_Empty_ShortName()
        {
            // Arrange
            var sut = new TemplateInfoRepository(_fileContentsProviderMock.Object);

            // Act & Assert
            sut.Invoking(x => x.FindByShortName(null)).Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void FindByShortName_Returns_Null_When_Not_Found()
        {
            // Arrange
            var sut = new TemplateInfoRepository(_fileContentsProviderMock.Object);
            var templateInfo = new TemplateInfo("MyTemplate", "my.template", "", "", TemplateType.TextTemplate, new[] { new TemplateParameter { Name = "MyParameter", Value = "123" } });
            sut.Add(templateInfo);

            // Act
            var actual = sut.FindByShortName("nonexisting");

            // Assert
            actual.Should().BeNull();
        }

        [Fact]
        public void FindByShortName_Returns_TemplateInfo_When_Found()
        {
            // Arrange
            var sut = new TemplateInfoRepository(_fileContentsProviderMock.Object);
            var templateInfo = new TemplateInfo("MyTemplate", "my.template", "", "", TemplateType.TextTemplate, new[] { new TemplateParameter { Name = "MyParameter", Value = "123" } });
            sut.Add(templateInfo);

            // Act
            var actual = sut.FindByShortName("MyTemplate");

            // Assert
            actual.Should().NotBeNull();
        }
    }
}
