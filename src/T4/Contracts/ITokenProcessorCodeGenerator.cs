using TextTemplateTransformationFramework.T4.Requests;

namespace TextTemplateTransformationFramework.T4.Contracts
{
    public interface ITokenProcessorCodeGenerator<TState>
        where TState : class
    {
        CodeGeneratorResultBuilder Generate(GenerateCodeRequest<TState> request);
    }
}
