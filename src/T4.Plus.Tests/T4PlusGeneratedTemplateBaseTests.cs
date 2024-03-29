﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using FluentAssertions;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Runtime;
using TextTemplateTransformationFramework.T4.Plus.CodeGenerators;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Plus.Tests
{
    [ExcludeFromCodeCoverage]
    public class T4PlusGeneratedTemplateBaseTests
    {
        [Fact]
        public void ErrorGetsCreatedWhenTemplateInstanciationDelegateReturnsNullAndSilentlyContinueOnErrorIsFalse()
        {
            // Arrange
            var sut = new T4PlusGeneratedTemplateBaseMock();
            sut.RegisterChildTemplate("test", () => null);

            // Act
            var actual = sut.GetChildTemplateByName("test");

            // Assert
            actual.Should().BeNull();
            sut.Errors.Should().HaveCount(1);
            sut.Errors[0].ErrorText.Should().Be("Child template [test] did not instanciate");
        }

        [Fact]
        public void ErrorDoesNotGetCreatedWhenTemplateInstanciationDelegateReturnsNullAndSilentlyContinueOnErrorIsTrue()
        {
            // Arrange
            var sut = new T4PlusGeneratedTemplateBaseMock();
            sut.RegisterChildTemplate("test", () => null);

            // Act
            var actual = sut.GetChildTemplateByName("test", true);

            // Assert
            actual.Should().BeNull();
            sut.Errors.Should().BeEmpty();
        }

        [Fact]
        public void WarningOnChildTemplateLeadsToWarning()
        {
            // Arrange
            var sut = new T4PlusGeneratedTemplateBaseMock();
            sut.RegisterChildTemplate("test", () => new ChildTemplateWithWarning());

            // Act
            sut.RenderChildTemplate("test");

            // Assert
            sut.Errors.Should().HaveCount(1);
            sut.Errors[0].IsWarning.Should().BeTrue();
            sut.Errors[0].ErrorText.Should().Be("Warning");
        }

        [Fact]
        public void RootContextReturnsRootContextInCaseOfChildTemplate()
        {
            // Arrange
            var sut = TemplateInstanceContext.CreateRootContext(new T4PlusGeneratedTemplateBaseMock()).CreateChildContext(this, null);

            // Act
            var actual = sut.RootContext.Template.GetType().Name;

            // Assert
            actual.Should().Be("T4PlusGeneratedTemplateBaseMock");
        }

        [Fact]
        public void GetContextByTypeReturnsNullWhenNotFound()
        {
            // Arrange
            var sut = TemplateInstanceContext.CreateRootContext(new T4PlusGeneratedTemplateBaseMock()).CreateChildContext(this, null);

            // Act
            var actual = sut.GetContextByType<UnknownTemplate>();

            // Assert
            actual.Should().BeNull();
        }

        [Fact]
        public void GetModelFromContextByTypeReturnsNullWhenNotFound()
        {
            // Arrange
            var sut = TemplateInstanceContext.CreateRootContext(new T4PlusGeneratedTemplateBaseMock()).CreateChildContext(this, null);

            // Act
            var actual = sut.GetModelFromContextByType<string>();

            // Assert
            actual.Should().BeNull();
        }

        [Fact]
        public void WriteWithParamsUsesStringFormat()
        {
            // Arrange
            var sut = new WriteWithParamsTemplate();
            var sb = new StringBuilder();

            // Act
            sut.Render(sb);
            var actual = sb.ToString();

            // Assert
            actual.Should().Be("Hello world");
        }

        [Fact]
        public void WriteLineWithParamsUsesStringFormat()
        {
            // Arrange
            var sut = new WriteLineWithParamsTemplate();
            var sb = new StringBuilder();

            // Act
            sut.Render(sb);
            var actual = sb.ToString();

            // Assert
            actual.Should().Be(@"Hello world
");
        }

        [Fact]
        public void ClearIndentClearsTheIndentation()
        {
            // Arrange
            var sut = new ClearIndentationTemplate();
            var sb = new StringBuilder();

            // Act
            sut.Render(sb);
            var actual = sb.ToString();

            // Assert
            actual.Should().Be(@"    Hello 1
    Hello 2
Hello 3
Hello 4
");
        }

        [Fact]
        public void CanIncludeTemplateWithOneLiner()
        {
            var sut = new CompositeTemplate2();
            var actual1 = sut.TransformText();

            var compare = new T4PlusCSharpCodeGenerator();
            var actual2 = TemplateRenderHelper.GetTemplateOutput(compare, Array.Empty<ITemplateToken<TokenParserState>>());

            actual1.Should().Be(actual2);
        }

        [Fact]
        public void ResolveWorksOnHierarchy()
        {
            // Arrange
            var sut = new T4PlusGeneratedTemplateBaseMock();
            sut.RegisterChildTemplate("BaseClass", () => new BaseClass(), typeof(BaseClass));
            sut.RegisterChildTemplate("FinalClass", () => new FinalClass(), typeof(FinalClass));

            // Act
            var baseClass = sut.GetChildTemplateByModelType(new BaseClass());
            var finalClass = sut.GetChildTemplateByModelType(new FinalClass());

            // Assert
            baseClass.Should().BeOfType<BaseClass>("BaseClass is not resolved correctly");
            finalClass.Should().BeOfType<FinalClass>("FinalClass is not resolved correctly");
        }

        [Fact]
        public void CanRenderTemplateWithPlaceholder()
        {
            // Arrange
            var sut = new Placeholder.GeneratedTemplate();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut);

            // Assert
            actual.Should().Be("Hello world!");
        }

        [Fact]
        public void AddingNonExistingTemplateToPlaceholderGivesError()
        {
            // Arrange
            var sut = new Placeholder.GeneratedTemplate();
            var builder = new StringBuilder();

            // Act
            sut.Initialize();
            sut.AddTemplateToPlaceholder("MyPlaceholder", "NonExistingChildTemplate");
            sut.Render(builder);

            // Assert
            sut.Errors.Should().ContainSingle();
            sut.Errors.First().ErrorText.Should().Be("Could not resolve child template with name NonExistingChildTemplate");
        }

        [Fact]
        public void CanRenderTemplateWithSeparator()
        {
            // Arrange
            var sut = new TemplateThatUsesSeparator();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut);

            // Assert
            actual.Should().Be("Test,Test,Test");
        }
    }

    internal sealed class TemplateThatUsesSeparator : T4PlusGeneratedTemplateBase
    {
        public void Initialize()
        {
            RegisterChildTemplate("Test", () => new SimpleTemplate("Test"), typeof(string));
            RegisterChildTemplate("Separator", () => new SimpleTemplate(","));
        }
        public void Render(StringBuilder builder)
        {
            GenerationEnvironment = builder;
            RenderChildTemplate("Test", Enumerable.Range(1, 3).Select(_ => string.Empty), separatorTemplateName: "Separator");
        }
    }

    [ExcludeFromCodeCoverage]
    internal class BaseClass
    {
    }

    [ExcludeFromCodeCoverage]
    internal sealed class FinalClass : BaseClass
    {
    }

    [ExcludeFromCodeCoverage]
    internal sealed class CompositeTemplate2 : T4PlusGeneratedTemplateBase
    {
        public string TransformText()
        {
            var t = new T4PlusCSharpCodeGenerator();
            TemplateRenderHelper.SetModelOnTemplate(t, Array.Empty<ITemplateToken<TokenParserState>>());
            TemplateRenderHelper.RenderTemplate(t, GenerationEnvironment, Session);
            return GenerationEnvironment.ToString();
        }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class T4PlusGeneratedTemplateBaseMock : T4PlusGeneratedTemplateBase
    {
        public object GetChildTemplateByName(string templateName, bool silentlyContinueOnError = false)
        {
            return GetChildTemplate(templateName, null, silentlyContinueOnError);
        }

        public object GetChildTemplateByModelType(object model, bool silentlyContinueOnError = false)
        {
            return GetChildTemplate(null, model, silentlyContinueOnError);
        }

        public new void RegisterChildTemplate(string templateName, Func<object> templateDelegate, Type modelType = null)
        {
            base.RegisterChildTemplate(templateName, templateDelegate, modelType);
        }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class ChildTemplateWithWarning : T4PlusGeneratedTemplateBase
    {
        public ChildTemplateWithWarning()
        {
            Warning("Warning");
        }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class UnknownTemplate : T4PlusGeneratedTemplateBase
    {
    }

    [ExcludeFromCodeCoverage]
    internal sealed class WriteWithParamsTemplate : T4PlusGeneratedTemplateBase
    {
        public void Render(StringBuilder builder)
        {
            GenerationEnvironment = builder;
            Write("{0} {1}", "Hello", "world");
        }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class WriteLineWithParamsTemplate : T4PlusGeneratedTemplateBase
    {
        public void Render(StringBuilder builder)
        {
            GenerationEnvironment = builder;
            WriteLine("{0} {1}", "Hello", "world");
        }
    }

    [ExcludeFromCodeCoverage]
    internal sealed class ClearIndentationTemplate : T4PlusGeneratedTemplateBase
    {
        public void Render(StringBuilder builder)
        {
            GenerationEnvironment = builder;
            PushIndent("    ");
            WriteLine("Hello 1");
            WriteLine("Hello 2");
            ClearIndent();
            WriteLine("Hello 3");
            WriteLine("Hello 4");
        }
    }

    [ExcludeFromCodeCoverage]
    public sealed class MyData
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public sealed class SimpleTemplate
    {
        private readonly string _contents;

        public SimpleTemplate(string contents)
        {
            _contents = contents;
        }

        public override string ToString() => _contents;
    }
}
