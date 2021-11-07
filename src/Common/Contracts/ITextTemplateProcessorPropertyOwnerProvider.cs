using System.ComponentModel;

namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface ITextTemplateProcessorPropertyOwnerProvider<TState>
        where TState : class
    {
        object Get(ITextTemplateProcessorContext<TState> context, PropertyDescriptor property, TemplateCompilerOutput<TState> templateCompilerOutput);
    }
}
