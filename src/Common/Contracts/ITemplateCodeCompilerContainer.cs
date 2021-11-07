namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface ITemplateCodeCompilerContainer<TState>
        where TState : class
    {
        ITemplateCodeCompiler<TState> TemplateCodeCompiler { get; set; }
    }
}
