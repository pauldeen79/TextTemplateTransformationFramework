namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface ITemplateToken<TState>
        where TState : class
    {
        SectionContext<TState> SectionContext { get; }
        int LineNumber { get; }
        string FileName { get; }
    }
}
