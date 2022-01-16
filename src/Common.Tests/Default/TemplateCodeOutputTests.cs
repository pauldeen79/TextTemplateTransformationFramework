using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using TextTemplateTransformationFramework.Common.Contracts;
using TextTemplateTransformationFramework.Common.Default;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.Default
{
    [ExcludeFromCodeCoverage]
    public class TemplateCodeOutputTests
    {
        [Fact]
        public void Ctor_Throws_On_Null_CodeGeneratorResult()
        {
            // Arrange
            var action = new Action(() => _ = new TemplateCodeOutput<TemplateCodeOutputTests>(Enumerable.Empty<ITemplateToken<TemplateCodeOutputTests>>(), null, "cs", Enumerable.Empty<string>(), Enumerable.Empty<string>(), "GeneratedClass", "C:\\Temp"));

            // Act
            action.Should().Throw<ArgumentNullException>().WithParameterName("codeGeneratorResult");
        }
    }
}
