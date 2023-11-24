using TextTemplateTransformationFramework.Common.Requests;
using Xunit;

namespace TextTemplateTransformationFramework.Common.Tests.Requests
{
    public class ExtractParametersFromTextTemplateRequestTests : TestBase
    {
        [Fact]
        public void Ctor_Throws_On_Null_Context()
        {
            ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(typeof(ExtractParametersFromTextTemplateRequest<ExtractParametersFromTextTemplateRequestTests>));
        }
    }
}
