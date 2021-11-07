using System.Collections.Generic;

namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface ITemplateCompilerOutputValidator<TState>
        where TState : class
    {
        bool Validate(ITemplateProcessorContext<TState> context, out IEnumerable<CompilerError> compilerErrorCollection);
    }
}
