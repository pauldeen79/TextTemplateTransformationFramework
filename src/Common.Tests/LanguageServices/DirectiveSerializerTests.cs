using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using Moq;
using SparkyTestHelpers.Moq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.Extensions;
using TextTemplateTransformationFramework.Common.LanguageServices;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.LanguageServices
{
    [ExcludeFromCodeCoverage]
    public class DirectiveSerializerTests
    {
        [Fact]
        public void CanDeserializeFromDirectiveToObject()
        {
            // Arrange
            const string Section = @"@ Directive name=""name 1"" optionalBoolean=""false"" language=""vb"" list=""a"" list=""b"" list=""c""";
            var callback = CreateTokenParserCallback();
            callback.Setup(x => x.GetSectionArguments(Any.InstanceOf<SectionContext<DirectiveSerializerTests>>(), Any.String)).Returns<SectionContext<DirectiveSerializerTests>, string>((_, name) =>
            {
                return name.ToLower(CultureInfo.InvariantCulture) switch
                {
                    "name" => new[] { "name 1" },
                    "optionalboolean" => new[] { "false" },
                    "language" => new[] { "vb" },
                    "list" => new[] { "a", "b", "c" },
                    _ => Array.Empty<string>(),
                };
            });
            var context = SectionContext.FromSection
            (
                Section,
                1,
                1,
                "",
                Array.Empty<ITemplateToken<DirectiveSerializerTests>>(),
                callback.Object,
                this,
                new Logger(),
                Array.Empty<TemplateParameter>()
            );
            var fileNameProvider = new Mock<IFileNameProvider>().Object;
            var fileContentsProvider = new Mock<IFileContentsProvider>().Object;
            var templateCodeCompiler = new Mock<ITemplateCodeCompiler<DirectiveSerializerTests>>().Object;
            var sut = new DirectiveSerializer<DirectiveSerializerTests, TestModel>(context, fileNameProvider, fileContentsProvider, templateCodeCompiler);

            // Act
            var actual = sut.Deserialize();

            // Assert
            actual.Name.Should().Be("name 1");
            actual.OptionalBoolean.Should().Be(false);
            actual.List.Length.Should().Be(3);
            actual.List[0].Should().Be("a");
            actual.List[1].Should().Be("b");
            actual.List[2].Should().Be("c");
            actual.Language.Should().Be(Language.VbNet);
        }

        [Fact]
        public void CanSerializeObjectToDirective()
        {
            // Arrange
            var context = SectionContext<DirectiveSerializerTests>.Empty;
            var fileNameProvider = new Mock<IFileNameProvider>().Object;
            var fileContentsProvider = new Mock<IFileContentsProvider>().Object;
            var templateCodeCompiler = new Mock<ITemplateCodeCompiler<DirectiveSerializerTests>>().Object;
            var sut = new DirectiveSerializer<DirectiveSerializerTests, TestModel>(context, fileNameProvider, fileContentsProvider, templateCodeCompiler);

            // Act
            var actual = sut.Serialize(new TestModel { Name = "name1", OptionalBoolean = false, List = new[] { "a", "b", "c" }, Language = Language.VbNet });

            // Assert
            actual.Should().Contain(@"Name=""name1"" Enabled=""false"" OptionalBoolean=""false"" List=""a"" List=""b"" List=""c"" Language=""vb""");
        }

        [Fact]
        public void CanSerializeObjectToDirectiveWithDoubleQuote()
        {
            // Arrange
            var context = SectionContext<DirectiveSerializerTests>.Empty;
            var fileNameProvider = new Mock<IFileNameProvider>().Object;
            var fileContentsProvider = new Mock<IFileContentsProvider>().Object;
            var templateCodeCompiler = new Mock<ITemplateCodeCompiler<DirectiveSerializerTests>>().Object;
            var sut = new DirectiveSerializer<DirectiveSerializerTests, TestModel>(context, fileNameProvider, fileContentsProvider, templateCodeCompiler);

            // Act
            var actual = sut.Serialize(new TestModel { Name = "name1\"", OptionalBoolean = false, List = new[] { "a", "b", "c" }, Language = Language.VbNet });

            // Assert
            actual.Should().Contain(@"Name=""name1\"""" Enabled=""false"" OptionalBoolean=""false"" List=""a"" List=""b"" List=""c"" Language=""vb""");
        }

        [Fact]
        public void CanSerializeObjectWithFloatPropertyToDirective()
        {
            // Arrange
            var context = SectionContext<DirectiveSerializerTests>.Empty;
            var fileNameProvider = new Mock<IFileNameProvider>().Object;
            var fileContentsProvider = new Mock<IFileContentsProvider>().Object;
            var templateCodeCompiler = new Mock<ITemplateCodeCompiler<DirectiveSerializerTests>>().Object;
            var sut = new DirectiveSerializer<DirectiveSerializerTests, FloatSinglePropertyTypeModel>(context, fileNameProvider, fileContentsProvider, templateCodeCompiler);

            // Act
            var actual = sut.Serialize(new FloatSinglePropertyTypeModel { MyFloatProperty = 2.3f });

            // Assert
            actual.Should().Be(@"MyFloatProperty=""2.3""");
        }

        [Fact]
        public void CanSerializeObjectWithGenericListPropertyToDirective()
        {
            // Arrange
            var context = SectionContext<DirectiveSerializerTests>.Empty;
            var fileNameProvider = new Mock<IFileNameProvider>().Object;
            var fileContentsProvider = new Mock<IFileContentsProvider>().Object;
            var templateCodeCompiler = new Mock<ITemplateCodeCompiler<DirectiveSerializerTests>>().Object;
            var sut = new DirectiveSerializer<DirectiveSerializerTests, FloatMultiplePropertyTypeModel>(context, fileNameProvider, fileContentsProvider, templateCodeCompiler);

            // Act
            var actual = sut.Serialize(new FloatMultiplePropertyTypeModel { MyFloatListProperty = new[] { 2.3f, 2.4f, 2.5f }.ToList() });

            // Assert
            actual.Should().Be(@"MyFloatListProperty=""2.3"" MyFloatListProperty=""2.4"" MyFloatListProperty=""2.5""");
        }

        [Fact]
        public void CanDeserializeFromDirectiveToFloatProperty()
        {
            // Arrange
            const string Section = @"@ Directive MyFloatProperty=""2.3""";
            var callback = CreateTokenParserCallback();
            callback.Setup(x => x.GetSectionArguments(Any.InstanceOf<SectionContext<DirectiveSerializerTests>>(), Any.String)).Returns<SectionContext<DirectiveSerializerTests>, string>((_, name) =>
            {
                return name.ToLower(CultureInfo.InvariantCulture) switch
                {
                    "myfloatproperty" => new[] { "2.3" },
                    _ => Array.Empty<string>(),
                };
            });
            var context = SectionContext.FromSection
            (
                Section,
                1,
                1,
                "",
                Array.Empty<ITemplateToken<DirectiveSerializerTests>>(),
                callback.Object,
                this,
                new Logger(),
                Array.Empty<TemplateParameter>()
            );
            var fileNameProvider = new Mock<IFileNameProvider>().Object;
            var fileContentsProvider = new Mock<IFileContentsProvider>().Object;
            var templateCodeCompiler = new Mock<ITemplateCodeCompiler<DirectiveSerializerTests>>().Object;
            var sut = new DirectiveSerializer<DirectiveSerializerTests, FloatSinglePropertyTypeModel>(context, fileNameProvider, fileContentsProvider, templateCodeCompiler);

            // Act
            var actual = sut.Deserialize();

            // Assert
            actual.MyFloatProperty.Should().Be(2.3f);
        }

        [Fact]
        public void CannotDeserializeFromDirectiveToGenericListFloatProperty()
        {
            // Arrange
            const string Section = @"@ Directive MyFloatListProperty=""2.3"" MyFloatListProperty=""2.4"" MyFloatListProperty=""2.5""";
            var callback = CreateTokenParserCallback();
            var context = SectionContext.FromSection
            (
                Section,
                1,
                1,
                "",
                Array.Empty<ITemplateToken<DirectiveSerializerTests>>(),
                callback.Object,
                this,
                new Logger(),
                Array.Empty<TemplateParameter>()
            );
            var fileNameProvider = new Mock<IFileNameProvider>().Object;
            var fileContentsProvider = new Mock<IFileContentsProvider>().Object;
            var templateCodeCompiler = new Mock<ITemplateCodeCompiler<DirectiveSerializerTests>>().Object;
            var sut = new DirectiveSerializer<DirectiveSerializerTests, FloatMultiplePropertyTypeModel>(context, fileNameProvider, fileContentsProvider, templateCodeCompiler);

            // Act
            sut.Invoking(x => x.Deserialize())
               .Should().Throw<InvalidCastException>();
        }

        [Fact]
        public void CanDeserializeFromDirectiveToStringArrayProperty()
        {
            // Arrange
            const string Section = @"@ Directive MyStringArrayProperty=""2.3"" MyStringArrayProperty=""2.4"" MyStringArrayProperty=""2.5""";
            var callback = CreateTokenParserCallback();
            callback.Setup(x => x.GetSectionArguments(Any.InstanceOf<SectionContext<DirectiveSerializerTests>>(), Any.String)).Returns<SectionContext<DirectiveSerializerTests>, string>((_, name) =>
            {
                return name.ToLower(CultureInfo.InvariantCulture) switch
                {
                    "mystringarrayproperty" => new[] { "2.3", "2.4", "2.5" },
                    _ => Array.Empty<string>(),
                };
            });
            var context = SectionContext.FromSection
            (
                Section,
                1,
                1,
                "",
                Array.Empty<ITemplateToken<DirectiveSerializerTests>>(),
                callback.Object,
                this,
                new Logger(),
                Array.Empty<TemplateParameter>()
            );
            var fileNameProvider = new Mock<IFileNameProvider>().Object;
            var fileContentsProvider = new Mock<IFileContentsProvider>().Object;
            var templateCodeCompiler = new Mock<ITemplateCodeCompiler<DirectiveSerializerTests>>().Object;
            var sut = new DirectiveSerializer<DirectiveSerializerTests, StringMultiplePropertyTypeModel>(context, fileNameProvider, fileContentsProvider, templateCodeCompiler);

            // Act
            var actual = sut.Deserialize();

            // Assert
            actual.MyStringArrayProperty.Length.Should().Be(3);
            actual.MyStringArrayProperty[0].Should().Be("2.3");
            actual.MyStringArrayProperty[1].Should().Be("2.4");
            actual.MyStringArrayProperty[2].Should().Be("2.5");
        }

        private static Mock<ITokenParserCallback<DirectiveSerializerTests>> CreateTokenParserCallback()
        {
            var mock = new Mock<ITokenParserCallback<DirectiveSerializerTests>>();
            mock.Setup(x => x.Parse(Any.InstanceOf<ITextTemplateProcessorContext<DirectiveSerializerTests>>()))
                .Returns(Array.Empty<ITemplateToken<DirectiveSerializerTests>>());
            mock.Setup(x => x.SectionIsDirectiveWithName(Any.InstanceOf<SectionContext<DirectiveSerializerTests>>(), Any.String))
                .Returns<SectionContext<DirectiveSerializerTests>, string>((context, name) => context.Section.IsDirective(name, "@ ", " "));
            mock.Setup(x => x.SectionStartsWithPrefix(Any.InstanceOf<SectionContext<DirectiveSerializerTests>>(), Any.String))
                .Returns<SectionContext<DirectiveSerializerTests>, string>((context, prefix) => context.Section.StartsWith(prefix));
            return mock;
        }

        private class FloatSinglePropertyTypeModel
        {
            public float MyFloatProperty { get; set; }
        }

        private class FloatMultiplePropertyTypeModel
        {
            public List<float> MyFloatListProperty { get; set; }
        }

        private class StringMultiplePropertyTypeModel
        {
#pragma warning disable S1144 // Unused private types or members should be removed
#pragma warning disable S3459 // Unassigned members should be removed
            public string[] MyStringArrayProperty { get; set; }
#pragma warning restore S3459 // Unassigned members should be removed
#pragma warning restore S1144 // Unused private types or members should be removed
        }
    }
}
