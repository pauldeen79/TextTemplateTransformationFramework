﻿using System;
using System.Linq;
using CrossCutting.Common.Testing;
using FluentAssertions;
using Moq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.Default
{
    public class TemplateInfoRepositoryTests
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
            TestHelpers.ConstructorMustThrowArgumentNullException(typeof(TemplateInfoRepository));
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
    }
}
