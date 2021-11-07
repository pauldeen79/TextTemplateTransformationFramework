namespace TextTemplateTransformationFramework.Common.Contracts
{
    public interface IProcessFinalizer<in TContext>
        where TContext : class
    {
        void Finalize(TContext context);
    }
}
