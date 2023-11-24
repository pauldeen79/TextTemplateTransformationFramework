using System;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using TextTemplateTransformationFramework.Common.Default.TemplateTokens;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Plus.Tests
{
    public class TokenParserTests : TestBase
    {
        private readonly ITextTemplateTokenParser<TokenParserState> _baseParser;
        private readonly ILogger _loggerMock;

        public TokenParserTests()
        {
            _baseParser = Fixture.Freeze<ITextTemplateTokenParser<TokenParserState>>();
            _loggerMock = Fixture.Freeze<ILogger>();
        }

        [Fact]
        public void Parse_Throws_On_Null_Context()
        {
            // Arrange
            var sut = new TokenParser(_baseParser);

            // Act & Assert
            sut.Invoking(x => x.Parse(null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Parse_Adds_TemplateSection_When_Not_Present_In_Source_Template()
        {
            // Arrange
            _baseParser.Parse(Arg.Any<ITextTemplateProcessorContext<TokenParserState>>())
                       .Returns(x => x.ArgAt<ITextTemplateProcessorContext<TokenParserState>>(0).TextTemplate.Template == "<#@ template language=\"C#\" #>"
                        ? new ITemplateToken<TokenParserState>[] { new TemplateClassNameToken<TokenParserState>(SectionContext<TokenParserState>.Empty, "MyClassName", "") }
                        : Array.Empty<ITemplateToken<TokenParserState>>());
            var sut = new TokenParser(_baseParser);

            // Act
            var actual = sut.Parse(new TextTemplateProcessorContext<TokenParserState>(new TextTemplate(string.Empty), Array.Empty<TemplateParameter>(), _loggerMock, null));

            // Assert
            actual.Should().HaveCount(1);
            // Note that if you would use a real implementation of the base parser, there would be a token of type SourceSectionToken<TokenParserState> with a bunch of tokens inside.
            // But we are unit testing here, we just want to see that the call to the base parser is made with the correct arguments
            actual.First().Should().BeOfType<TemplateClassNameToken<TokenParserState>>()
                          .And.Match(x => ((TemplateClassNameToken<TokenParserState>)x).ClassName == "MyClassName");
        }
    }
}
