using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using FluentAssertions;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Runtime;
using TextTemplateTransformationFramework.T4.CodeGenerators;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Tests
{
    [ExcludeFromCodeCoverage]
    public class T4CSharpCodeGeneratorTests
    {
        [Fact]
        public void Initialize_Validates_GeneratorName_Session_Parameter()
        {
            // Arrange
            var sut = new T4CSharpCodeGenerator
            {
                Session = new Dictionary<string, object>
                {
                    { nameof(T4CSharpCodeGenerator.GeneratorName), 1 },
                }
            };

            // Act
            sut.Initialize();

            // Assert
            sut.Errors.Where(e => !e.IsWarning).Should().HaveCount(1);
        }

        [Fact]
        public void Initialize_Validates_GeneratorVersion_Session_Parameter()
        {
            // Arrange
            var sut = new T4CSharpCodeGenerator
            {
                Session = new Dictionary<string, object>
                {
                    { nameof(T4CSharpCodeGenerator.GeneratorVersion), 1 },
                }
            };

            // Act
            sut.Initialize();

            // Assert
            sut.Errors.Where(e => !e.IsWarning).Should().HaveCount(1);
        }

        [Fact]
        public void Initialize_Validates_CultureCode_Session_Parameter()
        {
            // Arrange
            var sut = new T4CSharpCodeGenerator
            {
                Session = new Dictionary<string, object>
                {
                    { nameof(T4CSharpCodeGenerator.CultureCode), 1 },
                }
            };

            // Act
            sut.Initialize();

            // Assert
            sut.Errors.Where(e => !e.IsWarning).Should().HaveCount(1);
        }

        [Fact]
        public void Initialize_Validates_TemplateNamespace_Session_Parameter()
        {
            // Arrange
            var sut = new T4CSharpCodeGenerator
            {
                Session = new Dictionary<string, object>
                {
                    { nameof(T4CSharpCodeGenerator.TemplateNamespace), 1 },
                }
            };

            // Act
            sut.Initialize();

            // Assert
            sut.Errors.Where(e => !e.IsWarning).Should().HaveCount(1);
        }

        [Fact]
        public void Initialize_Validates_TemplateClassName_Session_Parameter()
        {
            // Arrange
            var sut = new T4CSharpCodeGenerator
            {
                Session = new Dictionary<string, object>
                {
                    { nameof(T4CSharpCodeGenerator.TemplateClassName), 1 },
                }
            };

            // Act
            sut.Initialize();

            // Assert
            sut.Errors.Where(e => !e.IsWarning).Should().HaveCount(1);
        }

        [Fact]
        public void Initialize_Validates_TemplateIsOverride_Session_Parameter()
        {
            // Arrange
            var sut = new T4CSharpCodeGenerator
            {
                Session = new Dictionary<string, object>
                {
                    { nameof(T4CSharpCodeGenerator.TemplateIsOverride), 1 }
                }
            };

            // Act
            sut.Initialize();

            // Assert
            sut.Errors.Where(e => !e.IsWarning).Should().HaveCount(1);
        }

        [Fact]
        public void Initialize_Validates_Tokens_Session_Parameter()
        {
            // Arrange
            var sut = new T4CSharpCodeGenerator
            {
                Session = new Dictionary<string, object>
                {
                    { nameof(T4CSharpCodeGenerator.Model), 1 },
                }
            };

            // Act
            sut.Initialize();

            // Assert
            sut.Errors.Where(e => !e.IsWarning).Should().HaveCount(1);
        }

        [Fact]
        public void Initialize_Validates_SkipInitializationCode_Session_Parameter()
        {
            // Arrange
            var sut = new T4CSharpCodeGenerator
            {
                Session = new Dictionary<string, object>
                {
                    { nameof(T4CSharpCodeGenerator.SkipInitializationCode), 1 },
                }
            };

            // Act
            sut.Initialize();

            // Assert
            sut.Errors.Where(e => !e.IsWarning).Should().HaveCount(1);
        }

        [Fact]
        public void Initialize_Validates_BaseClassInheritsFrom_Session_Parameter()
        {
            // Arrange
            var sut = new T4CSharpCodeGenerator
            {
                Session = new Dictionary<string, object>
                {
                    { nameof(T4CSharpCodeGenerator.BaseClassInheritsFrom), 1 },
                }
            };

            // Act
            sut.Initialize();

            // Assert
            sut.Errors.Where(e => !e.IsWarning).Should().HaveCount(1);
        }

        [Fact]
        public void Can_Run_T4SharpCodeGenerator_Using_TemplateRenderer_With_Builder()
        {
            // Arrange
            var sb = new StringBuilder();

            // Act
            var errors = TemplateRenderHelper.GetCompilerErrors(new T4CSharpCodeGenerator(), sb, Array.Empty<ITemplateToken<TokenParserState>>());

            // Assert
            errors.Where(e => !e.IsWarning).Should().BeEmpty();
            sb.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public void Can_Run_T4SharpCodeGenerator_Using_TemplateRenderer_Without_Builder()
        {
            // Act
            var output = TemplateRenderHelper.GetTemplateOutput(new T4CSharpCodeGenerator(), Array.Empty<ITemplateToken<TokenParserState>>());

            // Assert
            output.Should().NotBeEmpty();
        }
    }
}
