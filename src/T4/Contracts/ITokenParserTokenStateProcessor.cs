using TextTemplateTransformationFramework.Common.Contracts;

namespace TextTemplateTransformationFramework.T4.Contracts
{
    public interface ITokenParserTokenStateProcessor<TState>
        where TState : class
    {
        ProcessSectionResult<TState> Process(TokenParserState state, ITokenParserCallback<TState> tokenParserCallback, ILogger logger);
    }
}
