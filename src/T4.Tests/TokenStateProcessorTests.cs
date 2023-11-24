using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Contracts.TemplateSectionProcessors;
using TextTemplateTransformationFramework.T4.Contracts;
using Xunit;

namespace TextTemplateTransformationFramework.T4.Tests
{
    [ExcludeFromCodeCoverage]
    public class TokenStateProcessorTests : TestBase
    {
        private readonly Mock<ICompositeTemplateSectionProcessor<TokenParserState>> _sectionProcessorMock = new();
        private readonly Mock<ICodeSectionProcessor<TokenParserState>> _codeSectionProcessorMock = new();
        private readonly Mock<ITextSectionProcessor<TokenParserState>> _textSectionProcessorMock = new();
        private readonly Mock<ITokenSectionProcessor<TokenParserState>> _tokenSectionProcessorMock = new();

        [Fact]
        public void Ctor_Throws_On_Null_Arguments()
        {
            ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(typeof(TokenStateProcessor));
        }

        [Fact]
        public void Process_Throws_On_Null_State()
        {
            // Arrange
            var sut = new TokenStateProcessor(_sectionProcessorMock.Object, _codeSectionProcessorMock.Object, _textSectionProcessorMock.Object, _tokenSectionProcessorMock.Object);

            // Act
            sut.Invoking(x => x.Process(null, null, null, null))
               .Should().Throw<ArgumentNullException>().WithParameterName("state");
        }
    }
}
