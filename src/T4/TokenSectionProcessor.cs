using System;
using TextTemplateTransformationFramework.Common;
using TextTemplateTransformationFramework.T4.Contracts;

namespace TextTemplateTransformationFramework.T4
{
    public class TokenSectionProcessor : ITokenSectionProcessor<TokenParserState>
    {
        public ProcessSectionResult<TokenParserState> Process(SectionContext<TokenParserState> context,
                                                              Type sectionProcessorType,
                                                              SectionProcessResult<TokenParserState> sectionProcessResult)
            => ProcessSectionResult.Create
            (
                context,
                sectionProcessResult?.CustomProcessorType ?? sectionProcessorType,
                sectionProcessResult
            );
    }
}
