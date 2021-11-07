using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using FluentAssertions;
using Moq;
using TextTemplateTransformationFramework.Runtime;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Plus.Tests
{
    [ExcludeFromCodeCoverage]
    public class TemplateRenderHelperTests
    {
        [Fact]
        public void CanRenderPlainTemplateAsCompilerErrorCollection()
        {
            // Arrange
            var template = new MyPlainTemplate();
            var builder = new StringBuilder();

            // Act
            var actual = TemplateRenderHelper.GetCompilerErrors(template, builder);

            // Assert
            builder.ToString().Should().Be("Hello world!");
            actual.Should().BeEmpty();
        }

        [Fact]
        public void CanRenderErrorTemplateAsCompilerErrorCollection()
        {
            // Arrange
            var template = new MyErrorTemplate();
            var builder = new StringBuilder();

            // Act
            var actual = TemplateRenderHelper.GetCompilerErrors(template, builder, (object)null).ToArray();

            // Assert
            builder.ToString().Should().BeEmpty();
            actual.Should().HaveCount(1);
            actual[0].ErrorText.Should().Be("Something is wrong!");
        }

        [Fact]
        public void CanRenderModeledTemplateAsCompilerErrorCollection()
        {
            // Arrange
            var template = new MyModeledTemplate();
            var builder = new StringBuilder();

            // Act
            var actual = TemplateRenderHelper.GetCompilerErrors(template, builder, "Hello world...");

            // Assert
            builder.ToString().Should().Be("Hello world...");
            actual.Should().BeEmpty();
        }

        [Fact]
        public void CanRenderTemplateWithAdditionalParametersAsCompilerErrorCollectionUsingDictionary()
        {
            // Arrange
            var template = new MyTemplateWithAdditionalParameters();
            var builder = new StringBuilder();

            // Act
            var actual = TemplateRenderHelper.GetCompilerErrors(template, builder, additionalParameters: new[] { new KeyValuePair<string, object>("AdditionalParameter1", "Hello"), new KeyValuePair<string, object>("AdditionalParameter2", "World") });

            // Assert
            builder.ToString().Should().Be("HelloWorld");
            actual.Should().BeEmpty();
        }

        [Fact]
        public void CanRenderTemplateWithAdditionalParametersAsCompilerErrorCollectionUsingObject()
        {
            // Arrange
            var template = new MyTemplateWithAdditionalParameters();
            var builder = new StringBuilder();

            // Act
            var actual = TemplateRenderHelper.GetCompilerErrors(template, builder, null, additionalParameters: new { AdditionalParameter1 = "Hello", AdditionalParameter2 = "World" });

            // Assert
            builder.ToString().Should().Be("HelloWorld");
            actual.Should().BeEmpty();
        }

        [Fact]
        public void CanRenderPlainTemplateAsString()
        {
            // Arrange
            var template = new MyPlainTemplate();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(template, null);

            // Assert
            actual.Should().Be("Hello world!");
        }

        [Fact]
        public void CanRenderPlainTemplateWithPreRenderDelegate()
        {
            // Arrange
            var template = new MyPlainTemplate();
            bool executed = false;

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(template, null, preRenderDelegate: new Action(() => executed = true));

            // Assert
            actual.Should().Be("Hello world!");
            executed.Should().BeTrue();
        }

        [Fact]
        public void CanRenderErrorTemplateAsString()
        {
            // Arrange
            var template = new MyErrorTemplate();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(template, null);

            // Assert
            actual.Should().Be("Error: Something is wrong!");
        }

        [Fact]
        public void CanRenderModeledTemplateAsString()
        {
            // Arrange
            var template = new MyModeledTemplate();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(template, "Hello modeled world!");

            // Assert
            actual.Should().Be("Hello modeled world!");
        }

        [Fact]
        public void CanRenderTemplateWithAdditionalParametersStringUsingDictionary()
        {
            // Arrange
            var template = new MyTemplateWithAdditionalParameters();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(template, null, additionalParameters: new[] { new KeyValuePair<string, object>("AdditionalParameter1", "Hello"), new KeyValuePair<string, object>("AdditionalParameter2", "World") });

            // Assert
            actual.Should().Be("HelloWorld");
        }

        [Fact]
        public void CanRenderTemplateWithAdditionalParametersStringUsingObject()
        {
            // Arrange
            var template = new MyTemplateWithAdditionalParameters();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(template, null, additionalParameters: new { AdditionalParameter1 = "Hello", AdditionalParameter2 = "World" });

            // Assert
            actual.Should().Be("HelloWorld");
        }

        [Fact]
        public void CanRenderPlainTemplateUsingStringBuilder()
        {
            // Arrange
            var template = new MyPlainTemplate();
            var builder = new StringBuilder();

            // Act
            TemplateRenderHelper.RenderTemplate(template, builder);

            // Assert
            builder.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void CanRenderErrorTemplateUsingStringBuilder()
        {
            // Arrange
            var template = new MyErrorTemplate();
            var builder = new StringBuilder();

            // Act
            this.Invoking(_ => TemplateRenderHelper.RenderTemplate(template, builder)).Should().Throw<AggregateException>();
        }

        [Fact]
        public void CanRenderModeledTemplateUsingStringBuilder()
        {
            // Arrange
            var template = new MyModeledTemplate();
            var builder = new StringBuilder();
            template.Model = "Hello world!";

            // Act
            TemplateRenderHelper.RenderTemplate(template, builder); //note that this is the same as the plain template; because initialization is not part of this method, you have to set the Model yourself (see Arrange above)

            // Assert
            builder.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void CanRenderTemplateWithAdditionalParametersUsingStringBuilder()
        {
            // Arrange
            var template = new MyTemplateWithAdditionalParameters();
            var builder = new StringBuilder();
            template.AdditionalParameter1 = "Hello";
            template.AdditionalParameter2 = "World";

            // Act
            TemplateRenderHelper.RenderTemplate(template, builder); //note that this is the same as the plain template; because initialization is not part of this method, you have to set the Model yourself (see Arrange above)

            // Assert
            builder.ToString().Should().Be("HelloWorld");
        }

        [Fact]
        public void CanRenderPlainTemplateUsingTemplateFileManager()
        {
            // Arrange
            var template = new MyPlainTemplate();
            var baseTemplate = new T4PlusGeneratedTemplateBase();
            var templateFileManager = new TemplateFileManager(b => baseTemplate.GenerationEnvironment = b, () => baseTemplate.GenerationEnvironment);

            // Act
            TemplateRenderHelper.RenderTemplate(template, templateFileManager);

            // Assert
            templateFileManager.MultipleContentBuilder.Contents.First().Builder.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void CanRenderErrorTemplateUsingTemplateFileManager()
        {
            // Arrange
            var template = new MyErrorTemplate();
            var baseTemplate = new T4PlusGeneratedTemplateBase();
            var templateFileManager = new TemplateFileManager(b => baseTemplate.GenerationEnvironment = b, () => baseTemplate.GenerationEnvironment);

            // Act
            this.Invoking(_ => TemplateRenderHelper.RenderTemplate(template, templateFileManager)).Should().Throw<AggregateException>();
        }

        [Fact]
        public void CanRenderModeledTemplateUsingTemplateFileManager()
        {
            // Arrange
            var template = new MyModeledTemplate();
            var baseTemplate = new T4PlusGeneratedTemplateBase();
            var templateFileManager = new TemplateFileManager(b => baseTemplate.GenerationEnvironment = b, () => baseTemplate.GenerationEnvironment);
            template.Model = "Hello world!";

            // Act
            TemplateRenderHelper.RenderTemplate(template, templateFileManager); //note that this is the same as the plain template; because initialization is not part of this method, you have to set the Model yourself (see Arrange above)

            // Assert
            templateFileManager.MultipleContentBuilder.Contents.First().Builder.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void CanRenderTemplateWithAdditionalParametersUsingTemplateFileManager()
        {
            // Arrange
            var template = new MyTemplateWithAdditionalParameters();
            var baseTemplate = new T4PlusGeneratedTemplateBase();
            var templateFileManager = new TemplateFileManager(b => baseTemplate.GenerationEnvironment = b, () => baseTemplate.GenerationEnvironment);
            template.AdditionalParameter1 = "Hello";
            template.AdditionalParameter2 = "World";

            // Act
            TemplateRenderHelper.RenderTemplate(template, templateFileManager); //note that this is the same as the plain template; because initialization is not part of this method, you have to set the Model yourself (see Arrange above)

            // Assert
            templateFileManager.MultipleContentBuilder.Contents.First().Builder.ToString().Should().Be("HelloWorld");
        }

        [Fact]
        public void CanRenderPlainTemplateUsingMultipleContentBuilder()
        {
            // Arrange
            var template = new MyPlainTemplate();
            var multipleContentBuilder = new MultipleContentBuilder();

            // Act
            TemplateRenderHelper.RenderTemplate(template, multipleContentBuilder);

            // Assert
            multipleContentBuilder.Contents.First().Builder.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void CanRenderErrorTemplateUsingMultipleContentBuilder()
        {
            // Arrange
            var template = new MyErrorTemplate();
            var multipleContentBuilder = new MultipleContentBuilder();

            // Act
            this.Invoking(_ => TemplateRenderHelper.RenderTemplate(template, multipleContentBuilder)).Should().Throw<AggregateException>();
        }

        [Fact]
        public void CanRenderModeledTemplateUsingMultipleContentBuilder()
        {
            // Arrange
            var template = new MyModeledTemplate();
            var multipleContentBuilder = new MultipleContentBuilder();
            template.Model = "Hello world!";

            // Act
            TemplateRenderHelper.RenderTemplate(template, multipleContentBuilder); //note that this is the same as the plain template; because initialization is not part of this method, you have to set the Model yourself (see Arrange above)

            // Assert
            multipleContentBuilder.Contents.First().Builder.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void CanRenderTemplateWithAdditionalParametersUsingMultipleContentBuilder()
        {
            // Arrange
            var template = new MyTemplateWithAdditionalParameters();
            var multipleContentBuilder = new MultipleContentBuilder();
            template.AdditionalParameter1 = "Hello";
            template.AdditionalParameter2 = "World";

            // Act
            TemplateRenderHelper.RenderTemplate(template, multipleContentBuilder); //note that this is the same as the plain template; because initialization is not part of this method, you have to set the Model yourself (see Arrange above)

            // Assert
            multipleContentBuilder.Contents.First().Builder.ToString().Should().Be("HelloWorld");
        }

        [Fact]
        public void CanRenderTemplateWithPocoModelAndTransformText()
        {
            // Arrange
            var template = new MyModelTemplateWithPocoModelAndTransformText();
            var builder = new StringBuilder();

            // Act
            var actual = TemplateRenderHelper.GetCompilerErrors(template, builder, "Hello world!");

            // Assert
            builder.ToString().Should().Be("Hello world!");
            actual.Should().BeEmpty();
        }

        [Fact]
        public void CanRenderTemplateWithPocoModelAndToString()
        {
            // Arrange
            var template = new MyModelTemplateWithPocoModelAndToString();
            var builder = new StringBuilder();

            // Act
            var actual = TemplateRenderHelper.GetCompilerErrors(template, builder, "Hello world!");

            // Assert
            builder.ToString().Should().Be("Hello world!");
            actual.Should().BeEmpty();
        }

        [Fact]
        public void CanRenderTemplateWithPocoAdditionalPropertiesAndTransformText()
        {
            // Arrange
            var template = new MyModelTemplateWithPocoAdditionalPropertiesAndTransformText();
            var builder = new StringBuilder();

            // Act
            var actual = TemplateRenderHelper.GetCompilerErrors(template, builder, additionalParameters: new[] { new KeyValuePair<string, object>("AdditionalParameter1", "Hello"), new KeyValuePair<string, object>("AdditionalParameter2", "World") });

            // Assert
            builder.ToString().Should().Be("HelloWorld");
            actual.Should().BeEmpty();
        }

        [Fact]
        public void CanRenderTemplateWithMultipleContentBuilderOutput()
        {
            // Arrange
            var template = new MyMultipleContentStringBuilderTemplate();
            var templateFileManager = new TemplateFileManager(b => template.GenerationEnvironment = b, () => template.GenerationEnvironment);

            // Act
            TemplateRenderHelper.RenderTemplate(template, templateFileManager, new Dictionary<string, object>());

            // Assert
            templateFileManager.MultipleContentBuilder.ToString().Should().Be(@"<?xml version=""1.0"" encoding=""utf-16""?>
<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TextTemplateTransformationFramework"">
  <BasePath i:nil=""true"" />
  <Contents>
    <Contents>
      <FileName>test.txt</FileName>
      <Lines xmlns:d4p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
        <d4p1:string>Hello world!</d4p1:string>
        <d4p1:string></d4p1:string>
      </Lines>
      <SkipWhenFileExists>false</SkipWhenFileExists>
    </Contents>
  </Contents>
</MultipleContents>");
        }

        [Fact]
        public void CanReplaceChildTemplateWithTemplateWrapper()
        {
            // Arrange
            int renderCalled = 0;
            var childTemplates = RegistrationWrapper.Create
            (
                RegistrationWrapper.WrapTemplate<MyPlainTemplate>(onRenderEvenHandler: (sender, args) => renderCalled++)
            );
            var compositionRoot = new MyComposableTemplateCompositionRoot(registration => RegistrationWrapper.Resolve(registration, childTemplates));
            var sut = compositionRoot.ResolveTemplate();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut);

            // Assert
            actual.Should().Be("Hello world!");
            renderCalled.Should().Be(1);
        }

        [Fact]
        public void CanReplaceChildTemplateWithMock()
        {
            // Arrange
            var templateMock = new Mock<ITemplate>();
            templateMock.Setup(x => x.Render(It.IsAny<StringBuilder>()))
                        .Callback<StringBuilder>(builder => builder.Append("Hello world!"));
            var childTemplates = RegistrationWrapper.Create
            (
                RegistrationWrapper.WrapTemplate<MyPlainTemplate>(templateMock.Object)
            );
            var compositionRoot = new MyComposableTemplateCompositionRoot(registration => RegistrationWrapper.Resolve(registration, childTemplates));
            var sut = compositionRoot.ResolveTemplate();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut);

            // Assert
            actual.Should().Be("Hello world!");
            templateMock.Verify(x => x.Initialize(), Times.Once);
            templateMock.Verify(x => x.Render(It.IsAny<StringBuilder>()), Times.Once);
        }

        [Fact]
        public void CanCreateNestedTemplate()
        {
            // Arrange
            var child = TemplateRenderHelper.CreateNestedTemplate<MyComposableTemplate, MyPlainTemplate>();
            var builder = new StringBuilder();

            // Act
            child.Render(builder);

            // Assert
            builder.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void CanCreateNestedTemplateContext()
        {
            // Arrange
            var context = TemplateRenderHelper.CreateNestedTemplateContext<MyComposableTemplate, MyPlainTemplate, TemplateInstanceContext>();
            var builder = new StringBuilder();

            // Act
            TemplateRenderHelper.RenderTemplate(context.Template, builder);

            // Assert
            builder.ToString().Should().Be("Hello world!");
        }

        [Fact]
        public void CanReplaceViewModelWithMock()
        {
            // Arrange
            var viewModelMock = new Mock<MyViewModel>();
            viewModelMock.SetupGet(x => x.MyExpressionProperty)
                         .Returns("Hello world!");
            var viewModels = RegistrationWrapper.Create
            (
                RegistrationWrapper.WrapViewModel<MyViewModel>(viewModelMock.Object)
            );
            var compositionRoot = new MyViewModelTemplateCompositionRoot(viewModelModifierDelegate: registration => RegistrationWrapper.Resolve(registration, viewModels));
            var sut = compositionRoot.ResolveTemplate();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut);

            // Assert
            actual.Should().Be("Hello world!");
            viewModelMock.VerifyGet(x => x.MyExpressionProperty, Times.Once);
        }

        [Fact]
        public void CanComposeTemlpateFromAnotherCompositionRoot()
        {
            // Arrange
            var compositionRoot = new MyInheritedComposableTemplateCompositionRoot();
            var sut = compositionRoot.ResolveTemplate();

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut);

            // Assert
            actual.Should().Be("Hello world!Hello world!");
        }

        [ExcludeFromCodeCoverage]
        public class MyInheritedComposableTemplateCompositionRoot : T4PlusComposableGeneratedTemplateBaseCompositionRoot<MyInheritedComposableTemplate>
        {
            public MyInheritedComposableTemplateCompositionRoot(Func<Tuple<string, Func<object>, Type>, Tuple<string, Func<object>, Type>> childTemplateModifierDelegate = null,
                                                                Func<Tuple<string, Func<object>, Type>, Tuple<string, Func<object>, Type>> viewModelModifierDelegate = null,
                                                                Action<string, Func<object>, Type> registerChildTemplateDelegate = null,
                                                                Action<string, Func<object>, Type> registerViewModelDelegate = null)
                : base(childTemplateModifierDelegate, viewModelModifierDelegate, registerChildTemplateDelegate, registerViewModelDelegate)
            {
                _ = new MyComposableTemplateCompositionRoot(registerChildTemplateDelegate: RegisterChildTemplate, registerViewModelDelegate: RegisterViewModel);
                RegisterChildTemplate("child2", () => new MyPlainTemplate());
            }
        }

        [ExcludeFromCodeCoverage]
        public class MyInheritedComposableTemplate : T4PlusComposableGeneratedTemplateBase
        {
            public void Initialize()
            {
                // Method intentionally left empty.
            }

            public void Render(StringBuilder builder)
            {
                var backup = GenerationEnvironment;
                if (builder != null) GenerationEnvironment = builder;
                RenderChildTemplate("child");
                RenderChildTemplate("child2");
                if (builder != null) GenerationEnvironment = backup;
            }
        }

        [ExcludeFromCodeCoverage]
        public class MyComposableTemplateCompositionRoot : T4PlusComposableGeneratedTemplateBaseCompositionRoot<MyComposableTemplate>
        {
            public MyComposableTemplateCompositionRoot(Func<Tuple<string, Func<object>, Type>, Tuple<string, Func<object>, Type>> childTemplateModifierDelegate = null,
                                                       Func<Tuple<string, Func<object>, Type>, Tuple<string, Func<object>, Type>> viewModelModifierDelegate = null,
                                                       Action<string, Func<object>, Type> registerChildTemplateDelegate = null,
                                                       Action<string, Func<object>, Type> registerViewModelDelegate = null)
                : base(childTemplateModifierDelegate, viewModelModifierDelegate, registerChildTemplateDelegate, registerViewModelDelegate)
            {
                RegisterChildTemplate("child", () => new MyPlainTemplate());
            }
        }

        [ExcludeFromCodeCoverage]
        public class MyComposableTemplate : T4PlusComposableGeneratedTemplateBase
        {
            public void Initialize()
            {
                // Method intentionally left empty.
            }

            public void Render(StringBuilder builder)
            {
                var backup = GenerationEnvironment;
                if (builder != null) GenerationEnvironment = builder;
                RenderChildTemplate("child");
                if (builder != null) GenerationEnvironment = backup;
            }
        }

        [ExcludeFromCodeCoverage]
        public class MyViewModelTemplateCompositionRoot : T4PlusComposableGeneratedTemplateBaseCompositionRoot<MyViewModelTemplate>
        {
            public MyViewModelTemplateCompositionRoot(Func<Tuple<string, Func<object>, Type>, Tuple<string, Func<object>, Type>> childTemplateModifierDelegate = null, Func<Tuple<string, Func<object>, Type>, Tuple<string, Func<object>, Type>> viewModelModifierDelegate = null)
                : base(childTemplateModifierDelegate, viewModelModifierDelegate)
            {
                RegisterViewModel("MyViewModel", () => new MyViewModel());
            }
        }

        [ExcludeFromCodeCoverage]
        public class MyViewModelTemplate : T4PlusComposableGeneratedTemplateBase
        {
            public void Initialize()
            {
                ViewModel = (MyViewModel)GetViewModel("MyViewModel");
            }

            public void Render(StringBuilder builder)
            {
                var backup = GenerationEnvironment;
                if (builder != null) GenerationEnvironment = builder;
                this.Write(ViewModel.MyExpressionProperty);
                if (builder != null) GenerationEnvironment = backup;
            }

            public MyViewModel ViewModel { get; set; }
        }

        [ExcludeFromCodeCoverage]
        public class MyViewModel
        {
            public virtual string MyExpressionProperty => "Some very complex value";
        }

        [ExcludeFromCodeCoverage]
        private class MyPlainTemplate : T4PlusGeneratedTemplateBase
        {
            public void Render(StringBuilder builder)
            {
                builder.Append("Hello world!");
            }
        }

        [ExcludeFromCodeCoverage]
        private class MyModelTemplateWithPocoModelAndTransformText
        {
#pragma warning disable S3459 // Unassigned members should be removed
#pragma warning disable S1144 // Unused private types or members should be removed
            public string Model { get; set; }
#pragma warning restore S1144 // Unused private types or members should be removed
#pragma warning restore S3459 // Unassigned members should be removed

#pragma warning disable S1144 // Unused private types or members should be removed
            public string TransformText()
            {
                return Model;
            }
#pragma warning restore S1144 // Unused private types or members should be removed
        }

        [ExcludeFromCodeCoverage]
        private class MyModelTemplateWithPocoModelAndToString
        {
#pragma warning disable S3459 // Unassigned members should be removed
#pragma warning disable S1144 // Unused private types or members should be removed
            public string Model { get; set; }
#pragma warning restore S1144 // Unused private types or members should be removed
#pragma warning restore S3459 // Unassigned members should be removed

            public override string ToString()
            {
                return Model;
            }
        }

        [ExcludeFromCodeCoverage]
        private class MyModelTemplateWithPocoAdditionalPropertiesAndTransformText
        {
#pragma warning disable S1144 // Unused private types or members should be removed
#pragma warning disable S3459 // Unassigned members should be removed
            public string AdditionalParameter1 { get; set; }
#pragma warning restore S3459 // Unassigned members should be removed
#pragma warning restore S1144 // Unused private types or members should be removed
#pragma warning disable S1144 // Unused private types or members should be removed
#pragma warning disable S3459 // Unassigned members should be removed
            public string AdditionalParameter2 { get; set; }
#pragma warning restore S3459 // Unassigned members should be removed
#pragma warning restore S1144 // Unused private types or members should be removed

#pragma warning disable S1144 // Unused private types or members should be removed
            public string TransformText()
            {
                return string.Concat(AdditionalParameter1, AdditionalParameter2);
            }
#pragma warning restore S1144 // Unused private types or members should be removed
        }

        private class MyErrorTemplate : T4PlusGeneratedTemplateBase
        {
#pragma warning disable S1144 // Unused private types or members should be removed
            public void Initialize()
            {
                Error("Something is wrong!");
            }
#pragma warning restore S1144 // Unused private types or members should be removed

#pragma warning disable S1144 // Unused private types or members should be removed
            public void Render(StringBuilder builder) //if we don't add this method, ToString() will be called on the template, even though we have an error in initialization...
            {
                builder.Append("Not supposed to be called because initialization produced errors");
            }
#pragma warning restore S1144 // Unused private types or members should be removed
        }

        [ExcludeFromCodeCoverage]
        private class MyModeledTemplate : T4PlusGeneratedTemplateBase
        {
            public string Model { get; set; }

#pragma warning disable S1144 // Unused private types or members should be removed
            public void Initialize()
            {
                if (Session?.ContainsKey("Model") == true)
                {
                    Model = (string)Session["Model"];
                }
            }
#pragma warning restore S1144 // Unused private types or members should be removed

#pragma warning disable S1144 // Unused private types or members should be removed
            public void Render(StringBuilder builder)
            {
                builder.Append(Model);
            }
#pragma warning restore S1144 // Unused private types or members should be removed
        }

        [ExcludeFromCodeCoverage]
        private class MyTemplateWithAdditionalParameters : T4PlusGeneratedTemplateBase
        {
            public string AdditionalParameter1 { get; set; }
            public string AdditionalParameter2 { get; set; }

#pragma warning disable S1144 // Unused private types or members should be removed
            public void Initialize()
            {
                if (Session?.ContainsKey("AdditionalParameter1") == true)
                {
                    AdditionalParameter1 = (string)Session["AdditionalParameter1"];
                }
                if (Session?.ContainsKey("AdditionalParameter2") == true)
                {
                    AdditionalParameter2 = (string)Session["AdditionalParameter2"];
                }
            }
#pragma warning restore S1144 // Unused private types or members should be removed

#pragma warning disable S1144 // Unused private types or members should be removed
            public void Render(StringBuilder builder)
            {
                builder.Append(AdditionalParameter1);
                builder.Append(AdditionalParameter2);
            }
#pragma warning restore S1144 // Unused private types or members should be removed
        }

        private class MyMultipleContentStringBuilderTemplate : T4PlusGeneratedTemplateBase
        {
#pragma warning disable S1144 // Unused private types or members should be removed
            public void Render(StringBuilder builder)
            {
                var b = new MultipleContentBuilder();
                b.AddContent("test.txt").Builder.Append("Hello world!");
                builder.Append(b.ToString());
            }
#pragma warning restore S1144 // Unused private types or members should be removed
        }
    }
}
